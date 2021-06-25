namespace Graduation_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEntitiesDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contests",
                c => new
                    {
                        contestId = c.Int(nullable: false, identity: true),
                        contestTitle = c.String(),
                        contestStartTime = c.DateTime(nullable: false),
                        contestDuration = c.Int(nullable: false),
                        creationTime = c.DateTime(nullable: false),
                        contestVisabilty = c.String(),
                    })
                .PrimaryKey(t => t.contestId);
            
            CreateTable(
                "dbo.UserContests",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ContestId = c.Int(nullable: false),
                        isFavourite = c.Boolean(nullable: false),
                        isOwner = c.Boolean(nullable: false),
                        isRegistered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ContestId })
                .ForeignKey("dbo.Contests", t => t.ContestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ContestId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        PhotoUrl = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Boolean(nullable: false),
                        DateOfJoin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        UserRole = c.String(),
                        MemberSince = c.DateTime(nullable: false),
                        isFavourite = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        creationTime = c.DateTime(nullable: false),
                        GroupDescription = c.String(),
                        Visable = c.Boolean(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Problems",
                c => new
                    {
                        ProblemId = c.Int(nullable: false, identity: true),
                        ProblemSource = c.String(),
                        problemSourceId = c.Int(nullable: false),
                        problemTitle = c.String(),
                        problemType = c.Int(nullable: false),
                        timelimit = c.Int(nullable: false),
                        memorylimit = c.Int(nullable: false),
                        inputType = c.String(),
                        outputType = c.String(),
                        problemText = c.String(),
                        rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProblemId);
            
            CreateTable(
                "dbo.ProblemTags",
                c => new
                    {
                        ProblemId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProblemId, t.TagId })
                .ForeignKey("dbo.Problems", t => t.ProblemId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ProblemId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        tagId = c.Int(nullable: false, identity: true),
                        tagName = c.String(),
                    })
                .PrimaryKey(t => t.tagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProblemTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.ProblemTags", "ProblemId", "dbo.Problems");
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserContests", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserContests", "ContestId", "dbo.Contests");
            DropIndex("dbo.ProblemTags", new[] { "TagId" });
            DropIndex("dbo.ProblemTags", new[] { "ProblemId" });
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropIndex("dbo.UserContests", new[] { "ContestId" });
            DropIndex("dbo.UserContests", new[] { "UserId" });
            DropTable("dbo.Tags");
            DropTable("dbo.ProblemTags");
            DropTable("dbo.Problems");
            DropTable("dbo.Groups");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Users");
            DropTable("dbo.UserContests");
            DropTable("dbo.Contests");
        }
    }
}
