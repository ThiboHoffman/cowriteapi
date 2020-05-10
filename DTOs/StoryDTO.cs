using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.DTOs
{
    public class StoryDTO
    {
        [Required]
        public string Name { get; set; }
        public ICollection<ChapterDTO> Chapters { get; set; }

    }
}
