using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Data.Abstract;
using VideoGames.Data.Concrete.Contexts;

namespace VideoGames.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VideoGamesDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(VideoGamesDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IGenericRepository<T>>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }



    }
}
