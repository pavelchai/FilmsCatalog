using Microsoft.EntityFrameworkCore;
using FilmsCatalog.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public sealed class FilmRepository
    {
        private ApplicationDbContext context;

        public FilmRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Film> ReadOrDefaultAsync(int id)
        {
            return await this.context.Films.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task CreateAsync(Film film)
        {
            await this.context.Films.AddAsync(film);
            this.context.Entry(film).State = EntityState.Added;
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Film film)
        {
            this.context.Films.Attach(film);
            this.context.Entry(film).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await (this.context.Films as IQueryable<Film>).CountAsync();
        }

        public IAsyncEnumerable<Film> GetFilms(int offset = 0, int count = int.MaxValue)
        {
            return this.context.Films.AsNoTracking().OrderBy(f => f.Id).Skip(offset).Take(count).AsAsyncEnumerable();
        }
    }
}