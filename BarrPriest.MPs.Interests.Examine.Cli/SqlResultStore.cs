using Azure.AI.TextAnalytics;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BarrPriest.MPs.Interests.Examine.Cli
{
    public class SqlResultStore
    {
        private readonly string connectionString;

        public SqlResultStore(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task SaveEntities(RawHtmlData mp, List<CategorizedEntity> entities)
        {
            var analysisResults = new List<KeyValuePair<DocumentKey, CategorizedEntity>>();

            foreach (var entity in entities)
            {
                analysisResults.Add(new KeyValuePair<DocumentKey, CategorizedEntity>(new DocumentKey(mp,0), entity));
            }

            await SaveEntities(analysisResults);
        }

        public async Task SaveEntities(List<KeyValuePair<DocumentKey, CategorizedEntity>> analysisResults)
        {
            Console.WriteLine($"Saving {analysisResults.Count} results");

            try
            {
                await using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync();

                var sql = @"INSERT INTO Entity(MpKey,FromEntry,Entity,Created,Modified)
                                VALUES(@MpKey,@FromEntry,@Entity,SYSDATETIMEOFFSET(),SYSDATETIMEOFFSET())";

                foreach (var pair in analysisResults)
                {
                    await using var command = new SqlCommand(sql, connection);

                    command.Parameters.AddWithValue("@MpKey", pair.Key.MpKey);

                    command.Parameters.AddWithValue("@FromEntry", pair.Key.PublicationSet);

                    command.Parameters.AddWithValue("@Entity", pair.Value.Text);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());

                Console.ReadLine();
            }
        }

        public async Task SaveLinkedEntities(RawHtmlData mp, List<LinkedEntity> entities)
        {
            try
            {
                await using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync();

                var sql = @"INSERT INTO [LinkedEntity] ([MpKey],[FromEntry],[Name],[Id],[Url],[Datasource],[Created],[Modified])
                            VALUES (@MpKey,@FromEntry,@Name, @Id, @Url, @Datasource, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())";

                foreach (var entity in entities)
                {
                    await using var command = new SqlCommand(sql, connection);

                    command.Parameters.AddWithValue("@MpKey", mp.MpKey);

                    command.Parameters.AddWithValue("@FromEntry", mp.PublicationSet);

                    command.Parameters.AddWithValue("@Name", entity.Name);

                    command.Parameters.AddWithValue("@Id", entity.DataSourceEntityId);

                    command.Parameters.AddWithValue("@Url", entity.Url.AbsoluteUri);

                    command.Parameters.AddWithValue("@Datasource", entity.DataSource);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());

                Console.ReadLine();
            }
        }
    }
}
