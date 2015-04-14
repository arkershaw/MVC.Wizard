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
            if (model.UpdateWizardStep(ModelState))
            {
                DoUpdateWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult PreviousWizardStep(T model)
        {
            if (model.PreviousWizardStep(ModelState))
            {
                DoPreviousWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult NextWizardStep(T model)
        {
            if (model.NextWizardStep(ModelState))
            {
                try
                {
                    DoNextWizardStep(model);
                }
                catch (ValidationException valEx)
                {
                    model.AddError(valEx);
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
            if (model.UpdateWizardStep(ModelState))
            {
                await DoUpdateWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> PreviousWizardStep(T model)
        {
            if (model.PreviousWizardStep(ModelState))
            {
                await DoPreviousWizardStep(model);
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> NextWizardStep(T model)
        {
            if (model.NextWizardStep(ModelState))
            {
                try
                {
                    await DoNextWizardStep(model);
                }
                catch (ValidationException valEx)
                {
                    model.AddError(valEx);
                }
            }

            return Json(model);
        }

        protected abstract Task DoUpdateWizardStep(T model);
        protected abstract Task DoPreviousWizardStep(T model);
        protected abstract Task DoNextWizardStep(T model);
    }
}
