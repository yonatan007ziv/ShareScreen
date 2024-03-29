﻿using Microsoft.Extensions.Logging;
using ShareScreen.Core.DI.Interfaces;
using ShareScreen.Core.Exceptions;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace ShareScreen.Core.DI.Services;

public abstract class BaseTcpCommunication : ICommunication, IDisposable
{
	protected const string EncryptionTestWord = "Success";

	private readonly ISerializer serializer;
	private readonly ILogger logger;

	private NetworkStream Ns { get => client.GetStream(); }
	protected TcpClient client;
	protected IEncryptionHandler encryption;
	protected CancellationTokenSource disconnectedCts;
	protected TaskCompletionSource encryptionTask;

	public BaseTcpCommunication(TcpClient client, ISerializer serializer, ILogger logger)
	{
		this.client = client;
		this.serializer = serializer;
		this.logger = logger;

		encryption = new EncryptionHandler();
		disconnectedCts = new CancellationTokenSource();
		encryptionTask = new TaskCompletionSource();
	}

	protected abstract Task<bool> EstablishEncryption();

	public async Task InitializeEncryption()
	{
		if (!await EstablishEncryption())
			throw new EncryptionFailedException();
		encryptionTask.SetResult();
	}

	public async Task WriteMessage(Message msg)
	{
		await encryptionTask.Task;
		WriteExceptionType writeExceptionType;

		try
		{
			string encodedMessage = serializer.Serialize(msg)
				?? throw new SerializationException(msg);
			byte[] decryptedWriteBuffer = Encoding.UTF8.GetBytes(encodedMessage);
			byte[] writeBuffer = encryption.EncryptAes(decryptedWriteBuffer);
			await WriteBytes(writeBuffer);
			return;
		}
		catch (SerializationException ex) { logger.LogError($"Serialization Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.SerializationFailed; }
		catch (EncoderFallbackException ex) { logger.LogError($"Encoding Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.TextEncodingFailed; }
		catch (CryptographicException ex) { logger.LogError($"Cryptographic Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.EncryptionFailed; }
		catch (IOException ex) { logger.LogError($"IO Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.IO; }
		catch (ObjectDisposedException ex) { logger.LogError($"Disposed Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.Disposed; }
		catch (TimeoutException ex) { logger.LogError($"Timeout Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.Timedout; }
		catch (OperationCanceledException ex) { logger.LogError($"Operation Cancelled Exception: {ex.Message}"); writeExceptionType = WriteExceptionType.OperationCancelled; }
		catch (Exception ex) { logger.LogCritical($"MISHANDLED EXCEPTION: {ex.Message}"); throw new Exception("MISHANDLED"); }

		disconnectedCts.Cancel();
		throw new NetworkedWriteException(writeExceptionType);
	}

	public async Task<Message> ReadMessage()
	{
		await encryptionTask.Task;
		ReadExceptionType readExceptionType;

		try
		{
			byte[] encryptedReadBuffer = await ReadBytes();
			byte[] readBuffer = encryption.DecryptAes(encryptedReadBuffer);
			string message = Encoding.UTF8.GetString(readBuffer);
			return serializer.Deserialize<Message>(message)
				?? throw new DeserializationException(message);
		}
		catch (NetworkedReadException ex) when (ex.Type == ReadExceptionType.Disconnected) { throw; }
		catch (IOException ex) { logger.LogError($"IO Exception: {ex.Message}"); readExceptionType = ReadExceptionType.IO; }
		catch (TimeoutException ex) { logger.LogError($"Timeout Exception: {ex.Message}"); readExceptionType = ReadExceptionType.Timedout; }
		catch (DisconnectedException ex) { logger.LogError($"Disconnected Exception: {ex.Message}"); readExceptionType = ReadExceptionType.Disconnected; }
		catch (NetworkedReadException ex) { logger.LogError($"Read Exception: {ex.Message}"); readExceptionType = ex.Type; }
		catch (CryptographicException ex) { logger.LogError($"Cryptographic Exception: {ex.Message}"); readExceptionType = ReadExceptionType.EncryptionFailed; }
		catch (DecoderFallbackException ex) { logger.LogError($"Decoding Exception: {ex.Message}"); readExceptionType = ReadExceptionType.TextDecodingFailed; }
		catch (DeserializationException ex) { logger.LogError($"Deserialization Exception: {ex.Message}"); readExceptionType = ReadExceptionType.DeserializationFailed; }
		catch (ObjectDisposedException ex) { logger.LogError($"Disposed Exception: {ex.Message}"); readExceptionType = ReadExceptionType.Disposed; }
		catch (Exception ex) { logger.LogCritical($"MISHANDLED EXCEPTION: {ex.Message}"); throw new Exception("MISHANDLED"); }

		disconnectedCts.Cancel();
		throw new NetworkedReadException(readExceptionType);
	}

	protected async Task WriteBytes(byte[] writeBuffer)
	{
		// Prefixes 4 Bytes Indicating Message Length
		byte[] length = BitConverter.GetBytes(writeBuffer.Length);
		byte[] prefixedBuffer = new byte[writeBuffer.Length + sizeof(int)];

		Array.Copy(length, 0, prefixedBuffer, 0, sizeof(int));
		Array.Copy(writeBuffer, 0, prefixedBuffer, sizeof(int), writeBuffer.Length);

		try
		{
			await Ns.WriteAsync(prefixedBuffer, disconnectedCts.Token);
		}
		catch (OperationCanceledException)
		{
			throw new DisconnectedException();
		}
		catch { throw; }
	}

	protected async Task<byte[]> ReadBytes()
	{
		byte[] readBufer;
		int bytesRead;
		try
		{
			// Reads 4 Bytes Indicating Message Length
			byte[] lengthBuffer = new byte[4];
			await Ns.ReadAsync(lengthBuffer, disconnectedCts.Token);

			int length = BitConverter.ToInt32(lengthBuffer);
			readBufer = new byte[length];
			bytesRead = await Ns.ReadAsync(readBufer, disconnectedCts.Token);
		}
		catch (OperationCanceledException)
		{
			throw new DisconnectedException();
		}
		catch { throw; }

		if (bytesRead == 0)
			throw new NetworkedReadException(ReadExceptionType.Disconnected);

		return readBufer;
	}

	public void Dispose()
	{
		disconnectedCts.Cancel();
		client.Close();

		client = new TcpClient();
		encryption = new EncryptionHandler();
		disconnectedCts = new CancellationTokenSource();
		encryptionTask = new TaskCompletionSource();
	}
}