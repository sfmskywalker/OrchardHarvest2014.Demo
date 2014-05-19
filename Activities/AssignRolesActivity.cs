using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using Orchard.Users.Services;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Activities {
    public class AssignRolesActivity : Task {
        private readonly IRepository<UserRolesPartRecord> _userRolesRepository;
        private readonly IRoleService _roleService;
        private readonly IMembershipService _membershipService;

        public AssignRolesActivity(IRepository<UserRolesPartRecord> userRolesRepository, IRoleService roleService, IMembershipService membershipService) {
            _userRolesRepository = userRolesRepository;
            _roleService = roleService;
            _membershipService = membershipService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "AssignRoles"; }
        }

        public override LocalizedString Category {
            get { return T("Account"); }
        }

        public override LocalizedString Description {
            get { return T("Assign user roles."); }
        }

        public override string Form {
            get { return "SelectRoles"; }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Done"); // The roles are assigned.
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var user = workflowContext.Content.As<IUser>();
            var roles = ParseRoles(activityContext.GetState<string>("Roles"));
            var currentUserRoleRecords = _userRolesRepository.Fetch(x => x.UserId == user.Id);
            var currentRoleRecords = currentUserRoleRecords.Select(x => x.Role);
            var targetRoleRecords = roles.Select(x => _roleService.GetRole(x));

            foreach (var role in targetRoleRecords.Where(x => !currentRoleRecords.Contains(x))) {
                _userRolesRepository.Create(new UserRolesPartRecord { UserId = user.Id, Role = role });
            }
            
            yield return T("Done");
        }

        private static IEnumerable<int> ParseRoles(string text) {
            return String.IsNullOrWhiteSpace(text) 
                ? Enumerable.Empty<int>() 
                : text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);
        }
    }
}