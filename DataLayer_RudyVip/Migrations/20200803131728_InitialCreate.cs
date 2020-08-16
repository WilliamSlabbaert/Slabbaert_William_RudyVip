using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer_RudyVip.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    FirstHourPrice = table.Column<double>(nullable: false),
                    NightlifePrice = table.Column<double>(nullable: false),
                    WeddingPrice = table.Column<double>(nullable: false),
                    WellnessPrice = table.Column<double>(nullable: false),
                    Available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Categorie = table.Column<string>(nullable: true),
                    PhoneNummer = table.Column<string>(nullable: true),
                    BtwNummer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReservationData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderingDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    OverHours = table.Column<int>(nullable: false),
                    OverHoursPriceTotal = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    ExclusiveBTWPrice = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    OrderAdress = table.Column<string>(nullable: true),
                    DeliverAdress = table.Column<string>(nullable: true),
                    Categorie = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReservationCarsData",
                columns: table => new
                {
                    reservationID = table.Column<int>(nullable: false),
                    carID = table.Column<int>(nullable: false),
                    customerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCarsData", x => new { x.carID, x.reservationID });
                    table.ForeignKey(
                        name: "FK_ReservationCarsData_CarData_carID",
                        column: x => x.carID,
                        principalTable: "CarData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationCarsData_CustomerData_customerID",
                        column: x => x.customerID,
                        principalTable: "CustomerData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationCarsData_ReservationData_reservationID",
                        column: x => x.reservationID,
                        principalTable: "ReservationData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCarsData_customerID",
                table: "ReservationCarsData",
                column: "customerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCarsData_reservationID",
                table: "ReservationCarsData",
                column: "reservationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationCarsData");

            migrationBuilder.DropTable(
                name: "CarData");

            migrationBuilder.DropTable(
                name: "CustomerData");

            migrationBuilder.DropTable(
                name: "ReservationData");
        }
    }
}
