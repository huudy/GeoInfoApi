namespace GeoIpApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeoInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip = c.String(),
                        Type = c.String(),
                        Continent_code = c.String(),
                        Continent_name = c.String(),
                        Country_name = c.String(),
                        Region_code = c.String(),
                        Region_name = c.String(),
                        City = c.String(),
                        Zip = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Geoname_Id = c.Int(nullable: false),
                        Capital = c.String(),
                        Country_flag = c.String(),
                        Country_flag_emoji = c.String(),
                        Country_flag_emoji_unicode = c.String(),
                        Calling_code = c.String(),
                        Is_eu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Native = c.String(),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeoInfoes", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Languages", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Languages", new[] { "Location_Id" });
            DropIndex("dbo.GeoInfoes", new[] { "Location_Id" });
            DropTable("dbo.Languages");
            DropTable("dbo.Locations");
            DropTable("dbo.GeoInfoes");
        }
    }
}
