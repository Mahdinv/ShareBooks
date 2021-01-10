using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareBooks.DataLayer.Entities.Books
{
    public class BookComment
    {
        [Key]
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsReadAdmin { get; set; }


        #region Relations

        public Book Book { get; set; }
        public Users.User User { get; set; }


        #endregion
    }
}
