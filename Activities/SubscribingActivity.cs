using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class SubscribingActivity : Event {
        private readonly IMailingSubscriptionService _subscriptionService;

        public SubscribingActivity(IMailingSubscriptionService subscriptionService) {
            _subscriptionService = subscriptionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "Subscribing"; }
        }

        public override bool CanStartWorkflow {
            get { return true; }
        }

        public override LocalizedString Category {
            get { return T("MailingList"); }
        }

        public override LocalizedString Description {
            get { return T("A mailing list subscriber is coming in."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Email Address Is New");
            yield return T("Email Address Already Exists");
            yield return T("Email Address Is Inactive");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;

            // Add the address to the workflow state for easy access using the Workflow.State token.
            workflowContext.SetState("EmailAddress", emailAddress);

            // Check if the specified email address is already subscribed.
            var subscription = _subscriptionService.GetSubscriptionByEmailAddress(emailAddress);

            if (subscription != null) {
                if (subscription.IsActive) {
                    yield return T("Email Address Already Exists");
                }
                else {
                    yield return T("Email Address Is Inactive");
                }
            }
            else {
                yield return T("Email Address Is New");
            }
        }
    }
}