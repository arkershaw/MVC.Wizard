using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.Controllers;
using MVC.Wizard.Web.ViewModels;

namespace MVC.Wizard.Web.Controllers
{
    public class SampleWizardController : WizardController<SampleWizardViewModel>
    {
        public ActionResult Sample()
        {
            SampleWizardViewModel vm = new SampleWizardViewModel("This is just some demo text to indicate that you can use startup data.");
            return View(vm);
        }

        protected override async Task UpdateCurrentWizardStep(SampleWizardViewModel model)
        {
            // Custom code when the current step is updated
        }

        protected override async Task MoveToNextWizardStep(SampleWizardViewModel model)
        {
            // Custom code on moving to the next step
            if (model.StepIndex == 2)
            {
                model.Step2.Dynamic = "Dynamic";
            }
        }

        protected override async Task MoveToPreviousWizardStep(SampleWizardViewModel model)
        {
            // Custom code on moving to the previous step
        }
    }
}