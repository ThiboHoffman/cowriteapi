using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public class Story
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Favorites { get; set; }
        public string CreatedBy { get; set; }
        //public ICollection<string> Authors { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<Chapter> Chapters { get; set; }

        public Story()
        {
            //Authors = new Collection<string>();
            Chapters = new Collection<Chapter>();
            DateAdded = DateTime.Now;
            Favorites = 1;
        }

        public void AddChapter(Chapter chapter)
        {
            Chapters.Add(chapter);
        }

        public Chapter GetChapter(int id)
        {
            return this.Chapters.SingleOrDefault(c => c.Id == id);
        }
    }
}
