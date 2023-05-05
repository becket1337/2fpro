using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace _2fpro.Models
{
    public class Menu
    {

        public int Id { get; set; }

        public int ParentId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните название")]
        public string Text { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните Url")]
        [RegularExpression(@"([a-zA-Z0-9]+(-|_)[-a-zA-Z0-9]+|\w+)", ErrorMessage = "допустимый формат ввода = o_kompanii или o-kompanii)")]
        public string Url { get; set; }


        public string Body { get; set; }

        public string BodyEng { get; set; }

        public bool IsPublish { get; set; }
        public string CustomField { get; set; }
        public string SeoDescription { get; set; }

        public DateTime LastModifiedDate { get; set; }


        public string SeoKeywords { get; set; }


        public int SortOrder { get; set; }

        public int MenuSection { get; set; }

    }
}