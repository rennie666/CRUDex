using CRUDex.Data;
using CRUDex.Models.Dtos.Author;
using CRUDex.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CRUDex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            var items = _context.Authors.Select(
                item => new GetAuthorDto
                {
                    Id = item.Id,
                    Name = item.Name
                }).ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(int id)
        {
            var item = _context.Authors.Select(
                item => new GetAuthorDto
                {
                    Id = item.Id,
                    Name = item.Name
                }).FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return BadRequest("Author with the given ID does not exist.");
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateAuthor(CreateEditAuthorDto authorDto)
        {

            var item = new Author
            {
                Name = authorDto.Name
            };

            _context.Authors.Add(item);
            _context.SaveChanges();

            //newItem shte returnne na usera dto-to a ne samoto entity za da nqma misuse na danni
            var newItem = new CreateEditAuthorDto
            {
                Name = item.Name
            };

            return Ok(newItem);
        }

        [HttpPut("{id}")]
        public IActionResult EditAuthor(int id, CreateEditAuthorDto authorDto)
        {
            var item = _context.Authors.FirstOrDefault(i => i.Id == id);
            if (item is null)
            {
                return BadRequest("Author with the given ID does not exist.");
            }

            

            //Proverqvame dali e vuvedena stoinost ? ako ne e ostava starata : ako e vzimame novata
            item.Name = authorDto.Name.IsNullOrEmpty() ? item.Name : authorDto.Name;
            
            _context.SaveChanges();

            //newItem shte returnne na usera dto-to a ne samoto entity za da nqma misuse na danni
            var updatedItem = new CreateEditAuthorDto
            {
                Name = item.Name
            };

            return Ok(updatedItem);
        }

        [HttpDelete]
        public IActionResult DeleteAuthor(int id)
        {
            var item = _context.Authors.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return BadRequest("Author with the given ID does not exist.");
            }
            if (_context.Books.Any(b => b.AuthorId == id))
            {
                return BadRequest("Cannot delete author because they are referenced by books.");
            }

            _context.Authors.Remove(item);
            _context.SaveChanges();

            return Ok("Author was successfully deleted");
        }
    }
}
