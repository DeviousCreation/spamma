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
                name: MigrationConstants.Tables.Domain,
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    when_disabled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.Email,
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subdomain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    subject = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    when_sent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.Subdomain,
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    domain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    when_disabled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdomain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.User,
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    when_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    security_stamp = table.Column<Guid>(type: "uuid", nullable: false),
                    when_disabled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.DomainAccessPolicy,
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_assigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    domain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_type = table.Column<int>(type: "integer", nullable: false),
                    when_revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain_access_policy", x => new { x.domain_id, x.user_id, x.when_assigned });
                    table.ForeignKey(
                        name: "fk_domain_access_policy_domain_domain_id",
                        column: x => x.domain_id,
                        principalTable: MigrationConstants.Tables.Domain,
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.ChaosMonkeyAddress,
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
                        principalTable: MigrationConstants.Tables.Subdomain,
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.SubdomainAccessPolicy,
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_assigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    subdomain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_type = table.Column<int>(type: "integer", nullable: false),
                    when_revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subdomain_access_policy", x => new { x.subdomain_id, x.user_id, x.when_assigned });
                    table.ForeignKey(
                        name: "fk_subdomain_access_policy_subdomain_subdomain_id",
                        column: x => x.subdomain_id,
                        principalTable: MigrationConstants.Tables.Subdomain,
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: MigrationConstants.Tables.RecordedUserEvent,
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
                        principalTable: MigrationConstants.Tables.User,
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chaos_monkey_address_email_address",
                table: MigrationConstants.Tables.ChaosMonkeyAddress,
                column: "email_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_chaos_monkey_address_subdomain_id",
                table: MigrationConstants.Tables.ChaosMonkeyAddress,
                column: "subdomain_id");

            migrationBuilder.CreateIndex(
                name: "ix_domain_name",
                table: MigrationConstants.Tables.Domain,
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recorded_user_event_user_id",
                table: MigrationConstants.Tables.RecordedUserEvent,
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_subdomain_name",
                table: MigrationConstants.Tables.Subdomain,
                column: "name",
                unique: true);

            migrationBuilder.Sql($@"create or replace view public.vw_domain AS
select 
        *
    ,   case 
            when when_disabled is not null then true else false 
        end AS is_disabled
from public.{MigrationConstants.Tables.Domain}");

            migrationBuilder.Sql($@"create or replace view public.vw_email AS
select *
from public.{MigrationConstants.Tables.Email}");

            migrationBuilder.Sql($@"create or replace view public.vw_subdomain AS
select
        *
    ,   case 
            when when_disabled is not null then true else false 
        end AS is_disabled
from public.{MigrationConstants.Tables.Subdomain}");

            migrationBuilder.Sql($@"create or replace view public.vw_user AS
select
        u.id
    ,   u.name
    ,   u.email_address
    ,   u.when_created
    ,   u.security_stamp
    ,   u.when_disabled
    ,   (select when_happened from public.{MigrationConstants.Tables.RecordedUserEvent} where user_id = u.id and action_type = 4 order by when_happened desc limit 1) as when_verified
    ,   (select when_happened from public.{MigrationConstants.Tables.RecordedUserEvent} where user_id = u.id and action_type = 1 order by when_happened desc limit 1) as last_logged_in
    ,   count(dap.domain_id) as domain_count
    ,   count(sap.subdomain_id) as subdomain_count
from public.{MigrationConstants.Tables.User} u
left join public.{MigrationConstants.Tables.DomainAccessPolicy} dap
    on 
            u.id = dap.user_id
       and dap.when_revoked is null
left join public.{MigrationConstants.Tables.SubdomainAccessPolicy} sap
    on 
            u.id = sap.user_id
        and sap.when_revoked is null
group by 
        u.id
    ,   u.name
    ,   u.email_address
    ,   u.when_created
    ,   u.security_stamp
    ,   u.when_disabled");
            migrationBuilder.Sql($@"create or replace view public.vw_domain_access_policy AS
select 
        *
    ,   case 
            when when_revoked is not null then 1 else 0 
        end AS is_revoked
from public.{MigrationConstants.Tables.DomainAccessPolicy}");
            migrationBuilder.Sql($@"create or replace view public.vw_chaos_monkey_address AS
select *
from public.{MigrationConstants.Tables.ChaosMonkeyAddress}");

            migrationBuilder.Sql($@"create or replace view public.vw_subdomain_access_policy AS
select 
        *
    ,   case 
            when when_revoked is not null then true else false 
        end AS is_revoked
from public.{MigrationConstants.Tables.SubdomainAccessPolicy}");

            migrationBuilder.Sql($@"create or replace view public.vw_recorded_user_event AS
select *
from public.{MigrationConstants.Tables.RecordedUserEvent}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.ChaosMonkeyAddress);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.DomainAccessPolicy);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.Email);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.RecordedUserEvent);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.SubdomainAccessPolicy);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.Domain);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.User);

            migrationBuilder.DropTable(
                name: MigrationConstants.Tables.Subdomain);

            migrationBuilder.Sql("drop view public.vw_domain");

            migrationBuilder.Sql("drop view public.vw_email");

            migrationBuilder.Sql("drop view public.vw_subdomain");

            migrationBuilder.Sql("drop view public.vw_user");
            migrationBuilder.Sql("drop view public.vw_domain_access_policy");
            migrationBuilder.Sql("drop view public.vw_chaos_monkey_address");

            migrationBuilder.Sql("drop view public.vw_subdomain_access_policy");

            migrationBuilder.Sql("drop view public.vw_recorded_user_event");
        }
    }
}
