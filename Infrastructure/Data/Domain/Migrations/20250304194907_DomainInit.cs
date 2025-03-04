using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DomainInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleOfWorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobToPerformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledAtDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ScheduledAtPartOfDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Jobs_JobToPerformId",
                        column: x => x.JobToPerformId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Workers_ScheduleOfWorkerId",
                        column: x => x.ScheduleOfWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingScheduleChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NewPartOfDay = table.Column<int>(type: "int", nullable: false),
                    RequestDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingScheduleChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingScheduleChanges_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingScheduleChanges_RequestDateTime",
                table: "PendingScheduleChanges",
                column: "RequestDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PendingScheduleChanges_ScheduleId",
                table: "PendingScheduleChanges",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_JobToPerformId",
                table: "Schedules",
                column: "JobToPerformId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduledAtDate",
                table: "Schedules",
                column: "ScheduledAtDate");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduleOfWorkerId",
                table: "Schedules",
                column: "ScheduleOfWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduleOfWorkerId_ScheduledAtDate",
                table: "Schedules",
                columns: new[] { "ScheduleOfWorkerId", "ScheduledAtDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingScheduleChanges");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
