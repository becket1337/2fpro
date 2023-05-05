using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Models
{
    public class Diagnostic
    {
        public int ID { get; set; }

        public int UsersOnlineStatus { get; set; }

        public int AuthUsersOnlineStatus { get; set; }
        // UserIdentifier
        public string ConnectionID { get; set; }

        public string Location { get; set; }

        public bool IsActivate { get; set; }
        [Display(Name = "UserIdentifier")]
        public string CustomStringField { get; set; }
        //проверка в онлайн ли админ 1, если нет то 0
        public int CustomIntField { get; set; }

        public DateTime CurrDateTime { get; set; }

        public bool CustomBoolField { get; set; }

        [Display(Name = "Ip address")]
        public string IpAddress { get; set; }

        [Display(Name = "Get Data Status")]
        public string Status { get; set; }

        public string Country { get; set; }
        [Display(Name = "Country Code - 2 буквы")]
        public string CountryCode { get; set; }
        [Display(Name = "Регион, короткий код")]
        public string RegionCode { get; set; }
        [Display(Name = "Регион или штат")]
        public string RegionName { get; set; }

        public string City { get; set; }
        [Display(Name = "Zip-код ")]
        public string Zip { get; set; }
        [Display(Name = "Широта")]
        public float Latitude { get; set; }
        [Display(Name = "Долгота")]
        public float Longitude { get; set; }
        [Display(Name = "Временная зона в регионе")]
        public string Timezone { get; set; }

        public string ISPName { get; set; }
        public string OrganizationName { get; set; }
        public string AsNumberAndName { get; set; }
        public bool IsMobileConn { get; set; }

    }
}

