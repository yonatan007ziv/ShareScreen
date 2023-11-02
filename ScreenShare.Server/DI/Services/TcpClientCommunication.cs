using Microsoft.Extensions.Logging;
using ShareScreen.Core;
using ShareScreen.Core.DI.Interfaces;
using ShareScreen.Core.DI.Services;
using ShareScreen.Core.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace ScreenShare.Server.DI.Services;

internal class TcpClientCommunication : BaseTcpCommunication
{
	private readonly ILogger logger;

	public TcpClientCommunication(TcpClient client, ISerializer encoder, ILogger logger)
		: base(client, encoder, logger)
	{
		this.logger = logger;
		_ = InitializeEncryption();
	}

	public async new Task WriteMessage(Message msg)
	{
		try
		{
			await base.WriteMessage(msg);
		}
		catch (NetworkedWriteException) { Disconnect(); }
	}

	/// <returns> <see cref="null"/> if error occured </returns>
	public async new Task<Message?> ReadMessage()
	{
		Message result;
		try
		{
			result = await base.ReadMessage();
		}
		catch (NetworkedReadException) { Disconnect(); return null; };

		return result;
	}

	protected override async Task<bool> EstablishEncryption()
	{
		try
		{
			// Import Rsa Details
			byte[] rsaPublicKey = await ReadBytes();
			encryption.ImportRsa(rsaPublicKey);

			// Send Aes Details
			byte[] aesPrivateKey = encryption.ExportAesPrivateKey();
			byte[] encryptedRsaPrivateKey = encryption.EncryptRsa(aesPrivateKey);
			_ = WriteBytes(encryptedRsaPrivateKey);
			byte[] aesIv = encryption.ExportAesIv();
			byte[] encryptedRsaIv = encryption.EncryptRsa(aesIv);
			_ = WriteBytes(encryptedRsaIv);

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
		logger.LogInformation("Encryption Successful");
		return true;
	}

	public void Disconnect()
	{
		client.Close();
		disconnectedCts.Cancel();
	}
}