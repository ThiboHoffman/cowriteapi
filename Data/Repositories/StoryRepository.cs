using coWriteApi.Data;
using coWriteAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Data.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Story> _stories;

        public StoryRepository(ApplicationContext dbContext)
        {
            _context = dbContext;
            _stories = dbContext.Stories;
        }

        public IEnumerable<Story> GetAll()
        {
            return _stories.Include(r => r.Chapters).ToList();
        }

        public Story GetBy(int id)
        {
            return _stories.Include(r => r.Chapters).SingleOrDefault(r => r.Id == id);
        }

        public Story GetByName(string name)
        {
            return _stories.Include(r => r.Chapters).SingleOrDefault(r => r.Name == name);
        }
        public bool TryGetStory(int id, out Story story)
        {
            story = _stories.Include(t => t.Chapters).FirstOrDefault(t => t.Id == id);
            return story != null;
        }

        public void Add(Story story)
        {
            _stories.Add(story);
        }

        public void Update(Story story)
        {
            _stories.Update(story);
        }

        public void Delete(Story story)
        {
            _stories.Remove(story);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Story> GetBy(string name = null, string author = null, string chapterName = null)
        {
            var stories = _stories.Include(r => r.Chapters).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                stories = stories.Where(r => r.Name.IndexOf(name) >= 0);
            if (!string.IsNullOrEmpty(author))
                //stories = stories.Where(r => r.Authors.Contains(author));
            if (!string.IsNullOrEmpty(chapterName))
                stories = stories.Where(r => r.Chapters.Any(i => i.Name == chapterName));
            return stories.OrderBy(r => r.Name).ToList();
        }
    }
}
