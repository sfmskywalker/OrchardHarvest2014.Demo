using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class TriggerEventActivity : Task {
        private readonly IWorkflowManager _workflowManager;

        public TriggerEventActivity(IWorkflowManager workflowManager) {
            _workflowManager = workflowManager;
        }

        public override string Name {
            get { return "TriggerEvent"; }
        }

        public override LocalizedString Description {
            get { return T("Triggers a workflow event."); }
        }

        public Localizer T { get; set; }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return new[] { T("Done") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var eventName = activityContext.GetState<string>("EventName");
            var tokensText = activityContext.GetState<string>("Tokens");

            _workflowManager.TriggerEvent(eventName, workflowContext.Content, () => {
                var tokensDictionary = ParseTokensText(tokensText);
                tokensDictionary["Content"] = workflowContext.Content;
                return tokensDictionary;
            });

            yield return T("Done");
        }

        public override string Form {
            get {
                return "TriggerEvent";
            }
        }

        public override LocalizedString Category {
            get { return T("Workflows"); }
        }

        private Dictionary<string, object> ParseTokensText(string text) {
            var dictionary = new Dictionary<string, object>();

            if(String.IsNullOrWhiteSpace(text))
                return dictionary;

            var lines = Regex.Split(text, @"\r\n");

            foreach (var line in lines) {
                var pair = line.Split(new[] {':'});
                var key = pair[0].Trim();
                var value = pair[1].Trim();

                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}