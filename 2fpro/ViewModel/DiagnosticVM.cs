using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewModel
{
    public class DiagnosticVM
    {
        public int ID { get; set; }

        public int UsersOnlineStatus { get; set; }

        public int AuthUsersOnlineStatus { get; set; }

        public string TheMostPopularLocation { get; set; }

        public bool IsActivate { get; set; }

        public string CustomStringField { get; set; }

        public string CustomStringField2 { get; set; }

        public int CustomIntField3 { get; set; }

        public DateTime CurrDateTime { get; set; }

        public bool CustomBoolField { get; set; }

    }
}
