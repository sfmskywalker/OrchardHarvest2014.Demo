using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.ViewModels;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Controllers {
    [Themed]
    public class AccountController : Controller {
        private readonly IWorkflowManager _workflowManager;
        public AccountController(IWorkflowManager workflowManager) {
            _workflowManager = workflowManager;
        }

        public ActionResult SignUp() {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            // Trigger the SigningUp Activity.
            _workflowManager.TriggerEvent("SignUp", null, () => new Dictionary<string, object> {
                {"EmailAddress", model.EmailAddress},
                {"UserName", model.EmailAddress},
                {"Password", model.Password}
            });

            // If the workflow issued a redirect, we simply return HTTP 200 OK. Otherwise we redirect to the home page.
            return Response.IsRequestBeingRedirected ? (ActionResult)new EmptyResult() : Redirect("~/");
        }

        [Authorize]
        public ActionResult Dashboard() {
            return View();
        }
    }
}