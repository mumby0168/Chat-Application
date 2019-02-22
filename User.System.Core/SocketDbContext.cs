using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace User.System.Core
{
    public class SocketDbContext : DbContext
    {
        public SocketDbContext()
        {
            
        }

        public DbSet<Model.SocketUser> SocketUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=176.32.230.252;port=3306;pwd=Sockets123;uid=cl57-sockets;database=cl57-sockets;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
