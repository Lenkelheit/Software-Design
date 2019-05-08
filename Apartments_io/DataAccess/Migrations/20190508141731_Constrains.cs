using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Constrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_Users_RenterId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Apartments_ApartmentId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_RenterId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_ResidentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Apartments_ApartmentId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_ResidentId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_Users_RenterId",
                table: "Apartments",
                column: "RenterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Apartments_ApartmentId",
                table: "Bills",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_RenterId",
                table: "Bills",
                column: "RenterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_ResidentId",
                table: "Notifications",
                column: "ResidentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Apartments_ApartmentId",
                table: "Requests",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_ResidentId",
                table: "Requests",
                column: "ResidentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_Users_RenterId",
                table: "Apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Apartments_ApartmentId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_RenterId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_ResidentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Apartments_ApartmentId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_ResidentId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_Users_RenterId",
                table: "Apartments",
                column: "RenterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Apartments_ApartmentId",
                table: "Bills",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_RenterId",
                table: "Bills",
                column: "RenterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_ResidentId",
                table: "Notifications",
                column: "ResidentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Apartments_ApartmentId",
                table: "Requests",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_ResidentId",
                table: "Requests",
                column: "ResidentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
