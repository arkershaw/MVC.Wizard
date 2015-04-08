using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.ViewModels;

namespace MVC.Wizard.Controllers
{
    public class WizardController<T> : Controller where T : WizardViewModel
    {
        [HttpPost]
        public virtual async Task<ActionResult> UpdateWizardStep(T model)
        {
            RemoveValidationRulesFromOtherSteps(model);

            Validate(ModelState, model);

            await ProcessToUpdate(model);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> PreviousWizardStep(T model)
        {
            ModelState.Clear();
            model.Errors = null;
            model.StepIndex--;

            await ProcessToPrevious(model);

            return Json(model);
        }

        [HttpPost]
        public async virtual Task<ActionResult> NextWizardStep(T model)
        {
            RemoveValidationRulesFromOtherSteps(model);

            if (Validate(ModelState, model))
            {
                model.Errors = null;
                model.StepIndex++;

                try
                {
                    await ProcessToNext(model);
                }
                catch (ValidationException valEx)
                {
                    // Catch custom exceptions so decrease the stepindex
                    model.StepIndex--;

                    // Return the errors to the client
                    model.Errors = new List<WizardValidationResult>();                    
                    model.Errors.Add(new WizardValidationResult { MemberName = string.Empty, Message = valEx.ValidationResult.ErrorMessage });
                }
            }

            return Json(model);
        }

        protected void RemoveValidationRulesFromOtherSteps(WizardViewModel model)
        {
            //StepIndex starts at 1
            for (int i = model.Steps.Count - 1; i >= model.StepIndex; i--)
            {
                string prefix = string.Concat(model.Steps[i], ".");
                foreach (KeyValuePair<string, ModelState> state in ModelState.Where(m => m.Key.StartsWith(prefix)).ToList())
                {
                    ModelState.Remove(state.Key);
                }
            }
        }

        protected async virtual Task ProcessToUpdate(T model)
        {
        }

        protected async virtual Task ProcessToPrevious(T model)
        {
        }

        protected async virtual Task ProcessToNext(T model)
        {
        }

        protected bool Validate(ModelStateDictionary modelStateDict, WizardViewModel viewModel)
        {
            viewModel.Errors = new List<WizardValidationResult>();

            if (!modelStateDict.IsValid)
            {
                // deze foreach voegt alle attribute errors toe
                foreach (var modelState in modelStateDict)
                {
                    if (modelState.Value.Errors.Any())
                    {
                        viewModel.Errors.Add(new WizardValidationResult { MemberName = modelState.Key, Message = modelState.Value.Errors[0].ErrorMessage });
                    }
                }
            }

            return modelStateDict.IsValid;
        }

    }
}
