using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class SignInActivity : Task {
        private readonly IAuthenticationService _authenticationService;

        public SignInActivity(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "SignIn"; }
        }

        public override LocalizedString Category {
            get { return T("Account"); }
        }

        public override LocalizedString Description {
            get { return T("Sign in a user."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Proceed");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var user = workflowContext.GetState<IUser>("User");
            _authenticationService.SignIn(user, false);
            yield return T("Proceed");
        }
    }
}