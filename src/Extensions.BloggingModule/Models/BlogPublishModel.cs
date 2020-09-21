using System.ComponentModel.DataAnnotations;

namespace SatelliteSite.BloggingModule.Models
{
    public class BlogPublishModel
    {
        public string Content { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public bool ShowOnHomePage { get; set; }
    }
}
