using System.ComponentModel.DataAnnotations;

namespace OrchardHarvest2014.WorkflowsJobsDemo.ViewModels {
    public class SignUpViewModel {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}