using System;
using Orchard.Data.Migration;

namespace OrchardHarvest2014.WorkflowsJobsDemo {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("Subscription", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<string>("EmailAddress", c => c.WithLength(128))
                .Column<bool>("IsActive")
                .Column<DateTime>("CreatedUtc")
                .Column<DateTime>("ActivatedUtc")
                .Column<DateTime>("DeactivatedUtc"));
            return 1;
        }
    }
}