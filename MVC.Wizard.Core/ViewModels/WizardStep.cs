using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;

namespace MVC.Wizard.ViewModels
{
    public abstract class WizardStep
    {
        public virtual string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether client validation is enabled.
        /// This must be set to true if:
        /// there is a simple form with a few editors
        /// and there is no server validation for a property on the current step
        /// and there is no validation between multiple steps.
        /// If this is set to true and above criteria aren't past, you could get strange error messages appear after all other validation are satisfied.
        /// Warning: Setting this to true could result in slow performance for the current step and extra processing on the server.
        /// </summary>
        /// <value><c>true</c> if client validation must be enabled; otherwise, <c>false</c>.</value>
        public virtual bool EnableClientValidation
        {
            get
            {
                return true;
            }
        }

        //public async virtual Task Load() { }
        //public async virtual Task Unload() { }
        //public async virtual Task Update() { }
    }
}