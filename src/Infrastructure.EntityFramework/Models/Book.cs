namespace BookRec.Infrastructure.EntityFramework.Models
{
    using System;
    using System.Collections.Generic;

    public class Book : AggregateRoot
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Authors { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishedDate { get; set; }

        public int PageCount { get; set; }

        public string Categories { get; set; }

        public string MaturityRating { get; set; }

        public string ImageLink { get; set; }

        public string ContainsImageBubbles { get; set; }

        public string LanguageCode { get; set; }

        public string PrintType { get; set; }

        public string PreviewLink { get; set; }

        public string Country { get; set; }

        public bool PublicDomain { get; set; }

        public ICollection<UserBook> BookUsers { get; set; }
    }
}
