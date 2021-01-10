using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareBooks.DataLayer.Entities.Books
{
    public class BookLevel
    {
        [Key]
        public int LevelId { get; set; }

        [Display(Name = "سطح کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LevelTitle { get; set; }

        #region Relations

        public List<Book> Books { get; set; }


        #endregion
    }
}
