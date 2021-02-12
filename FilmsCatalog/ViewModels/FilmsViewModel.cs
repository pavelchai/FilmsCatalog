using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class FilmsViewModel
    {
        public IAsyncEnumerable<FilmViewModel> Films { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public PageSize PageSize { get; set; }
    }
}