using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShareBooks.DataLayer.Entities.Books
{
    public class BookGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string GroupTitle { get; set; }

        [Display(Name = "توضیحات گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Description { get; set; }

        [Display(Name = "تصویر گروه")]
        [MaxLength(100)]
        public string GroupImageName { get; set; }

        [Display(Name = "حذف شده ؟")]
        public bool IsDelete { get; set; }

        [Display(Name = "گروه اصلی")]
        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public List<BookGroup> BookGroups { get; set; }

        [InverseProperty("BookGroup")]
        public List<Book> Books { get; set; }

        [InverseProperty("SubGroup")]
        public List<Book> SubGroupBook { get; set; }

        [InverseProperty("SecondSubGroup")]
        public List<Book> SecondSubGroupBook { get; set; }
        #endregion
    }
}
