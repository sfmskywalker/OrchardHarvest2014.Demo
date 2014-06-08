using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class ContentUpdatedActivity : Event {
        private readonly IAuthenticationService _authenticationService;
        public ContentUpdatedActivity(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        public override string Name {
            get { return "ContentUpdatedByOwner"; }
        }

        public override LocalizedString Description {
            get { return T("Content is updated by its owner."); }
        }

        public Localizer T { get; set; }

        public override bool CanStartWorkflow {
            get { return true; }
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext) {
            try {

                var contentTypesState = activityContext.GetState<string>("ContentTypes");

                // "" means 'any'
                if (String.IsNullOrEmpty(contentTypesState)) {
                    return true;
                }

                var contentTypes = contentTypesState.Split(',');
                var content = workflowContext.Content;
                var currentUser = _authenticationService.GetAuthenticatedUser();

                if (content == null || currentUser == null || content.As<ICommonPart>().Owner.Id != currentUser.Id) {
                    return false;
                }

                return contentTypes.Any(contentType => content.ContentItem.TypeDefinition.Name == contentType);
            }
            catch {
                return false;
            }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return new[] { T("Done") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Done");
        }

        public override string Form {
            get {
                return "SelectContentTypes";
            }
        }

        public override LocalizedString Category {
            get { return T("Content Items"); }
        }
    }
}