﻿using MVC.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MVC.Wizard.Web.ViewModels
{
    public class SampleWizardViewModel : WizardViewModel
    {    
        public SampleWizardViewModel()
        {
            Step1 = new SampleWizardViewModelStep1();
            Step2 = new SampleWizardViewModelStep2();
            Step3 = new SampleWizardViewModelStep3();
            Step4 = new SampleWizardViewModelStep4();
            Step5 = new SampleWizardViewModelStep5();
            Step6 = new SampleWizardViewModelStep6();
        }

        public SampleWizardViewModel(string initialWizardValue)
            : this()
        {
            Step1.InitialWizardValue = initialWizardValue;
        }

        [WizardStep]
        public SampleWizardViewModelStep1 Step1 { get; set; }

        [WizardStep]
        public SampleWizardViewModelStep2 Step2 { get; set; }

        [WizardStep]
        public SampleWizardViewModelStep3 Step3 { get; set; }

        [WizardStep]
        public SampleWizardViewModelStep4 Step4 { get; set; }

        [WizardStep]
        public SampleWizardViewModelStep5 Step5 { get; set; }

        [WizardStep]
        public SampleWizardViewModelStep6 Step6 { get; set; }

    }
    
    public class SampleWizardViewModelStep1 : WizardStep
    {
        [Required]
        public string InitialWizardValue { get; set; }
    }

    public class SampleWizardViewModelStep2 : WizardStep
    {
        public override string Name
        {
            get
            {
                return "Custom title for step 2";
            }
        }

        [Required]
        public int? Required { get; set; }

        public string NotRequired { get; set; }

        [Required, StringLength(3, MinimumLength=2)]
        public string StringLength { get; set; }

        public string Dynamic { get; set; }

        public SampleWizardViewModelStep2()
        {
            Dynamic = "Static";
        }
    }

    public class SampleWizardViewModelStep3 : WizardStep
    {
        [Required]
        public int? RequiredDirectUpdate { get; set; }

        [Required]
        public string MyProperty { get; set; }

        [StringLength(3)]
        public string StringLengthDirectUpdate { get; set; }
    }

    public class SampleWizardViewModelStep4 : WizardStep, IValidatableObject
    {
        [Required]
        public int? RequiredNoClientValidation { get; set; }

        public string NotRequiredNoClientValidation { get; set; }

        [Required]
        public int? HigherThan17AndRequired { get; set; }

        public override bool EnableClientValidation
        {
            get { return false; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var higherThan17Property = new[] { "HigherThan17AndRequired" };

            // Check if the value is higher than 17
            if (!HigherThan17AndRequired.HasValue)
            {
                results.Add(new ValidationResult(
                      "HigherThan17AndRequired is required and must be a number",
                      higherThan17Property));
            }
            else if (HigherThan17AndRequired.Value < 18)
            {
                results.Add(new ValidationResult(
                    "HigherThan17AndRequired is not higher than 17",
                    higherThan17Property));
            }

            return results;
        }
    }

    public class SampleWizardViewModelStep5 : WizardStep, IValidatableObject
    {
        [Required]
        public bool IAgree { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var IAgreeProperty = new[] { "IAgree" };

            // some other random validation
            if (!IAgree)
            {
                results.Add(new ValidationResult(
                      "You have to agree to proceed",
                      IAgreeProperty));
            }

            return results;
        }
    }

    public class SampleWizardViewModelStep6 : WizardStep { }
}