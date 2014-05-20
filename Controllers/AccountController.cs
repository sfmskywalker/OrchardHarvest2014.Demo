using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.Security;
using Orchard.Services;
using Orchard.Themes;
using Orchard.Users.Services;
using Orchard.Workflows.Services;
using OrchardHarvest2014.WorkflowsJobsDemo.ViewModels;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Controllers {
    [Themed]
    public class AccountController : Controller {
        private readonly IWorkflowManager _workflowManager;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IClock _clock;

        public AccountController(IWorkflowManager workflowManager, IMembershipService membershipService, IUserService userService, IClock clock) {
            _workflowManager = workflowManager;
            _membershipService = membershipService;
            _userService = userService;
            _clock = clock;
        }

        public ActionResult SignUp() {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            // Trigger the SignUp workflow event.
            _workflowManager.TriggerEvent("SignUp", null, () => new Dictionary<string, object> {
                {"FirstName", model.FirstName},
                {"LastName", model.LastName},
                {"EmailAddress", model.EmailAddress},
                {"UserName", model.EmailAddress},
                {"Password", model.Password}
            });

            // If the workflow issued a redirect, we simply return HTTP 200 OK. Otherwise we redirect to the home page.
            return Response.IsRequestBeingRedirected ? (ActionResult)new EmptyResult() : Redirect("~/");
        }

        public ActionResult Activate(string nonce) {
            var user = ValidateChallenge(nonce);

            if (user == null)
                return HttpNotFound();

            // Trigger the ActivateAccount workflow event.
            _workflowManager.TriggerEvent("ActivateAccount", user, () => new Dictionary<string, object>());

            // If the workflow issued a redirect, we simply return HTTP 200 OK. Otherwise we redirect to the home page.
            return Response.IsRequestBeingRedirected ? (ActionResult)new EmptyResult() : Redirect("~/");
        }

        [Authorize]
        public ActionResult Dashboard() {
            return View();
        }

        // Copied from UserService.ValidateChallenge, but without side effects (not updating the EmailStatus to Approved).
        private IUser ValidateChallenge(string nonce) {
            string username;
            DateTime validateByUtc;

            if (!_userService.DecryptNonce(nonce, out username, out validateByUtc)) {
                return null;
            }

            if (validateByUtc < _clock.UtcNow)
                return null;

            var user = _membershipService.GetUser(username);
            if (user == null)
                return null;

            return user;
        }
    }
}