using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartXGPT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingevents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offerType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AllowedDays = table.Column<int>(type: "int", nullable: false),
                    SendExpireNotification = table.Column<bool>(type: "bit", nullable: false),
                    MessagesSendDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessagesSendHour = table.Column<int>(type: "int", nullable: false),
                    customerType = table.Column<int>(type: "int", nullable: false),
                    MessagePassage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    creatorLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
