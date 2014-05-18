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
            var userName = workflowContext.GetState<string>("UserName");
            var emailAddress = workflowContext.GetState<string>("EmailAddress");
            var password = workflowContext.GetState<string>("Password");
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

                workflowContext.Content = user;
                yield return T("Done");                
            }
        }
    }
}