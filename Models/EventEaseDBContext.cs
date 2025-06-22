using Microsoft.EntityFrameworkCore;

namespace _10453370_POE_WebApp.Models
{
    public class EventEaseDBContext : DbContext
    {
        public EventEaseDBContext(DbContextOptions<EventEaseDBContext> options) : base(options)
        {

        }

        public DbSet<Event> Event { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<EventType> EventType { get; set; }

        
        public DbSet<BookingSummary> BookingSummary { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingSummary>().HasNoKey().ToView("BookingSummary");
        }
    }
}



/*REFERENCES
    GeeksForGeeks. 2025. ASP.NET Web Pages - Tutorial, 2025. [Online]. 
    Available at:
    https://www.w3schools.com/asp/webpages_intro.asp
    [Accessed 6 April 2025]
*/