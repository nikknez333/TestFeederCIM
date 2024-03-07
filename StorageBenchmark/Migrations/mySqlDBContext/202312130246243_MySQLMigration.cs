namespace StorageBenchmark.Migrations.mySqlDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MySQLMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RDFGraphs",
                c => new
                    {
                        RDFGraphId = c.Int(nullable: false, identity: true),
                        GraphName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.RDFGraphId);
            
            CreateTable(
                "dbo.RDFTriples",
                c => new
                    {
                        RDFTripleId = c.Int(nullable: false, identity: true),
                        RDFGraphId = c.Int(nullable: false),
                        Subject = c.String(unicode: false),
                        Predicate = c.String(unicode: false),
                        Object = c.String(unicode: false),
                        GraphName = c.String(unicode: false),
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
