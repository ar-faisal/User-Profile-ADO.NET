using ADO.NET.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            _context = context;
        }
    }


}
