using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage = "Please enter a message")]
        public string Msg { get; set; }
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }
        [Required]
        public int SneakerId { get; set; }
        [Required]
        public Sneaker Sneaker { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public ApplicationUser Sender { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}