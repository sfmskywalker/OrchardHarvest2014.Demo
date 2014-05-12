using System;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Models {
    public class Subscription {
        public virtual int Id { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime CreatedUtc { get; set; }
        public virtual DateTime ActivatedUtc { get; set; }
        public virtual DateTime? DeactivatedUtc { get; set; }
    }
}