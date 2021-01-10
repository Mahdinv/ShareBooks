using Microsoft.EntityFrameworkCore;
using ShareBooks.DataLayer.Entities.Books;
using ShareBooks.DataLayer.Entities.Permissions;
using ShareBooks.DataLayer.Entities.Publishers;
using ShareBooks.DataLayer.Entities.Site;
using ShareBooks.DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.DataLayer.Context
{
    public class ShareBooksContext : DbContext
    {
        public ShareBooksContext(DbContextOptions<ShareBooksContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGroup> BookGroups { get; set; }
        public DbSet<BookLevel> BookLevels { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
