using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.ViewModels
{
    public class ShowBookForAdminViewModel
    {
        public int BookId { get; set; }
        public string Publisher { get; set; }
        public string BookLevel { get; set; }
        public string BookFaTitle { get; set; }
        public string BookLatinTitle { get; set; }
        public string BookImageName { get; set; }
        public bool IsSpecial { get; set; }
        public DateTime CreateDate { get; set; }
        public string GroupId { get; set; }
        public string SubGroupId { get; set; }
        public string SecondSubGroupId { get; set; }

    }
}
