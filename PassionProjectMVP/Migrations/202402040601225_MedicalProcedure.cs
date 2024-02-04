namespace PassionProjectMVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MedicalProcedure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalProcedures",
                c => new
                    {
                        MedicalProcedureID = c.Int(nullable: false, identity: true),
                        MedicalProcedureName = c.String(),
                        MedicalCenter = c.String(),
                        MedicalProcedureDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MedicalProcedureID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MedicalProcedures");
        }
    }
}
