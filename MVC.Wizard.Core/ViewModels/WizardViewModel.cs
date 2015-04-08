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
        public List<string> Steps
        {
            get
            {
                if (_steps == null)
                {
                    _steps = new List<string>();

                    IEnumerable<PropertyInfo> properties = this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(WizardStepAttribute)));

                    foreach (PropertyInfo property in properties)
        {
                        //steps.Add((WizardStep)property.GetMethod.Invoke(this, null));
                        _steps.Add(property.Name);
                    }
                }

                return _steps;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardViewModel"/> class.
        /// </summary>
        public WizardViewModel()
        {
            StepIndex = 1;
        }

        /// <summary>
        /// Gets the step index of the wizard.
        /// </summary>
        /// <value>The index of the step.</value>
        public int StepIndex { get; set; }

        /// <summary>
        /// Gets the errors of the current step.
        /// </summary>
        /// <value>The errors.</value>
        public List<WizardValidationResult> Errors { get; internal set; }
    }
}
