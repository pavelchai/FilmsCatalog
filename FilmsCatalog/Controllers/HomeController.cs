using FilmsCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly FilmRepository _repository;

        private readonly UserManager<User> _userManager;

        public HomeController(FilmRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("GetFilms");
        }

        [HttpGet]
        public async Task<IActionResult> GetFilms(int pageIndex = 0, PageSize pageSize = PageSize.PS10)
        {
            var user = await _userManager.GetUserAsync(this.User);

            var size = (int)pageSize;
            var offset = pageIndex * size;
            var count = await _repository.CountAsync();

            var model = new FilmsViewModel()
            {
                Films = _repository.GetFilms(offset, size).Select(f => GetFilmViewModel(f, user, false)),
                PageIndex = pageIndex,
                PageCount = (int)Math.Ceiling((double)count / size),
                PageSize = pageSize
            };

            return View("Films", model);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayFilm(int filmId)
        {
            var film = await _repository.ReadAsync(filmId);
            if (film == null)
            {
                return new BadRequestResult();
            }

            return View("Film", GetFilmViewModel(film, null, true));
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateFilm()
        {
            return View("Film", new FilmViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFilm(FilmViewModel model)
        {
            byte[] imageData = null;
            var files = this.HttpContext.Request.Form.Files;

            if (files.Count != 1)
            {
                if (string.IsNullOrEmpty(model.PosterImageBase64WithMimeType))
                {
                    ModelState.AddModelError("Image", $"Получено неверное количество файлов (принято {files.Count}, ожидается 1)");
                }
            }
            else
            {
                if (files[0].TryConvertFileToPNG(out imageData))
                {
                    model.PosterImageBase64WithMimeType = ImageUtils.ConvertToBase64WithMimeType(imageData);
                }
                else
                {
                    ModelState.AddModelError("Image", $"Получено некорректное изображение: {files[0].FileName}");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Type = FilmViewModelType.Add;
                return View("Film", model);
            }

            var user = await _userManager.GetUserAsync(this.User);
            var film = new Film
            {
                Name = model.Name,
                Description = model.Description,
                Year = model.Year,
                Producer = model.Producer,
                AuthorId = user.Id,
            };

            SetImageData(film, imageData, model);

            await _repository.CreateAsync(film);

            return RedirectToAction("GetFilms");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateFilm(int filmId = 0)
        {
            var film = await _repository.ReadAsync(filmId);
            if (film == null)
            {
                return new BadRequestResult();
            }

            var user = await _userManager.GetUserAsync(this.User);
            return View("Film", GetFilmViewModel(film, user, true));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFilm(FilmViewModel model)
        {
            byte[] imageData = null;
            var files = this.HttpContext.Request.Form.Files;

            switch (files.Count)
            {
                case 0:
                    if (string.IsNullOrEmpty(model.PosterImageBase64WithMimeType))
                    {
                        ModelState.AddModelError("Image", "Получено некорректное изображение");
                    }
                    break;
                case 1:
                    if (!files[0].TryConvertFileToPNG(out imageData))
                    {
                        ModelState.AddModelError("Image", $"Получено некорректное изображение: {files[0].FileName}");
                    }
                    else
                    {
                        model.PosterImageBase64WithMimeType = ImageUtils.ConvertToBase64WithMimeType(imageData);
                    }
                    break;
                default:
                    if (string.IsNullOrEmpty(model.PosterImageBase64WithMimeType))
                    {
                        ModelState.AddModelError("Image", $"Получено неверное количество файлов (принято {files.Count}, ожидается 0 или 1)");
                    }
                    break;
            }

            var user = await _userManager.GetUserAsync(this.User);
            var film = await _repository.ReadAsync(model.Id);

            if (user.Id != film.AuthorId)
            {
                ModelState.AddModelError("User", $"Пользователь {user.GetUserName()} не является создателем информации о фильме");
            }

            if (!ModelState.IsValid)
            {
                model.Type = FilmViewModelType.Update;
                return View("Film", model);
            }

            film.Name = model.Name;
            film.Description = model.Description;
            film.Year = model.Year;
            film.Producer = model.Producer;

            SetImageData(film, imageData, model);

            await _repository.UpdateAsync(film);

            return RedirectToAction("GetFilms");
        }

        private FilmViewModel GetFilmViewModel(Film film, User user, bool addImage)
        {
            User filmCreator = _userManager.Users.First(u => u.Id == film.AuthorId);

            FilmViewModelType type = user != null && user.Id == filmCreator.Id ? 
                FilmViewModelType.Update : FilmViewModelType.Read;

            var model = new FilmViewModel
            {
                Id = film.Id,
                Name = film.Name,
                Description = film.Description,
                Year = film.Year,
                Producer = film.Producer,
                Publisher = filmCreator.GetUserName(),
                Type = type 
            };

            if (addImage)
            {
                model.PosterImageBase64WithMimeType = ImageUtils.ConvertToBase64WithMimeType(film.PosterImage);
            }

            return model;
        }

        private static void SetImageData(Film film, byte[] imageData, FilmViewModel model)
        {
            if (imageData != null)
            {
                film.PosterImage = imageData;
            }
            else
            {
                film.PosterImage = ImageUtils.ConvertFromBase64WithMimeType(model.PosterImageBase64WithMimeType);
            }
        }
    }
}