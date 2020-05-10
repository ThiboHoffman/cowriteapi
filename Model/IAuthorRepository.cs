using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public interface IAuthorRepository
    {
        Author GetBy(string email);
        void Add(Author user);
        void SaveChanges();
    }
}
