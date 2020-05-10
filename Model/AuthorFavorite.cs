using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public class AuthorFavorite
    {
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public Author User { get; set; }
        public Story Story { get; set; }

        public AuthorFavorite() { }
    }
}
