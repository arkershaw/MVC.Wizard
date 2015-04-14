using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.Wizard.ViewModels
{
    [Serializable]
    public class WizardViewModel
    {
        private List<string> _steps = null;
        public List<string> StepNames
        {
            get
            {
                if (_steps == null)
                {
                    _steps = new List<string>();

                    IEnumerable<PropertyInfo> properties = this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(WizardStepAttribute)));

                    foreach (PropertyInfo property in properties)
        {
                        _steps.Add(property.Name);
                    }
                }

                return _steps;
            }
        }

        private int _stepIndex = 1;
        /// <summary>
        /// Gets/sets the step index of the wizard.
        /// </summary>
        /// <value>The index of the step.</value>
        public int StepIndex
        {
            get
            {
                return _stepIndex;
        }
            set
            {
                //_stepIndex = Math.Min(StepNames.Count, Math.Max(1, value));

                if (value < 1 || value > StepNames.Count)
                    throw new ApplicationException(string.Format("Step {0} does not exist", value));

                _stepIndex = value;
            }
        }

        /// <summary>
        /// Gets the errors of the current step.
        /// </summary>
        /// <value>The errors.</value>
        public List<WizardValidationResult> Errors { get; protected set; }

        public WizardViewModel()
        {
            Errors = new List<WizardValidationResult>();
        }

        //protected WizardStep GetStep()
        //{
        //    string stepName = StepNames[StepIndex - 1];
        //    PropertyInfo step = this.GetType().GetProperty(stepName);
        //    return step.GetGetMethod().Invoke(this, null) as WizardStep;
        //}

        //protected async Task<bool> UnloadWizardStep()
        //{
        //    WizardStep step = GetStep();

        //    if (step != null)
        //    {
        //        try
        //        {
        //            await step.Unload();
        //            return true;
        //        }
        //        catch (ValidationException ex)
        //        {
        //            AddError(ex);
        //        }
        //    }

        //    return false;
        //}

        //protected async Task<bool> LoadWizardStep()
        //{
        //    WizardStep step = GetStep();

        //    if (step != null)
        //    {
        //        try
        //        {
        //            await step.Load();
        //            return true;
        //        }
        //        catch (ValidationException ex)
        //        {
        //            AddError(ex);
        //        }
        //    }

        //    return false;
        //}

        //public bool UpdateWizardSetp(ModelStateDictionary modelStateDict)
        //{
        //    return UpdateWizardStepAsync(modelStateDict).Result;
        //}

        //public async Task<bool> UpdateWizardStepAsync(ModelStateDictionary modelStateDict)
        public bool UpdateWizardStep(ModelStateDictionary modelStateDict)
        {
            RemoveValidationRulesFromOtherSteps(modelStateDict);

            if (Validate(modelStateDict))
            {
                //WizardStep step = GetStep();

                //if (step != null)
                //{
                //    try
                //    {
                //        await step.Update();
                //        return true;
                //    }
                //    catch (ValidationException ex)
                //    {
                //        AddError(ex);
                //    }
                //}

                //return false;
                return true;
            }
            else
                return false;
        }

        //public bool PreviousWizardStep(ModelStateDictionary modelStateDict)
        //{
        //    return PreviousWizardStepAsync(modelStateDict).Result;
        //}

        //public async Task<bool> PreviousWizardStepAsync(ModelStateDictionary modelStateDict)
        public bool PreviousWizardStep(ModelStateDictionary modelStateDict)
        {
            modelStateDict.Clear();
            Errors.Clear();

            //await UnloadWizardStep();

            StepIndex--;

            //await LoadWizardStep();

            return true;
        }

        //public bool NextWizardStep(ModelStateDictionary modelStateDict)
        //{
        //    return NextWizardStepAsync(modelStateDict).Result;
        //}

        //public async Task<bool> NextWizardStepAsync(ModelStateDictionary modelStateDict)
        public bool NextWizardStep(ModelStateDictionary modelStateDict)
        {
            RemoveValidationRulesFromOtherSteps(modelStateDict);

            if (Validate(modelStateDict))
            {
                //await UnloadWizardStep();

                StepIndex++;

                //bool success = await LoadWizardStep();
                //if (!success)
                //    StepIndex--;

                //return success;
                return true;
            }
            else
                return false;
        }

        public void AddError(ValidationException ex)
        {
            //Errors.Add(new WizardValidationResult() { MemberName = string.Join(",", ex.ValidationResult.MemberNames), Message = ex.ValidationResult.ErrorMessage });

            foreach (string member in ex.ValidationResult.MemberNames)
            {
                AddError(member, ex.ValidationResult.ErrorMessage);
            }
        }

        public void AddError(string memberName, string message)
        {
            if (Errors.Count == 0)
                StepIndex--;

            Errors.Add(new WizardValidationResult { MemberName = memberName, Message = message });
        }

        protected void RemoveValidationRulesFromOtherSteps(ModelStateDictionary modelStateDict)
        {
            //StepIndex starts at 1
            for (int i = StepNames.Count - 1; i >= StepIndex; i--)
            {
                string prefix = string.Concat(StepNames[i], ".");
                foreach (KeyValuePair<string, ModelState> state in modelStateDict.Where(m => m.Key.StartsWith(prefix)).ToList())
                {
                    modelStateDict.Remove(state.Key);
                }
            }
        }

        protected bool Validate(ModelStateDictionary modelStateDict)
        {
            Errors.Clear();

            if (!modelStateDict.IsValid)
            {
                foreach (KeyValuePair<string, ModelState> modelState in modelStateDict)
                {
                    if (modelState.Value.Errors.Any())
                        Errors.Add(new WizardValidationResult { MemberName = modelState.Key, Message = modelState.Value.Errors[0].ErrorMessage });
                }
            }

            return modelStateDict.IsValid;
        }
    }
}
