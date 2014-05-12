using System;
using Orchard.Data;
using Orchard.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.Models;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Services {
    public class MailingSubscriptionService : IMailingSubscriptionService {
        private readonly IRepository<Subscription> _subscriptionRepository;
        private readonly IClock _clock;

        public MailingSubscriptionService(IRepository<Subscription> subscriptionRepository, IClock clock) {
            _subscriptionRepository = subscriptionRepository;
            _clock = clock;
        }

        public Subscription GetSubscriptionByEmailAddress(string emailAddress) {
            return _subscriptionRepository.Get(x => x.EmailAddress == emailAddress);
        }

        public Subscription CreateSubscription(string emailAddress) {
            var subscription = new Subscription {
                EmailAddress = emailAddress,
                ActivatedUtc = _clock.UtcNow,
                CreatedUtc = _clock.UtcNow,
                IsActive = true
            };

            _subscriptionRepository.Create(subscription);
            return subscription;
        }

        public void DeactivateSubscription(Subscription subscription) {
            subscription.IsActive = false;
            subscription.DeactivatedUtc = _clock.UtcNow;
        }

        public void ActivateSubscription(Subscription subscription) {
            if(subscription.IsActive)
                throw new InvalidOperationException("The specified subscription is already active.");

            subscription.IsActive = true;
            subscription.ActivatedUtc = _clock.UtcNow;
        }
    }
}