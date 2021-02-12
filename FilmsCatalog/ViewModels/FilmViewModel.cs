using System;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class FilmViewModel
    {
        [Required(ErrorMessage = "Отсутствует название")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Отсутствует описание")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Отсутствует указание года создания")]
        public DateTime Year { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Отсутствует указание режисера")]
        public string Producer { get; set; } = "";

        public string Publisher { get; set; } = "";

        public int Id { get; set; } = 0;

        public FilmViewModelType Type { get; set; } = FilmViewModelType.Add;

        public string PosterImageBase64WithMimeType { get; set; } = "";
    }
}