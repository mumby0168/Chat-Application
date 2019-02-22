using System;
using System.Collections.Generic;
using System.Text;
using User.System.Core.Model;
using User.System.Core.Repository.Core.Interfaces;

namespace User.System.Core.Repository.Interfaces
{
    public interface ISocketUserRepo : IRepository<SocketUser>, IRepositoryAsync<SocketUser>
    {
    }
}
