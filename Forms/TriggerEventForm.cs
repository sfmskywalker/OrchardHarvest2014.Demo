using Orchard;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Forms {
    public class TriggerEventForm : Component, IFormProvider {
        protected dynamic New { get; set; }

        public TriggerEventForm(IShapeFactory shapeFactory) {
            New = shapeFactory;
        }

        public void Describe(DescribeContext context) {
            context.Form("TriggerEvent",
                shape => {
                    var form = New.Form(
                        Id: "TriggerEvent",
                        _EventName: New.Textbox(
                            Id: "EventName", Name: "EventName",
                            Title: T("Event Name"),
                            Description: T("The name of the event to trigger."),
                            Classes: new[] {"medium", "text", "tokenized"}),
                        _Tokens: New.Textarea(
                            Id: "Tokens", Name: "Tokens",
                            Title: T("Tokens"),
                            Description: T("A list of key/value pairs to be parsed into a tokens dictionary. Use a key:value pair per line"),
                            Classes: new[] {"large", "text", "tokenized"})
                    );

                    return form;
                }
            );
        }
    }
}