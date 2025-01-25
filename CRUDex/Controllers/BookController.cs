using CRUDex.Data;
using CRUDex.Models.Dtos.Book;
using CRUDex.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUDex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var items = _context.Books.Include(i => i.BookAuthor).Select(
                item => new GetBookDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    AuthorId = item.AuthorId,
                    AuthorName = item.BookAuthor.Name
                }).ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var item = _context.Books.Include(i => i.BookAuthor).Select(
                item => new GetBookDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    AuthorId = item.AuthorId,
                    AuthorName = item.BookAuthor.Name
                }).FirstOrDefault(x => x.Id == id);

            if(item == null)
            {
                return BadRequest("There is no such Book");
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBook(CreateEditBookDto bookDto)
        {
            

            if(_context.Books.Any(a => a.AuthorId != bookDto.AuthorId))
            {
                return BadRequest("There is no such author");
            }

            var item = new Book
            {
                    
                    Title = bookDto.Title,
                    Description = bookDto.Description,
                    AuthorId = bookDto.AuthorId
            };

            _context.Books.Add(item);
            _context.SaveChanges();

            //newItem shte returnne na usera dto-to a ne samoto entity za da nqma misuse na danni
            var newItem = new CreateEditBookDto
            {
                
                Title = item.Title,
                Description = item.Description,
                AuthorId = item.AuthorId
            };

            return Ok(newItem);
        }

        [HttpPut("{id}")]
        public IActionResult EditBook(int id, CreateEditBookDto bookDto)
        {
            var item = _context.Books.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return BadRequest("There is no such book");
            }

            if (bookDto.AuthorId != 0 && !_context.Authors.Any(a => a.Id == bookDto.AuthorId))
            {
                return BadRequest("Author with the given ID does not exist.");
            }
            //ne raboti mnogo
            //Proverqvame dali e vuvedena stoinost ? ako ne e ostava starata : ako e vzimame novata
            item.Title = bookDto.Title.IsNullOrEmpty() ? item.Title : bookDto.Title;
            item.Description = bookDto.Description.IsNullOrEmpty() ? item.Description : bookDto.Description; 
            item.AuthorId = bookDto.AuthorId == 0 ? item.AuthorId : bookDto.AuthorId;


            
            _context.SaveChanges();

            //newItem shte returnne na usera dto-to a ne samoto entity za da nqma misuse na danni
            var updatedItem = new CreateEditBookDto
            {
                Title = item.Title,
                Description = item.Description,
                AuthorId = item.AuthorId
            };

            return Ok(updatedItem);
        }

        [HttpDelete]
        public IActionResult DeleteBook(int id)
        {
            var item = _context.Books.Include(i => i.BookAuthor).FirstOrDefault(b => b.Id == id);
            if (item == null)
            {
                return BadRequest("There is no such book");
            }

            _context.Books.Remove(item);
            _context.SaveChanges();

            return Ok("Book was successfully deleted");
        }
    }
}
