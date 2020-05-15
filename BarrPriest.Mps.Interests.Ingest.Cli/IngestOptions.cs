using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class IngestOptions
    {
        public string GitSignatureEmail { get; set; }

        public string GitSignatureName { get; set; }

        public string LocalDataPath { get; set; }

        public string LocalRepoPath { get; set; }

        public string ParliamentWebsiteRootDirectory { get; set; }

        public string[] ParliamentWebsiteSessionPageNames { get; set; }
    }
}
