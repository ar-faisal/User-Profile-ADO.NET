using ADO.NET.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDbContext _context;
        public IUserRepository User { get; private set; }
        public UnitOfWork(IDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }

    }
}
