using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class SignUpActivity : Event {
        
        public SignUpActivity() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override bool CanStartWorkflow {
            get { return true; }
        }

        public override string Name {
            get { return "SignUp"; }
        }

        public override LocalizedString Category {
            get { return T("Account"); }
        }

        public override LocalizedString Description {
            get { return T("Executes when a user signs up."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Proceed");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var userName = workflowContext.Tokens["UserName"] as string;
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;
            var password = workflowContext.Tokens["Password"] as string;

            workflowContext.SetState("UserName", userName);
            workflowContext.SetState("EmailAddress", emailAddress);
            workflowContext.SetState("Password", password);

            yield return T("Proceed");
        }
    }
}