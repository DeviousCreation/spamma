using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spamma.App.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "domain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subdomain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    subject = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    when_sent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subdomain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    domain_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdomain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    when_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    security_stamp = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "domain_access_policy",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    domain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_type = table.Column<int>(type: "integer", nullable: false),
                    when_assigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    when_revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain_access_policy", x => new { x.domain_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_domain_access_policy_domain_domain_id",
                        column: x => x.domain_id,
                        principalTable: "domain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chaos_monkey_address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    subdomain_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chaos_monkey_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_chaos_monkey_address_subdomain_subdomain_id",
                        column: x => x.subdomain_id,
                        principalTable: "subdomain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subdomain_access_policy",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subdomain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_type = table.Column<int>(type: "integer", nullable: false),
                    when_assigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    when_revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdomain_access_policy", x => new { x.subdomain_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_subdomain_access_policy_subdomain_subdomain_id",
                        column: x => x.subdomain_id,
                        principalTable: "subdomain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recorded_user_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_type = table.Column<int>(type: "integer", nullable: false),
                    when_happened = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recorded_user_event", x => x.id);
                    table.ForeignKey(
                        name: "fk_recorded_user_event_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chaos_monkey_address_email_address",
                table: "chaos_monkey_address",
                column: "email_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_chaos_monkey_address_subdomain_id",
                table: "chaos_monkey_address",
                column: "subdomain_id");

            migrationBuilder.CreateIndex(
                name: "ix_domain_name",
                table: "domain",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recorded_user_event_user_id",
                table: "recorded_user_event",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_subdomain_name",
                table: "subdomain",
                column: "name",
                unique: true);

            migrationBuilder.Sql(@"CREATE VIEW vw_domain AS
                SELECT *
                FROM domain");
            migrationBuilder.Sql(@"CREATE VIEW vw_email AS
                SELECT *
                FROM email");
            migrationBuilder.Sql(@"CREATE VIEW vw_subdomain AS
                SELECT *
                FROM subdomain");
            migrationBuilder.Sql(@"CREATE VIEW vw_user AS
                SELECT *
                FROM user");
            migrationBuilder.Sql(@"CREATE VIEW vw_domain_access_policy AS
                SELECT 
                        *
                    ,   CASE 
                            WHEN when_revoked is not null THEN 1 
                            ELSE 0 
                        END AS result
                FROM domain_access_policy");
            migrationBuilder.Sql(@"CREATE VIEW vw_chaos_monkey_address AS
                SELECT *
                FROM chaos_monkey_address");
            migrationBuilder.Sql(@"CREATE VIEW vw_subdomain_access_policy AS
                SELECT 
                        *
                    ,   CASE 
                            WHEN when_revoked is not null THEN 1 
                            ELSE 0 
                        END AS result
                FROM subdomain_access_policy");
            migrationBuilder.Sql(@"CREATE VIEW vw_recorded_user_event AS
                SELECT *
                FROM recorded_user_event");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chaos_monkey_address");

            migrationBuilder.DropTable(
                name: "domain_access_policy");

            migrationBuilder.DropTable(
                name: "email");

            migrationBuilder.DropTable(
                name: "recorded_user_event");

            migrationBuilder.DropTable(
                name: "subdomain_access_policy");

            migrationBuilder.DropTable(
                name: "domain");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "subdomain");

            migrationBuilder.Sql("DROP VIEW vw_chaos_monkey_address");

            migrationBuilder.Sql("DROP VIEW vw_domain_access_policy");

            migrationBuilder.Sql("DROP VIEW vw_email");

            migrationBuilder.Sql("DROP VIEW vw_recorded_user_event");

            migrationBuilder.Sql("DROP VIEW vw_subdomain_access_policy");

            migrationBuilder.Sql("DROP VIEW vw_domain");
            migrationBuilder.Sql("DROP VIEW vw_user");
            migrationBuilder.Sql("DROP VIEW vw_subdomain");
        }
    }
}