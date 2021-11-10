using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace BarrPriest.MPs.Interests.Examine.Cli
{
    public class CompatibleDocument
    {
        private readonly string content;

        private readonly int chunkSize;

        public CompatibleDocument(string content, int chunkSize)
        {
            this.content = content;

            this.chunkSize = chunkSize;
        }

        public int Length()
        {
            return this.content.Length;
        }

        public IEnumerable<string> DocumentChunks()
        {
            var chunks =  this.content.ToCharArray().Batch(chunkSize);

            foreach (var chunk in chunks)
            {
                yield return new string(chunk.ToArray());
            }
        }
    }
}
