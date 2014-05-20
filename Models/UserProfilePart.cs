using Orchard.ContentManagement;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Models {
    public class UserProfilePart : ContentPart {
        public string FirstName {
            get { return this.Retrieve(x => x.FirstName); }
            set { this.Store(x => x.FirstName, value); }
        }

        public string LastName {
            get { return this.Retrieve(x => x.LastName); }
            set { this.Store(x => x.LastName, value); }
        }
    }
}