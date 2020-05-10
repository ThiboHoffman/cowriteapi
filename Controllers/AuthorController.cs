using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IStoryRepository _storyRepository;

        public AuthorController(IAuthorRepository authorRepository, IStoryRepository storyRepository)
        {
            _authorRepository = authorRepository;
            _storyRepository = storyRepository;
        }

        /// <summary>
        /// Get the details of the authenticated customer
        /// </summary>
        /// <returns>the customer</returns>
        [HttpGet()]
        public ActionResult<AuthorDTO> GetAuthor()
        {
            Author author = _authorRepository.GetBy(User.Identity.Name);
            return new AuthorDTO(author);
        }

        [HttpGet("isFavorited/{id}")]
        public ActionResult<Boolean> IsFavorited(int id)
        {
            Author author = _authorRepository.GetBy(User.Identity.Name);
            return author.FavoriteStories.SingleOrDefault(f => f.Id == id) != null;
        }

        [HttpPost]
        public ActionResult<Story> FavoriteStory(StoryDTO story)
        {
            Author a = _authorRepository.GetBy(User.Identity.Name);
            Story StoryToFavorite = _storyRepository.GetByName(story.Name);
            a.Favorite(StoryToFavorite);
            _authorRepository.SaveChanges();

            return CreatedAtAction(nameof(GetStory), new { id = StoryToFavorite }, StoryToFavorite);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<Story> GetStory(int id)
        {
            Story story = _storyRepository.GetBy(id);
            if (story == null) return NotFound();
            return story;
        }

        [HttpGet("favoriteStories")]
        public IEnumerable<Story> GetFavoriteStories()
        {
            Author Author = _authorRepository.GetBy(User.Identity.Name);
            return Author.FavoriteStories;
        }
    }
}
