namespace CRUDex.Models.Dtos.Book
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; } //svurzva tablicite Book i Author
        public string AuthorName { get; set; } //Za da se vizualizira imeto na avtora v HttpGet
    }
}
