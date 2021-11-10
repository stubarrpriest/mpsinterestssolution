using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;

namespace BarrPriest.MPs.Interests.Examine.Cli
{
    public class DocumentKey
    {
        public string MpKey { get; }

        public string PublicationSet { get; }

        public string Index { get; } = "0";

        public DocumentKey(RawHtmlData mp, int index)
        {
            this.MpKey = mp.MpKey;

            this.PublicationSet = mp.PublicationSet;

            this.Index = index.ToString();
        }

        public DocumentKey(string documentKey)
        {
            var mp = documentKey.Split('|');

            this.MpKey = mp[0];

            this.PublicationSet = mp[1];

            this.Index = mp[2];
        }

        public override string ToString()
        {
            return $"{this.MpKey}|{this.PublicationSet}|{this.Index}";
        }
    }
}
