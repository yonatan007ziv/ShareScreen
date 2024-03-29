﻿using Microsoft.Extensions.Logging;
using ScreenShare.WPF.DI.Interfaces;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using ShareScreen.Core.DI.Services;
using ShareScreen.Core.Exceptions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.WPF.DI.Services;

class TcpClientCommunication : BaseTcpCommunication, IClient
{
	private readonly ILogger logger;

	public TcpClientCommunication(ISerializer encoder, ILogger logger)
		: base(new TcpClient(), encoder, logger)
	{
		this.logger = logger;
	}

	public async new Task WriteMessage(Message msg)
	{
		try
		{
			await base.WriteMessage(msg);
		}
		catch (NetworkedWriteException) { Disconnect(); }
	}

	public async new Task<Message?> ReadMessage()
	{
		Message result;
		try
		{
			result = await base.ReadMessage();
		}
		catch (NetworkedReadException ex) when (ex.Type == ReadExceptionType.Timedout) { Disconnect(); return null; }
		catch (NetworkedReadException) { Disconnect(); return null; }

		return result;
	}

	protected override async Task<bool> EstablishEncryption()
	{
		try
		{
			// Send Rsa Details
			byte[] rsaPublicKey = encryption.ExportRsa();
			_ = WriteBytes(rsaPublicKey);

			// Import Aes Details
			byte[] encryptedAesPrivateKey = await ReadBytes();
			byte[] aesPrivateKey = encryption.DecryptRsa(encryptedAesPrivateKey);
			encryption.ImportAesPrivateKey(aesPrivateKey);
			byte[] encryptedAesIv = await ReadBytes();
			byte[] aesIv = encryption.DecryptRsa(encryptedAesIv);
			encryption.ImportAesIv(aesIv);

			// Test Encryption: Send
			string msgTest = EncryptionTestWord;
			byte[] decryptedTest = Encoding.UTF8.GetBytes(msgTest);
			byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
			_ = WriteBytes(encryptedTest);

			// Test Encryption: Receive
			encryptedTest = await ReadBytes();
			decryptedTest = encryption.DecryptAes(encryptedTest);
			msgTest = Encoding.UTF8.GetString(decryptedTest);

			if (msgTest != EncryptionTestWord)
				throw new EncryptionFailedException();
		}
		catch (Exception ex)
		{
			logger.LogError($"Failed Establishing Encryption: {ex.Message}");
			disconnectedCts.Cancel();
			return false;
		}
		return true;
	}

	public async Task<bool> Connect(IPAddress addr, int port)
	{
		try
		{
			await client.ConnectAsync(addr, port);
			await InitializeEncryption();
			return true;
		}
		catch (SocketException ex) { logger.LogError($"Connection Failed: {ex.Message}"); }
		catch (EncryptionFailedException ex) { logger.LogError($"Encryption Failed: {ex.Message}"); }
		catch (NetworkedWriteException ex) { logger.LogError($"Disconnected (While Write): {ex.Message}"); }
		catch (NetworkedReadException ex) { logger.LogError($"Disconnected (While Read): {ex.Message}"); }
		catch (CryptographicException ex) { logger.LogError($"Disconnected (While initializing Encryption): {ex.Message}"); }
		return false;
	}

	public void Disconnect()
		=> Dispose();
}