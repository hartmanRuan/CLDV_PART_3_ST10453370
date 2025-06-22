using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _10453370_POE_WebApp.Models;
namespace _10453370_POE_WebApp.Controllers
{

    public class BookingSummaryController : Controller
    {
        private readonly EventEaseDBContext _context;

        public BookingSummaryController(EventEaseDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            
            var bookingQ = from b in _context.BookingSummary select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingQ = bookingQ.Where(b => b.Event_Name.ToLower().Contains(searchString) || b.Venue_Name.ToLower().Contains(searchString));
            }
            

            var summary = await bookingQ.ToListAsync();
            return View(summary);
            

            
        }

    }
}
