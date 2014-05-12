using Orchard;
using OrchardHarvest2014.WorkflowsJobsDemo.Models;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Services {
    public interface IMailingSubscriptionService : IDependency {
        Subscription GetSubscriptionByEmailAddress(string emailAddress);
        Subscription CreateSubscription(string emailAddress);
        void DeactivateSubscription(Subscription subscription);
        void ActivateSubscription(Subscription subscription);
    }
}