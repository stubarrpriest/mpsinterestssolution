using System.Security.Cryptography;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class GitHubPathHash
    {
        private readonly string fileExtension;

        public GitHubPathHash(string fileExtension)
        {
            this.fileExtension = fileExtension;
        }

        public string From(string mpIdentifier)
        {
            var pathForHash = $"{mpIdentifier}{this.fileExtension}";

            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(pathForHash));

            return this.HexadecimalStringFrom(hash);
        }

        private string HexadecimalStringFrom(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var item in bytes)
            {
                builder.Append(item.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
