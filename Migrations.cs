using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace OrchardHarvest2014.WorkflowsJobsDemo {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterPartDefinition("UserProfilePart", part => part
                .WithDescription("Adds some additional fields to the User type."));

            ContentDefinitionManager.AlterTypeDefinition("User", type => type
                .WithPart("UserProfilePart"));

            return 1;
        }
    }
}