using coWriteAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.DTOs
{
    public class AuthorDTO
    {
        public string Nickname { get; set; }

        public string Email { get; set; }

        public IEnumerable<Story> Stories { get; set; }

        public AuthorDTO() { }

        public AuthorDTO(Author user) : this()
        {
            Nickname = user.Nickname;
            Email = user.Email;
            Stories = user.Stories;
        }
    }
}
