using System.Text;
using System.Security.Cryptography;
namespace bazyProjektBlazor.Utilities
{
	public class HashPassword
	{
		public static string EncryptSHA256(string password)
		{
			byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

			return Convert.ToBase64String(hashBytes);
		}
	}
}
