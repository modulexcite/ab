using FluentMigrator;

namespace ab.Tests.Schema
{
    [Migration(2)]
    public class ABExperiment : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ABExperiment")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Name").AsString(250).NotNullable().Indexed()
                .WithColumn("Outcome").AsInt32().Nullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("CompletedAt").AsDateTime().Nullable()
                ;
        }
    }
}
