using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2fpro.ViewModel
{
    public class MakeOrderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resourses.Resource),
           ErrorMessageResourceName = "NameRequired")]
        [StringLength(300)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(300)]
        [Display(Name = "Company", ResourceType = typeof(Resourses.Resource))]
        public string Company { get; set; }
        [StringLength(1000)]
        [Display(Name = "OrderDetails", ResourceType = typeof(Resourses.Resource))]
        public string Description { get; set; }

        [StringLength(300)]
        [Display(Name = "Budget", ResourceType = typeof(Resourses.Resource))]
        public string Budget { get; set; }
        [Display(Name = "SiteType", ResourceType = typeof(Resourses.Resource))]
        public string SiteType { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resourses.Resource),
            ErrorMessageResourceName = "EmailRequired")]
        [StringLength(300)]
        [RegularExpression(".+@.+\\..+", ErrorMessageResourceType = typeof(Resourses.Resource),
                                     ErrorMessageResourceName = "EmailInvalid")]
        [Display(Name = "Email", ResourceType = typeof(Resourses.Resource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resourses.Resource),
            ErrorMessageResourceName = "PhoneRequired")]
        [StringLength(300)]
        [Display(Name = "Phone", ResourceType = typeof(Resourses.Resource))]
        public string Phone { get; set; }
    }
}