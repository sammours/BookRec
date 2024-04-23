namespace BookRec.Infrastructure.EntityFramework.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BookRec");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "BookRec",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Subtitle = table.Column<string>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<DateTime>(nullable: false),
                    PageCount = table.Column<int>(nullable: false),
                    Categories = table.Column<string>(nullable: true),
                    MaturityRating = table.Column<string>(nullable: true),
                    ImageLink = table.Column<string>(nullable: true),
                    ContainsImageBubbles = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PrintType = table.Column<string>(nullable: true),
                    PreviewLink = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PublicDomain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books",
                schema: "BookRec");
        }
    }
}
