using System.Security.Cryptography;
using System.Text;

namespace Boxtorio.Common;

public static class HashHelper
{
	public static string GetHash(string input)
    {
        var data = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        var sb = new StringBuilder();
        foreach (var t in data)
        {
            sb.Append(t.ToString("x2"));
        }

        return sb.ToString();
    }

    public static bool Verify(string input, string hash)
	{
		var hashImput = GetHash(input);
		var comparer = StringComparer.OrdinalIgnoreCase;
		return comparer.Compare(hashImput, hash) == 0;
	}
}
