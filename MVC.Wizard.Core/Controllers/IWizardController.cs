using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVC.Wizard.ViewModels;

namespace MVC.Wizard.Controllers
{
    public interface IWizardController<T> where T : WizardViewModel
    {
        JsonResult UpdateWizardStep(T model);
        JsonResult PreviousWizardStep(T model);
        JsonResult NextWizardStep(T model);
    }

    public interface IWizardControllerAsync<T> where T : WizardViewModel
    {
        Task<JsonResult> UpdateWizardStep(T model);
        Task<JsonResult> PreviousWizardStep(T model);
        Task<JsonResult> NextWizardStep(T model);
    }
}
