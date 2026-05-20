using ChatApp.Core.DataAccess.EFEntity;
using ChatApp.Core.Entities;
using ChatApp.DataAccess.Abstract;
using ChatApp.DataAccess.Concrete.DbContexts;
using ChatApp.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Concrete
{
    public class UserRepository : EfRepositoryBase<User>, IUserRepository
    {

        //public async Task<bool> IsNameExistAsync(string name, CancellationToken cancellationToken)
        //{

        //    return await _context.Users.AnyAsync(x => x.Name == name, cancellationToken);
        //}

        //public async Task<User> GetByNameAsync(string name, CancellationToken cancellationToken)
        //{

        //    return await _context.Users.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        //}

        //public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        //{


        //    await _context.Users.AddAsync(user, cancellationToken);
        //    await _context.SaveChangesAsync(cancellationToken);
        //}

        //public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        //{
        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync(cancellationToken);

        //}
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Set<User>().AnyAsync(x => x.Name == name, cancellationToken);
        }
    }
}
