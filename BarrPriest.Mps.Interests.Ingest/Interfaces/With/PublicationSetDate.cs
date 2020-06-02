using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With
{
    public class PublicationSetDate
    {
        private readonly string publicationSetName;

        public PublicationSetDate(string publicationSetName)
        {
            this.publicationSetName = publicationSetName;
        }

        public DateTime LikelyPublicationDate
        {
            get
            {
                var dateParts = new int[] { int.Parse(this.publicationSetName.Substring(0, 2)), int.Parse(this.publicationSetName.Substring(2, 2)), int.Parse(this.publicationSetName.Substring(4, 2)) };

                return new DateTime(this.AssumedYearFromTwoCharacters(dateParts[0]), dateParts[1], dateParts[2]);
            }
        }

        private int AssumedYearFromTwoCharacters(int datePart)
        {
            return 2000 + datePart;
        }
    }
}
