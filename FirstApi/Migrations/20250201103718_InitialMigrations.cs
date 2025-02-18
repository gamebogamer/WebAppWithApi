using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_users",
                columns: table => new
                {
                    c_userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    c_username = table.Column<string>(type: "text", nullable: false),
                    c_email = table.Column<string>(type: "text", nullable: false),
                    c_password = table.Column<string>(type: "text", nullable: false),
                    c_dateofbirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    c_gender = table.Column<string>(type: "text", nullable: false),
                    c_phonenumber = table.Column<string>(type: "text", nullable: false),
                    c_address = table.Column<string>(type: "text", nullable: false),
                    c_usertype = table.Column<string>(type: "text", nullable: false),
                    c_status = table.Column<string>(type: "text", nullable: false),
                    c_createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    c_updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    c_hobby = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_users", x => x.c_userid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_users");
        }
    }
}
