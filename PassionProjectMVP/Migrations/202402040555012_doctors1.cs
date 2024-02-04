namespace PassionProjectMVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctors1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        DoctorFirstName = c.String(),
                        DoctorLastName = c.String(),
                        DoctorEmail = c.String(),
                        DoctorPhone = c.Int(nullable: false),
                        DoctorSpecialization = c.String(),
                    })
                .PrimaryKey(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Doctors");
        }
    }
}
