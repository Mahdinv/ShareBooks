using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShareBooks.DataLayer.Entities.Books
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? SubGroupId { get; set; }

        public int? SecondSubGroupId { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [Required]
        public int LevelId { get; set; }

        [Display(Name = "عنوان لاتین کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string BookLatinTitle { get; set; }

        [Display(Name = "عنوان فارسی کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string BookFaTitle { get; set; }

        [Display(Name = "حجم کتاب(مگابایت)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Volume { get; set; }

        [Display(Name = "زبان کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Language { get; set; }

        [Display(Name = "توضیحات کتاب ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BookDescription { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تعداد صفحات کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PageNumber { get; set; }

        [Display(Name = "نویسنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Writer { get; set; }

        [Display(Name = "نام مترجم")]
        public string TranslatorName { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [MaxLength(600)]
        public string Tags { get; set; }

        [Display(Name = "فایل کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string BookFileName { get; set; }

        [Display(Name = "تصویر کتاب")]
        [MaxLength(100)]
        public string BookImageName { get; set; }

        [Display(Name = "تعداد فایل‌ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CountFiles { get; set; }

        [Display(Name = "محصول ویژه؟")]
        public bool IsSpecial { get; set; }


        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }


        #region Relations

        [ForeignKey("GroupId")]
        public BookGroup BookGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public BookGroup SubGroup { get; set; }

        [ForeignKey("SecondSubGroupId")]
        public BookGroup SecondSubGroup { get; set; }

        public BookLevel BookLevel { get; set; }

        public Publishers.Publisher Publisher { get; set; }

        public List<BookComment> BookComments { get; set; }


        #endregion
    }
}
