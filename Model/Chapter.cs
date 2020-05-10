using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public class Chapter
    {
        private string nickname;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime Written { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }

        public Chapter() {
            Written = DateTime.Now;
        }

        public Chapter(int number, string name, string text, string nickname)
        {
            Number = number;
            Name = name;
            Text = text;
            Author = nickname;
            Written = DateTime.Now;
        }
    }
}
