using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.System.Core.Repository.Implementations;
using User.System.Core.Repository.Interfaces;

namespace User.System.Core.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SocketDbContext _socketDbContext;
        public UnitOfWork(SocketDbContext socketDbContext)
        {
            _socketDbContext = socketDbContext;
            SocketUserRepo = new SocketUserRepo(socketDbContext);
        }

        public ISocketUserRepo SocketUserRepo { get; private set; }

        public int Complete()
        {
            return _socketDbContext.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _socketDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _socketDbContext.Dispose();
        }
    }
}
