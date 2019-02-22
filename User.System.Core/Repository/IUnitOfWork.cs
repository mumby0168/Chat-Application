using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User.System.Core.Repository.Interfaces;

namespace User.System.Core.Repository
{
    public interface IUnitOfWork
    {
        #region Repos

        ISocketUserRepo SocketUserRepo { get; }

        #endregion


        int Complete();

        Task<int> CompleteAsync();
    }
}
