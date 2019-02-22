using System;
using System.Collections.Generic;
using System.Text;
using User.System.Core.Model;
using User.System.Core.Repository.Core;
using User.System.Core.Repository.Interfaces;

namespace User.System.Core.Repository.Implementations
{
    public class SocketUserRepo : Repository<SocketUser>, ISocketUserRepo
    {
        public SocketUserRepo(SocketDbContext context) : base(context)
        {
        }


    }
}
