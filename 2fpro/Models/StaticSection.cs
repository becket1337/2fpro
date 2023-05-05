using System;

using System.ComponentModel.DataAnnotations;



namespace _2fpro.Models
{
    public class StaticSection
    {
        [Key]
        public int ID { get; set; }


        public string Title { get; set; }

        public int Sequance { get; set; }


        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Preview { get; set; }

        public int Type { get; set; }
        public int SectionType { get; set; }


    }
}