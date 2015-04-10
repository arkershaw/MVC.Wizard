using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.Controllers;
using MVC.Wizard.Web.ViewModels;

namespace MVC.Wizard.Web.Controllers
{
    public class SampleWizardController : WizardControllerBase<SampleWizardViewModel>
    {
        public ActionResult Sample()
        {
            SampleWizardViewModel vm = new SampleWizardViewModel("This is just some demo text to indicate that you can use startup data.");
            return View(vm);
        }

        protected override void DoUpdateWizardStep(SampleWizardViewModel model)
        {
            // Custom code when the current step is updated
        }

        protected override void DoPreviousWizardStep(SampleWizardViewModel model)
        {
            // Custom code on moving to the previous step
        }

        protected override void DoNextWizardStep(SampleWizardViewModel model)
        {
            // Custom code on moving to the next step
            if (model.StepIndex == 2)
                model.Step2.Dynamic = "Dynamic";
        }
    }
}