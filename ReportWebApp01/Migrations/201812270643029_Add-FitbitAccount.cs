namespace ReportWebApp01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFitbitAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MST_USERMANAGE",
                c => new
                    {
                        user_code = c.String(nullable: false, maxLength: 128),
                        corp_code = c.String(nullable: false, maxLength: 10, unicode: false),
                        nickname = c.String(nullable: false, maxLength: 10),
                        device_list = c.String(nullable: false),
                        gift_flg = c.Boolean(nullable: false),
                        last_sync_date = c.DateTime(nullable: false, storeType: "date"),
                        access_token = c.String(),
                        refresh_token = c.String(),
                        create_date = c.DateTime(nullable: false),
                        update_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.user_code)
                .Index(t => new { t.corp_code, t.nickname }, unique: true, name: "IX_CorpcodeNickName");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MST_USERMANAGE", "IX_CorpcodeNickName");
            DropTable("dbo.MST_USERMANAGE");
        }
    }
}
