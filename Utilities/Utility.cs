using System.Security.Cryptography;
using System.Text;
namespace bazyProjektBlazor.Utilities
{
	public class Utility
	{
		public static string EncryptSHA256(string password)
		{
			byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

			return Convert.ToBase64String(hashBytes);
		}
		public static string Capitalise(string text)
		{

			return $"{text[0].ToString().ToUpper()}{text[1..]}";
		}
	}
}
