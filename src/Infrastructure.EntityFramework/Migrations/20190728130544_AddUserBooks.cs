namespace BookRec.Infrastructure.EntityFramework.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBooks",
                schema: "BookRec",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    BookId = table.Column<Guid>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBooks_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "BookRec",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_BookId",
                schema: "BookRec",
                table: "UserBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_Username",
                schema: "BookRec",
                table: "UserBooks",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBooks",
                schema: "BookRec");
        }
    }
}
