using System;

namespace FilmsCatalog.Models
{
    public class Film
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Year { get; set; }

        public string Producer { get; set; }

        public byte[] PosterImage { get; set; }

        public string AuthorId { get; set; }
    }
}