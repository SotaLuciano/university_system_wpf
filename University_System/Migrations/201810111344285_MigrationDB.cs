namespace University_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InstituteId = c.Int(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutes", t => t.InstituteId)
                .Index(t => t.InstituteId);
            
            CreateTable(
                "dbo.Institutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DepartmentId = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SpecializationId = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specializations", t => t.SpecializationId)
                .Index(t => t.SpecializationId);
            
            AddColumn("dbo.Students", "GroupId", c => c.Int());
            CreateIndex("dbo.Students", "GroupId");
            AddForeignKey("dbo.Students", "GroupId", "dbo.Groups", "Id");
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
            DropColumn("dbo.Students", "GroupId");
            DropTable("dbo.Groups");
            DropTable("dbo.Specializations");
            DropTable("dbo.Institutes");
            DropTable("dbo.Departments");
        }
    }
}
