namespace BookRec.Infrastructure.EntityFramework.Models
{
    using System;

    public class UserBook : AggregateRoot
    {
        public string Username { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public int Rating { get; set; }
    }
}
