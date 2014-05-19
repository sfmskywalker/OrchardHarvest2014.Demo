using System;
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

        public override string Form {
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
            var requireEmailVerification = activityContext.GetState<bool>("RequireEmailVerification");
            var emailVerificationTimeout = ToTimeSpan(activityContext.GetState<string>("EmailVerificationTimeout"), TimeSpan.FromDays(7));

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
                    isApproved: !requireEmailVerification
                ));

                var nonce = _userService.CreateNonce(user, emailVerificationTimeout.Value);

                workflowContext.SetState("Nonce", nonce);
                workflowContext.Content = user;
                workflowContext.Record.ContentItemRecord = user.ContentItem.Record;
                yield return T("Done");                
            }
        }

        private static TimeSpan? ToTimeSpan(string value, TimeSpan? defaultValue = null) {
            if (String.IsNullOrWhiteSpace(value))
                return defaultValue;

            TimeSpan timeSpan;
            if (!TimeSpan.TryParse(value, out timeSpan))
                return defaultValue;

            return timeSpan;
        }
    }
}