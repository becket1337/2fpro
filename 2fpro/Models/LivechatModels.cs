using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{

    public class LiveUser
    {
        public LiveUser()
        {
            LiveMessages = new List<Message>();
        }
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Заполните имя")]
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOnline { get; set; }
        [Required(ErrorMessage = "Пустое сообщение")]
        public string FeedMessage { get; set; }
        [Required(ErrorMessage = "Заполните email")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string Email { get; set; }
        public string ConnId { get; set; }
        public string GroupId { get; set; }

        public virtual ICollection<Message> LiveMessages { get; set; }

    }

    public class Message
    {
        [Key]
        public int MessID { get; set; }
        public string TextMess { get; set; }

        public DateTime DateAdded { get; set; }
        public bool Visited { get; set; }


        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual LiveUser LiveUser { get; set; }

    }
    //public class LiveRoom {
    //    [Key]
    //    public string RoomName { get; set; }
    //    public virtual ICollection<LiveUser> LiveUsers { get; set; }
    //}
}
