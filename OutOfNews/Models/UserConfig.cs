namespace OutOfNews.Models
{
    public class UserConfig
    {
        public int Id { get; set; }
        public bool UseNickname { get; set; } = false;
        public string Nickname { get; set; } = null;

        // TODO: public ICollection<Article> SavedArticles { get; set; } = new List<Article>
    }
}