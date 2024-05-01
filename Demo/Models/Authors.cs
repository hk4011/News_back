using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Demo.Models
{
    public class Authors
    {
        public int Id { get; set; }
      
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Author's name must be between 3 and 20 characters.")]
        public string Name { get; set; }
        [JsonIgnore] 
        public virtual List<News>? News { get; set; }
    }
}
