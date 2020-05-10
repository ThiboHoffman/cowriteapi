using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coWriteAPI.Model
{
    public class Author
    {
        public int AuthorId { get; set;}
        public string Nickname { get; set; }
        public string Email { get; set; }
        public ICollection<AuthorFavorite> Favorites { get; private set; }
        public ICollection<Story> Stories { get; set; }
        public IEnumerable<Story> FavoriteStories => Favorites.Select(s => s.Story);
        public Author()
        {
            Stories = new List<Story>();
            Favorites = new List<AuthorFavorite>();
        }

        public void Favorite(Story story)
        {
            if (IsFavorited(story))
            {
                RemoveFavoriteStory(story);
            } else
            {
                AddFavoriteStory(story);
            }
        }
        public Boolean IsFavorited(Story story)
        {
            System.Diagnostics.Debug.WriteLine(FavoriteStories.Contains(story));
            return FavoriteStories.Contains(story);
        }

        public void AddFavoriteStory(Story story)
        {
            System.Diagnostics.Debug.WriteLine("added");
            Favorites.Add(new AuthorFavorite() { StoryId = story.Id, UserId = AuthorId, Story = story, User = this });
        }

        public void RemoveFavoriteStory(Story story)
        {
            System.Diagnostics.Debug.WriteLine("removed");
            AuthorFavorite af = Favorites.SingleOrDefault(af => af.StoryId == story.Id);
            Favorites.Remove(af);
        }
    }
}
