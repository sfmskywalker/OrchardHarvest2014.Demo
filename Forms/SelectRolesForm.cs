using System.Web.Mvc;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Roles.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Forms {
    public class SelectRolesForm : Component, IFormProvider {
        private readonly IRoleService _roleService;
        protected dynamic New { get; set; }

        public SelectRolesForm(IShapeFactory shapeFactory, IRoleService roleService) {
            New = shapeFactory;
            _roleService = roleService;
        }

        public void Describe(DescribeContext context) {
            context.Form("SelectRoles",
                shape => {
                    var form = New.Form(
                        Id: "SelectRoles",
                        _Roles: New.SelectList(
                            Id: "Roles", Name: "Roles",
                            Title: T("Roles"),
                            Description: T("Select the roles to assign."),
                            Multiple: true,
                            Size: 10)
                    );

                    var availableRoles = _roleService.GetRoles();

                    foreach (var role in availableRoles) {
                        form._Roles.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name });    
                    }
                    
                    return form;
                }
            );
        }
    }
}