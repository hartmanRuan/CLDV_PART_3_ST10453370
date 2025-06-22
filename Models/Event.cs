using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace _10453370_POE_WebApp.Models
{
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }
        [Required(ErrorMessage = "Event Name Is Required")]
        public string? Event_Name { get; set; }
        [Required(ErrorMessage = "Event Date Is Required")]
        public DateOnly Event_Date { get; set; }
        [Required(ErrorMessage = "Event Description Is Required")]
        public string? Description { get; set; }
        
        public int Venue_ID { get; set; }
        [ForeignKey("Venue_ID")]
        public Venue? Venue { get; set; }

        public int? EventType_ID { get; set; }
        [ForeignKey("EventType_ID")]
        public EventType? EventType { get; set; }

        //public List<Booking> Booking { get; set; } = new();

        //public List<Venue> Venue { get; set; } = new();

        

    }
}
