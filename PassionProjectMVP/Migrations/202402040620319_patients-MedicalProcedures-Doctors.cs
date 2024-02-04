namespace PassionProjectMVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientsMedicalProceduresDoctors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalProcedures", "PatientID", c => c.Int(nullable: false));
            AddColumn("dbo.MedicalProcedures", "DoctorID", c => c.Int(nullable: false));
            CreateIndex("dbo.MedicalProcedures", "PatientID");
            CreateIndex("dbo.MedicalProcedures", "DoctorID");
            AddForeignKey("dbo.MedicalProcedures", "DoctorID", "dbo.Doctors", "DoctorId", cascadeDelete: true);
            AddForeignKey("dbo.MedicalProcedures", "PatientID", "dbo.Patients", "PatientID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalProcedures", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.MedicalProcedures", "DoctorID", "dbo.Doctors");
            DropIndex("dbo.MedicalProcedures", new[] { "DoctorID" });
            DropIndex("dbo.MedicalProcedures", new[] { "PatientID" });
            DropColumn("dbo.MedicalProcedures", "DoctorID");
            DropColumn("dbo.MedicalProcedures", "PatientID");
        }
    }
}
