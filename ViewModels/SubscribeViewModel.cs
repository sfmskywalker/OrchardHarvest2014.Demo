using System.ComponentModel.DataAnnotations;

namespace OrchardHarvest2014.WorkflowsJobsDemo.ViewModels {
    public class SubscribeViewModel {
        [Required]
        public string EmailAddress { get; set; }
    }
}