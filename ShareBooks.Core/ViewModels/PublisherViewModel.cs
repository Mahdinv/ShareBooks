using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareBooks.Core.ViewModels
{
    public class CreatePublisherViewModel
    {
        [Display(Name = "نام ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string PublisherTitle { get; set; }

        public IFormFile PublisherImageName { get; set; }
    }

    public class EditPublisherViewModel
    {
        public int PublisherId { get; set; }

        [Display(Name = "نام ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string PublisherTitle { get; set; }

        public IFormFile PublisherImageName { get; set; }

        public string AvatarName { get; set; }
    }
}
