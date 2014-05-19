using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Roles.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Shapes {
    public class AssignRolesActivityShape : IShapeTableProvider {
        private readonly IRoleService _roleService;
        public AssignRolesActivityShape(IRoleService roleService) {
            _roleService = roleService;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Activity").OnDisplaying(context => {
                var activityName = context.Shape.Name;

                if (activityName != "AssignRoles")
                    return;

                var allRoles = _roleService.GetRoles().ToDictionary(x => x.Id);
                var roleIds = ParseRoles((string)context.Shape.State.Roles);
                var roles = roleIds.Select(x => allRoles[x]).ToArray();

                context.Shape.Roles = roles;
            });
        }

        private static IEnumerable<int> ParseRoles(string text) {
            return String.IsNullOrWhiteSpace(text)
                ? Enumerable.Empty<int>()
                : text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);
        }
    }
}