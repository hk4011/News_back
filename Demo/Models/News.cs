using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Demo.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public DateOnly Pulication { get; set; }
        public DateOnly Creation { get; set; }

        [ForeignKey("Authors")]
       
        public int? AuthorsId { get; set; }
        [JsonIgnore]
        public virtual Authors? Authors { get; set; }
       
    }
}

