using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC.Wizard.Controllers;
using MVC.Wizard.Web.ViewModels;

namespace MVC.Wizard.Web.Controllers
{
    public class SampleWizardController : WizardController<SampleWizardViewModel>
    {
        public ActionResult Sample()
        {
            //InitializeWizard();
            var vm = new SampleWizardViewModel("This is just some demo text to indicate that you can use startup data.");

            return View(vm);
        }

        //protected override void ProcessToNext(Wizard.ViewModels.WizardViewModel model)
        protected override void ProcessToNext(SampleWizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.
        }

        //protected override void ProcessToPrevious(Wizard.ViewModels.WizardViewModel model)
        protected override void ProcessToPrevious(SampleWizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.
        }
    }
}