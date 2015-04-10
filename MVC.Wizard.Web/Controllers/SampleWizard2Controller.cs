using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.Controllers;
using MVC.Wizard.Web.ViewModels;

namespace MVC.Wizard.Web.Controllers
{
    public class SampleWizard2Controller : Controller, IWizardController<SampleWizardViewModel>
    {
        public ActionResult Sample()
        {
            SampleWizardViewModel vm = new SampleWizardViewModel("This is just some demo text to indicate that you can use startup data.");
            return View(vm);
        }

        [HttpPost]
        public JsonResult UpdateWizardStep(SampleWizardViewModel model)
        {
            if (WizardHelper.UpdateWizardStep(ModelState, model))
            {
                // Custom code when the current step is updated
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult PreviousWizardStep(SampleWizardViewModel model)
        {
            if (WizardHelper.PreviousWizardStep(ModelState, model))
            {
                // Custom code on moving to the previous step
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult NextWizardStep(SampleWizardViewModel model)
        {
            if (WizardHelper.NextWizardStep(ModelState, model))
            {
                // Custom code on moving to the next step

                if (model.StepIndex == 2)
                    model.Step2.Dynamic = "Dynamic";
            }

            return Json(model);
        }
    }
}