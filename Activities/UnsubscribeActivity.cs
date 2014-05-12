using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class UnsubscribeActivity : Task {
        private readonly IMailingSubscriptionService _subscriptionService;

        public UnsubscribeActivity(IMailingSubscriptionService subscriptionService) {
            _subscriptionService = subscriptionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "Unsubscribe"; }
        }

        public override LocalizedString Category {
            get { return T("MailingList"); }
        }

        public override LocalizedString Description {
            get { return T("Unsubscribes an email address."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Success");
            yield return T("Not Found");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;

            // Check if the specified email address is subscribed.
            var subscription = _subscriptionService.GetSubscriptionByEmailAddress(emailAddress);

            if (subscription == null) {
                yield return T("Not Found");
            }
            else {
                // Create the subscription.
                _subscriptionService.DeactivateSubscription(subscription);

                // Add the subscription to the workflow context state for other activities to consume.
                workflowContext.SetState("Subscription", subscription);
                yield return T("Success");
            }
        }
    }
}