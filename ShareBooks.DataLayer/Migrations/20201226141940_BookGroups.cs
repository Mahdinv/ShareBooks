using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareBooks.DataLayer.Migrations
{
    public partial class BookGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupTitle = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    GroupImageName = table.Column<string>(maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_BookGroups_BookGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BookGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookLevels",
                columns: table => new
                {
                    LevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LevelTitle = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLevels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    SubGroupId = table.Column<int>(nullable: true),
                    SecondSubGroupId = table.Column<int>(nullable: true),
                    LevelId = table.Column<int>(nullable: false),
                    BookLatinTitle = table.Column<string>(maxLength: 400, nullable: false),
                    BookFaTitle = table.Column<string>(maxLength: 400, nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    Language = table.Column<string>(maxLength: 200, nullable: false),
                    BookDescription = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    PageNumber = table.Column<string>(nullable: false),
                    Writer = table.Column<string>(nullable: false),
                    TranslatorName = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(maxLength: 600, nullable: true),
                    BookFileName = table.Column<string>(maxLength: 500, nullable: false),
                    BookImageName = table.Column<string>(maxLength: 100, nullable: true),
                    CountFiles = table.Column<int>(nullable: false),
                    IsSpecial = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_BookGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "BookGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_BookLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "BookLevels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_BookGroups_SecondSubGroupId",
                        column: x => x.SecondSubGroupId,
                        principalTable: "BookGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_BookGroups_SubGroupId",
                        column: x => x.SubGroupId,
                        principalTable: "BookGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    BookId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    IsReadAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_BookComments_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_BookId",
                table: "BookComments",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_UserId",
                table: "BookComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGroups_ParentId",
                table: "BookGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_GroupId",
                table: "Books",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LevelId",
                table: "Books",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SecondSubGroupId",
                table: "Books",
                column: "SecondSubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SubGroupId",
                table: "Books",
                column: "SubGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookComments");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BookGroups");

            migrationBuilder.DropTable(
                name: "BookLevels");
        }
    }
}
