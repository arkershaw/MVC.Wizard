using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC.Wizard.ViewModels;

namespace MVC.Wizard
{
    public sealed class WizardHelper
    {
        public static bool UpdateWizardStep(ModelStateDictionary modelStateDict, WizardViewModel model)
        {
            RemoveValidationRulesFromOtherSteps(modelStateDict, model);
            if (Validate(modelStateDict, model))
                return true;
            else
                return false;
        }

        public static bool PreviousWizardStep(ModelStateDictionary modelStateDict, WizardViewModel model)
        {
            modelStateDict.Clear();
            model.Errors.Clear();
            model.StepIndex--;
            return true;
        }

        public static bool NextWizardStep(ModelStateDictionary modelStateDict, WizardViewModel model)
        {
            RemoveValidationRulesFromOtherSteps(modelStateDict, model);

            if (Validate(modelStateDict, model))
            {
                model.StepIndex++;
                return true;
            }
            else
                return false;
        }

        protected static void RemoveValidationRulesFromOtherSteps(ModelStateDictionary modelStateDict, WizardViewModel model)
        {
            //StepIndex starts at 1
            for (int i = model.StepNames.Count - 1; i >= model.StepIndex; i--)
            {
                string prefix = string.Concat(model.StepNames[i], ".");
                foreach (KeyValuePair<string, ModelState> state in modelStateDict.Where(m => m.Key.StartsWith(prefix)).ToList())
                {
                    modelStateDict.Remove(state.Key);
                }
            }
        }

        protected static bool Validate(ModelStateDictionary modelStateDict, WizardViewModel model)
        {
            model.Errors.Clear();

            if (!modelStateDict.IsValid)
            {
                foreach (KeyValuePair<string, ModelState> modelState in modelStateDict)
                {
                    if (modelState.Value.Errors.Any())
                        model.Errors.Add(new WizardValidationResult { MemberName = modelState.Key, Message = modelState.Value.Errors[0].ErrorMessage });
                }
            }

            return modelStateDict.IsValid;
        }
    }
}
