namespace ReportWebApp01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MST_CORPRATE",
                c => new
                    {
                        corp_code = c.String(nullable: false, maxLength: 10, unicode: false),
                        region_code = c.String(nullable: false, maxLength: 4, unicode: false),
                        corp_name = c.String(nullable: false),
                        corp_secure_code = c.String(nullable: false, maxLength: 8, unicode: false),
                    })
                .PrimaryKey(t => new { t.corp_code, t.region_code });
            
            
            
        }
        
        public override void Down()
        {
             DropTable("dbo.MST_CORPRATE");
        }
    }
}
