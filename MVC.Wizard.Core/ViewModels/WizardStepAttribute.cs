using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Wizard.ViewModels
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WizardStepAttribute : Attribute { }
}
