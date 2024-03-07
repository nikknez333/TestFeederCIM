namespace StorageBenchmark.Migrations.PostgreSQLDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostgreSQLMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RDFGraphs",
                c => new
                    {
                        RDFGraphId = c.Int(nullable: false, identity: true),
                        GraphName = c.String(),
                    })
                .PrimaryKey(t => t.RDFGraphId);
            
            CreateTable(
                "dbo.RDFTriples",
                c => new
                    {
                        RDFTripleId = c.Int(nullable: false, identity: true),
                        RDFGraphId = c.Int(nullable: false),
                        Subject = c.String(),
                        Predicate = c.String(),
                        Object = c.String(),
                        GraphName = c.String(),
                    })
                .PrimaryKey(t => t.RDFTripleId)
                .ForeignKey("dbo.RDFGraphs", t => t.RDFGraphId, cascadeDelete: true)
                .Index(t => t.RDFGraphId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RDFTriples", "RDFGraphId", "dbo.RDFGraphs");
            DropIndex("dbo.RDFTriples", new[] { "RDFGraphId" });
            DropTable("dbo.RDFTriples");
            DropTable("dbo.RDFGraphs");
        }
    }
}
