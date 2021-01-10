using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareBooks.DataLayer.Migrations
{
    public partial class AddSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(maxLength: 100, nullable: true),
                    SiteDescription = table.Column<string>(nullable: true),
                    SiteKeys = table.Column<string>(nullable: true),
                    SmsApi = table.Column<string>(nullable: true),
                    SmsSender = table.Column<string>(maxLength: 15, nullable: true),
                    MailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    MailPassword = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
