using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class SubscribeActivity : Task {
        private readonly IMailingSubscriptionService _subscriptionService;

        public SubscribeActivity(IMailingSubscriptionService subscriptionService) {
            _subscriptionService = subscriptionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "Subscribe"; }
        }

        public override LocalizedString Category {
            get { return T("MailingList"); }
        }

        public override LocalizedString Description {
            get { return T("Subscribes an email address."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Success");
            yield return T("Duplicate");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var emailAddress = workflowContext.Tokens["EmailAddress"] as string;

            // Check if the specified email address is already subscribed.
            var subscription = _subscriptionService.GetSubscriptionByEmailAddress(emailAddress);

            if (subscription != null) {
                if (subscription.IsActive) {
                    yield return T("Duplicate");
                }
                else {
                    _subscriptionService.ActivateSubscription(subscription);
                    yield return T("Success");
                }
            }
            else {
                // Create the subscription.
                subscription = _subscriptionService.CreateSubscription(emailAddress);
                yield return T("Success");
            }

            // Add the subscription to the workflow context state for other activities to consume.
            workflowContext.SetState("Subscription", subscription);
        }
    }
}