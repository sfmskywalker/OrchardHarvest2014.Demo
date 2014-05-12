using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class UnsubscribingActivity : Event {
        private readonly IMailingSubscriptionService _subscriptionService;

        public UnsubscribingActivity(IMailingSubscriptionService subscriptionService) {
            _subscriptionService = subscriptionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "Unsubscribing"; }
        }

        public override bool CanStartWorkflow {
            get { return true; }
        }

        public override LocalizedString Category {
            get { return T("MailingList"); }
        }

        public override LocalizedString Description {
            get { return T("A mailing list subscriber is unsubscribing."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Email Address Exists");
            yield return T("Email Address Is Inactive");
            yield return T("Email Address Does Not Exist");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;

            // Add the address to the workflow state for easy access using the Workflow.State token.
            workflowContext.SetState("EmailAddress", emailAddress);

            // Check if the specified email address exists.
            var subscription = _subscriptionService.GetSubscriptionByEmailAddress(emailAddress);

            if (subscription != null) {
                if (subscription.IsActive) {
                    yield return T("Email Address Exists");
                }
                else {
                    yield return T("Email Address Is Inactive");
                }
            }
            else {
                yield return T("Email Address Does Not Exist");
            }
        }
    }
}