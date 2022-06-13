using System.ComponentModel.DataAnnotations;

namespace WebSec_Task.Models
{
    public class ChatMessageEntity
    {
        [Key]
        public int Id { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
    }
}
