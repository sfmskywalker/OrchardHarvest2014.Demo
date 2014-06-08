using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Handlers {

    public class WorkflowContentHandler : ContentHandler {

        public WorkflowContentHandler(IWorkflowManager workflowManager) {

            OnUpdated<ContentPart>(
                (context, part) => {
                    if(context.ContentItemRecord == null) {
                        return;
                    }

                    workflowManager.TriggerEvent(
                        "ContentUpdatedByOwner",
                        context.ContentItem,
                        () => new Dictionary<string, object> { { "Content", context.ContentItem } }
                    );
                });
        }
    }
}