using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class GitCommitter
    {
        private readonly ILogger<GitCommitter> logger;

        private readonly DirectoryStructureRawHtml dataSource;

        private readonly string localDataPath;

        private readonly string localRepoPath;

        private readonly string gitSignatureEmail;

        private readonly string gitSignatureName;

        public GitCommitter(ILogger<GitCommitter> logger, DirectoryStructureRawHtml dataSource, IOptions<IngestOptions> options)
        {
            this.logger = logger;

            this.dataSource = dataSource;

            this.gitSignatureEmail = options.Value.GitSignatureEmail;

            this.gitSignatureName = options.Value.GitSignatureName;

            this.localDataPath = options.Value.LocalDataPath;

            this.localRepoPath = options.Value.LocalRepoPath;
        }

        public async Task AddAndCommitAllFiles()
        {
            var lastPublicationSet = string.Empty;

            var lastPublicationDate = DateTime.MinValue;

            var commitMessage = string.Empty;

            foreach (var publicationSet in dataSource.PublicationSetsFrom(this.localDataPath).OrderBy(x => x))
            {
                foreach (var rawData in dataSource.MpDataFrom(this.localDataPath, publicationSet))
                {
                    if (lastPublicationSet == string.Empty)
                    {
                        lastPublicationSet = publicationSet;
                    }

                    if (lastPublicationDate == DateTime.MinValue)
                    {
                        lastPublicationDate = rawData.LikelyPublicationDate;
                    }

                    if (lastPublicationSet != publicationSet)
                    {
                        commitMessage = $"Add amendments to register made on {lastPublicationDate}";

                        this.CommitAllFiles(this.localRepoPath, commitMessage, lastPublicationDate);

                        this.logger.LogInformation(commitMessage);

                        lastPublicationSet = publicationSet;

                        lastPublicationDate = rawData.LikelyPublicationDate;
                    }

                    await File.WriteAllTextAsync($"{this.localRepoPath}\\{rawData.MpKey}.html", this.FormatHtml(rawData.Html));
                }

                commitMessage = $"Add amendments to register made on {lastPublicationDate}";
            }

            this.CommitAllFiles(this.localRepoPath, commitMessage, lastPublicationDate);
        }

        private void CommitAllFiles(string directory, string message, DateTime date)
        {
            using var repo = new Repository(directory);

            Commands.Stage(repo, "*");

            var signature = new Signature(this.gitSignatureName, this.gitSignatureEmail, new DateTimeOffset(date));

            repo.Commit(message, signature, signature);
        }

        private string FormatHtml(string htmlInput)
        {
            var parsedDocument = new HtmlParser().ParseDocument(htmlInput);

            var sw = new StringWriter();

            parsedDocument.ToHtml(sw, new PrettyMarkupFormatter());

            return sw.ToString();
        }
    }
}
