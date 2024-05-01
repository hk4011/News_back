using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using Demo.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    
    [Route("api/[controller]")]
    
    [ApiController]
   
    public class NewsController : ControllerBase
    {
        private readonly TaskContext _context;

        public NewsController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/News
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            var newsList = await _context.News.Include(s => s.Authors).ToListAsync();
            var newsDtoList = newsList.Select(news => new NewswithauthornameDTo
            {
                Id = news.Id,
                Title = news.Title,
                AuthorName = news.Authors.Name,
                Content = news.Content,
                ImageUrl = news.ImageUrl,
                PublicationDate = news.Pulication,
                CreationDate = news.Creation
            }).ToList();

            return Ok(newsDtoList);
        }
        [HttpGet("{id:int}", Name = "OneNew")]
        public IActionResult GetNew(int id)
        {
            News news = _context.News.Include(s => s.Authors).FirstOrDefault(e => e.Id == id);
            if (news == null)
            {
                return NotFound(); // Return 404 Not Found if news article is not found
            }

            NewswithauthornameDTo newswithauthornameDTo = new NewswithauthornameDTo();
            newswithauthornameDTo.AuthorName = news.Authors.Name;
            newswithauthornameDTo.Id = news.Id;
            newswithauthornameDTo.Title = news.Title;
            newswithauthornameDTo.Content = news.Content;
            newswithauthornameDTo.PublicationDate = news.Pulication;
            newswithauthornameDTo.CreationDate= news.Creation;
            return Ok(newswithauthornameDTo); // Return the DTO object instead of the News object
        }


        // GET: api/News/5


        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, News news)
        {
            var oldNews = await _context.News.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (oldNews == null) return BadRequest();

            if (news.PublicationDate < DateTime.Today || news.PublicationDate > DateTime.Today.AddDays(7))
            {
                return BadRequest("Publication date must be between today and a week from today.");
            }
            oldNews.CreationDate = news.CreationDate;
            oldNews.PublicationDate = news.PublicationDate;
            oldNews.AuthorName = news.AuthorName;
            oldNews.Title = news.Title;
            oldNews.Content = news.Content;
            oldNews.ImageUrl = news.ImageUrl;
            
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
            if (news.PublicationDate < DateTime.Today || news.PublicationDate > DateTime.Today.AddDays(7))
            {
                return BadRequest("Publication date must be between today and a week from today.");
            }
            news.CreationDate = DateTime.Now;
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.Id }, news);
        }
      

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
