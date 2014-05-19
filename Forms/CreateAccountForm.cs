using Orchard;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;

namespace OrchardHarvest2014.WorkflowsJobsDemo.Forms {
    public class CreateAccountForm : Component, IFormProvider {
        protected dynamic New { get; set; }

        public CreateAccountForm(IShapeFactory shapeFactory) {
            New = shapeFactory;
        }

        public void Describe(DescribeContext context) {
            context.Form("CreateAccount",
                shape => {
                    var form = New.Form(
                        Id: "CreateAccount",
                        _RequireEmailVerification: New.Checkbox(
                            Id: "RequireEmailVerification", Name: "RequireEmailVerification",
                            Title: T("Require Email Verification"),
                            Checked: false,
                            Value: "true",
                            Description: T("Require the user to verify her email address.")),
                        _EmailVerificationTimeout: New.Textbox(
                            Id: "EmailVerificationTimeout", Name: "EmailVerificationTimeout",
                            Title: T("Email Verification Timeout"),
                            Description: T("The time to allow the user to verify her email addres before the nonce expires. Format: d.hh:mm:ss"),
                            Value: "7.00:00:00",
                            Classes: new[] {"medium", "text", "tokenized"})
                    );

                    return form;
                }
            );
        }
    }
}