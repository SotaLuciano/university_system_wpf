namespace University_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migratio_20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Departments", "InstituteId", "dbo.Institutes");
            DropForeignKey("dbo.Specializations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Groups", "SpecializationId", "dbo.Specializations");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropIndex("dbo.Departments", new[] { "InstituteId" });
            DropIndex("dbo.Specializations", new[] { "DepartmentId" });
            DropIndex("dbo.Groups", new[] { "SpecializationId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            AlterColumn("dbo.Departments", "InstituteId", c => c.Int(nullable: false));
            AlterColumn("dbo.Specializations", "DepartmentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "SpecializationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Students", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Departments", "InstituteId");
            CreateIndex("dbo.Specializations", "DepartmentId");
            CreateIndex("dbo.Groups", "SpecializationId");
            CreateIndex("dbo.Students", "GroupId");
            AddForeignKey("dbo.Departments", "InstituteId", "dbo.Institutes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Specializations", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "SpecializationId", "dbo.Specializations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Students", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "SpecializationId", "dbo.Specializations");
            DropForeignKey("dbo.Specializations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "InstituteId", "dbo.Institutes");
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "SpecializationId" });
            DropIndex("dbo.Specializations", new[] { "DepartmentId" });
            DropIndex("dbo.Departments", new[] { "InstituteId" });
            AlterColumn("dbo.Students", "GroupId", c => c.Int());
            AlterColumn("dbo.Groups", "SpecializationId", c => c.Int());
            AlterColumn("dbo.Specializations", "DepartmentId", c => c.Int());
            AlterColumn("dbo.Departments", "InstituteId", c => c.Int());
            CreateIndex("dbo.Students", "GroupId");
            CreateIndex("dbo.Groups", "SpecializationId");
            CreateIndex("dbo.Specializations", "DepartmentId");
            CreateIndex("dbo.Departments", "InstituteId");
            AddForeignKey("dbo.Students", "GroupId", "dbo.Groups", "Id");
            AddForeignKey("dbo.Groups", "SpecializationId", "dbo.Specializations", "Id");
            AddForeignKey("dbo.Specializations", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Departments", "InstituteId", "dbo.Institutes", "Id");
        }
    }
}
