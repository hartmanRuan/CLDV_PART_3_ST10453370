using System.ComponentModel.DataAnnotations;

namespace _10453370_POE_WebApp.Models
{
    public class EventType
    {
        [Key]
        [Required(ErrorMessage = "Event type id Is Required")]
        public int EventType_ID { get; set; }
        
        [Required(ErrorMessage = "Event type description Is Required")]
        public string? EventType_Description { get; set; }
        

    }
}
