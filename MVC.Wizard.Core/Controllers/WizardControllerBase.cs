using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MVC.Wizard.Controllers
{
    public abstract class WizardControllerBase<T> : Controller, IWizardController<T> where T : WizardViewModel
    {
        [HttpPost]
        public JsonResult UpdateWizardStep(T model)
        {
            if (WizardHelper.UpdateWizardStep(ModelState, model))
            {
                DoUpdateWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult PreviousWizardStep(T model)
        {
            if (WizardHelper.PreviousWizardStep(ModelState, model))
            {
                DoPreviousWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult NextWizardStep(T model)
        {
            if (WizardHelper.NextWizardStep(ModelState, model))
            {
                try
                {
                    DoNextWizardStep(model);
                }
                catch (ValidationException valEx)
                {
                    model.StepIndex--;
                    model.Errors.Add(new WizardValidationResult { MemberName = string.Join(",", valEx.ValidationResult.MemberNames), Message = valEx.ValidationResult.ErrorMessage });
                }
            }

            return Json(model);
        }

        protected abstract void DoUpdateWizardStep(T model);
        protected abstract void DoPreviousWizardStep(T model);
        protected abstract void DoNextWizardStep(T model);
    }

    public abstract class WizardControllerBaseAsync<T> : Controller, IWizardControllerAsync<T> where T : WizardViewModel
    {
        [HttpPost]
        public async Task<JsonResult> UpdateWizardStep(T model)
        {
            if (WizardHelper.UpdateWizardStep(ModelState, model))
            {
                await DoUpdateWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> PreviousWizardStep(T model)
        {
            if (WizardHelper.PreviousWizardStep(ModelState, model))
            {
                await DoPreviousWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> NextWizardStep(T model)
        {
            if (WizardHelper.NextWizardStep(ModelState, model))
            {
                try
                {
                    await DoNextWizardStep(model);
                }
                catch (ValidationException valEx)
                {
                    model.StepIndex--;
                    model.Errors.Add(new WizardValidationResult { MemberName = string.Join(",", valEx.ValidationResult.MemberNames), Message = valEx.ValidationResult.ErrorMessage });
                }
            }

            return Json(model);
        }

        protected abstract Task DoUpdateWizardStep(T model);
        protected abstract Task DoPreviousWizardStep(T model);
        protected abstract Task DoNextWizardStep(T model);
    }
}
