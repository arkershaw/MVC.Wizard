using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                _stepIndex = Math.Min(StepNames.Count, Math.Max(1, value));
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
    }
}
