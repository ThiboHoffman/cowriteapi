using coWriteApi.Data;
using coWriteAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Author> _users;

        public AuthorRepository(ApplicationContext dbContext)
        {
            _context = dbContext;
            _users = dbContext.Authors;
        }

        public Author GetBy(string email)
        {
            return _users.Include(c => c.Favorites).ThenInclude(f => f.Story).ThenInclude(r => r.Chapters).Include(s => s.Stories).ThenInclude(r => r.Chapters).SingleOrDefault(c => c.Email == email);
        }

        public void Add(Author user)
        {
            _users.Add(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
