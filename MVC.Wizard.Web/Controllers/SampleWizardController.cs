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

        protected override async Task ProcessToNext(SampleWizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.
        }

        protected override async Task ProcessToPrevious(SampleWizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.
        }
    }
}