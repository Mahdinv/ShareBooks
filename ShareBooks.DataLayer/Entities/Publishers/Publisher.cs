using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareBooks.DataLayer.Entities.Publishers
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        [Display(Name = "نام ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string PublisherTitle { get; set; }

        [Display(Name = "تصویر ناشر")]
        [MaxLength(100)]
        public string PublisherImageName { get; set; }

        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }

        #region Relations

        public List<Books.Book> Books { get; set; }

        #endregion
    }
}
