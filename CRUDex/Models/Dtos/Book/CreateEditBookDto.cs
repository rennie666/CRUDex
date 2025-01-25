namespace CRUDex.Models.Dtos.Book
{
    public class CreateEditBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; } //svurzva tablicite Book i Author
    }
}
