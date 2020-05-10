using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public interface IStoryRepository
    {
        Story GetBy(int id);
        Story GetByName(string name);
        bool TryGetStory(int id, out Story story);
        IEnumerable<Story> GetAll();
        IEnumerable<Story> GetBy(string name = null, string author = null, string chapterName = null);
        void Add(Story story);
        void Delete(Story story);
        void Update(Story story);
        void SaveChanges();
    }
}
