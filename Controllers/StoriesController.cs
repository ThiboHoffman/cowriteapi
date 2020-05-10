using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using coWriteAPI.DTOs;
using coWriteAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace coWriteAPI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IAuthorRepository _authorRepository;

        public StoriesController(IStoryRepository context, IAuthorRepository authorRepository)
        {
            _storyRepository = context;
            _authorRepository = authorRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Story> GetStories(string name = null, string author = null, string chapterName = null)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(author) && string.IsNullOrEmpty(chapterName))
                return _storyRepository.GetAll();
            return _storyRepository.GetBy(name, author, chapterName);
        }

        [HttpGet("chapters/{id}")]
        [AllowAnonymous]
        public IEnumerable<Chapter> GetChapters(int id)
        {
            return _storyRepository.GetBy(id).Chapters;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<Story> GetStory(int id)
        {
            Story story = _storyRepository.GetBy(id);
            if (story == null) return NotFound();
            return story;
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<Story> PostStory(StoryDTO story)
        {
            Author a = _authorRepository.GetBy(User.Identity.Name);
            Story storyToCreate = new Story()
            {
                Name = story.Name,
                CreatedBy = a.Nickname
            };
            foreach (var i in story.Chapters)
            {
                var chapter = new Chapter(1, i.Name, i.Text, a.Nickname);
                storyToCreate.AddChapter(chapter);
            }
            a.Stories.Add(storyToCreate);
            _storyRepository.Add(storyToCreate);
            _storyRepository.SaveChanges();

            return CreatedAtAction(nameof(GetStory), new { id = storyToCreate }, storyToCreate);
        }

        [HttpGet("{id}/chapters/{chapterId}")]
        public ActionResult<Chapter> GetChapter(int id, int chapterId)
        {
            if (!_storyRepository.TryGetStory(id, out var story))
            {
                return NotFound();
            }
            Chapter chapter = story.GetChapter(chapterId);
            if (chapter == null)
                return NotFound();
            return chapter;
        }

        [HttpPost("{storyId}/chapters")]
        public ActionResult<Chapter> PostChapter(int storyId, ChapterDTO chapter)
        {
            Author a = _authorRepository.GetBy(User.Identity.Name);
            if (!_storyRepository.TryGetStory(storyId, out var story))
            {
                return NotFound();
            }
            int number = _storyRepository.GetBy(storyId).Chapters.Count() + 1;
            var chapterToCreate = new Chapter(number, chapter.Name, chapter.Text, a.Nickname);
            story.AddChapter(chapterToCreate);
            _storyRepository.SaveChanges();
            return CreatedAtAction("GetChapter" , new { id = story.Id, chapterId = chapterToCreate.Id }, chapterToCreate);
        }

        [HttpGet("MyStories")]
        public IEnumerable<Story> GetMyStories()
        {
            Author Author = _authorRepository.GetBy(User.Identity.Name);
            return Author.Stories;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult PutStory(int id, Story story)
        {
            if (id != story.Id)
            {
                return BadRequest();
            } else
            {
                Story s = _storyRepository.GetBy(id);
                foreach (Chapter c in story.Chapters)
                {
                    if (!s.Chapters.Select(c => c.Name).Contains(c.Name) && c.Name != "")
                    {
                        c.Number = s.Chapters.Count() + 1;
                        c.Written = DateTime.Now;
                        c.Author = _authorRepository.GetBy(User.Identity.Name).Nickname;
                        s.AddChapter(c);
                    }
                }
            }
            _storyRepository.SaveChanges();
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStory(int id)
        {
            Story story = _storyRepository.GetBy(id);
            if (story == null)
            {
                return NotFound();
            }
            _storyRepository.Delete(story);
            _storyRepository.SaveChanges();
            return NoContent();
        }
    }
}
