using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.ViewModels;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Controllers {
    [Themed]
    public class MailingListController : Controller {
        private readonly IWorkflowManager _workflowManager;
        public MailingListController(IWorkflowManager workflowManager) {
            _workflowManager = workflowManager;
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Subscribe() {
            return View();
        }

        [HttpPost]
        public ActionResult Subscribe(SubscribeViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            // Trigger the Subscribing Activity.
            _workflowManager.TriggerEvent("Subscribing", null, () => new Dictionary<string, object> {
                {"EmailAddress", model.EmailAddress}
            });

            // If the workflow issued a redirect, we simply return HTTP 200 OK. Otherwise we redirect to the home page.
            return Response.IsRequestBeingRedirected ? (ActionResult)new EmptyResult() : Redirect("~/");
        }

        public ActionResult Unsubscribe(string email) {
            var viewModel = new UnsubscribeViewModel {
                EmailAddress = email
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Unsubscribe(UnsubscribeViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            // Trigger the Unsubscribing Activity.
            _workflowManager.TriggerEvent("Unsubscribing", null, () => new Dictionary<string, object> {
                {"EmailAddress", model.EmailAddress}
            });

            // If the workflow issued a redirect, we simply return HTTP 200 OK. Otherwise we redirect to the home page.
            return Response.IsRequestBeingRedirected ? (ActionResult)new EmptyResult() : Redirect("~/");
        }
    }
}