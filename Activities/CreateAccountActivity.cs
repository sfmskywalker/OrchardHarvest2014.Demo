using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Users.Services;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class CreateAccountActivity : Task {
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        public CreateAccountActivity(IUserService userService, IMembershipService membershipService) {
            _userService = userService;
            _membershipService = membershipService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "CreateAccount"; }
        }

        public override LocalizedString Category {
            get { return T("Account"); }
        }

        public override LocalizedString Description {
            get { return T("Create a new user account."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Done");         // The user was created.
            yield return T("Not Unique");   // The user name and or email address are already in use by someone else.
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var userName = workflowContext.Tokens["UserName"] as string;
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;
            var password = workflowContext.Tokens["Password"] as string;
            var isUnique = _userService.VerifyUserUnicity(userName, emailAddress);

            if (!isUnique) {
                yield return T("Not Unique");
            }
            else {
                var user = _membershipService.CreateUser(new CreateUserParams(
                    userName,
                    password,
                    emailAddress,
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true
                ));

                // Add the created user to the workflow state so that other activities can do something with it.
                workflowContext.SetState("User", user);

                yield return T("Done");                
            }
        }
    }
}