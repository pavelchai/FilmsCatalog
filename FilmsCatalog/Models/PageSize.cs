using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public enum PageSize : int
    {
        [Display(Name = "3")]
        PS3 = 3,

        [Display(Name = "5")]
        PS5 = 5,

        [Display(Name = "10")]
        PS10 = 10,

        [Display(Name = "25")]
        PS25 = 25,

        [Display(Name = "50")]
        PS50 = 50,

        [Display(Name = "100")]
        PS100 = 100,

        [Display(Name = "200")]
        PS200 = 200,

        [Display(Name = "500")]
        PS500 = 500,

        [Display(Name = "1000")]
        PS1000 = 1000
    }
}