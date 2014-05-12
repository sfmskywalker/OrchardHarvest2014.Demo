using System.ComponentModel.DataAnnotations;

namespace OrchardHarvest2014.WorkflowsJobsDemo.ViewModels {
    public class UnsubscribeViewModel {
        [Required]
        public string EmailAddress { get; set; }
    }
}