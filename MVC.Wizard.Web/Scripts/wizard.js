(function (factory) {
    if (typeof define === 'function' && define.amd) {
        define(['jquery', 'knockout', 'knockout-mapping', 'jquery-validate'], factory);
    } else {
        factory(jQuery, ko, ko.mapping);
    }
}(function ($, ko, mapping) {
    if (ko.mapping === undefined)
        ko.mapping = mapping;

    ko.bindingHandlers.wizardStep = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var stepIndex = ko.unwrap(valueAccessor());
            var model = bindingContext.$data.Model;

            var step = eval('model.' + model.StepNames()[stepIndex - 1]);

            if (!step.EnableClientValidation())
                $(element).find(':input').attr('data-val', 'false');

            if (stepIndex == model.StepNames().length)
            {
                var form = $(element).closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            }
        },

        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var stepIndex = ko.unwrap(valueAccessor());
            var model = bindingContext.$data.Model;

            if (stepIndex == model.StepIndex())
                $(element).show();
            else
                $(element).hide();

            if (!model.CurrentStep().EnableClientValidation()) {
                var form = $(element).closest('form');
                var validator = form.validate();
                form.find('.field-validation-error span').each(function () {
                    validator.settings.success($(this));
                });
                validator.resetForm();
            }
        }
    };

    $.fn.Wizard = function (options) {

        // Append ul with error messages in li that are not for a specific property
        this.find('.wizard-errors-summary').append('<ul data-bind="foreach: Model.GeneralErrors, visible: Model.GeneralErrors().length > 0"><li data-bind="text: Message"></li></ul>');

        function SetServerErrors(errorsList) {
            // Check if client side validation is enabled and if we have errors
            if (errorsList) {
                // Get the validator from the form
                var validator = $("#" + options.formId).validate();

                // Remove old errors
                validator.resetForm();

                // Create new errors
                var errors = {};
                for (var i = 0; i < errorsList.length; i++) {
                    if (errorsList[i].MemberName !== '') {
                        errors[errorsList[i].MemberName] = errorsList[i].Message;
                    }
                }

                // Show errors
                validator.showErrors(errors);
            }
        }

        var ViewModel = function (d, m) {
            var self = this;

            self.Model = ko.mapping.fromJS(d, m);

            self.Model.GeneralErrors = ko.computed(function () {
                return ko.utils.arrayFilter(self.Model.Errors(), function (item) {
                    return !!item.MemberName;
                });
            });

            self.Model.CurrentStep = ko.computed(function () {
                return eval('self.Model.' + self.Model.StepNames()[self.Model.StepIndex() - 1]);
            });

            self.Model.TotalSteps = ko.computed(function () {
                return self.Model.StepNames().length;
            });

            self.Model.Update = function (element) {
                self.Update(element);
            }

            self.Next = function (element) {
                var validator = $(element).closest('form').validate();

                if ($(element).closest('form').valid()) {
                    self.RoundTrip('NextWizardStep');
                }
                else {
                    validator.focusInvalid();
                }
            }

            self.Previous = function () {
                self.RoundTrip('PreviousWizardStep');
            }

            self.Update = function (element) {
                var validator = $(element).closest('form').validate();

                if ($(element).closest('form').valid()) {
                    //if (self.UpdateOnChange) {
                    //    self.RoundTrip("UpdateWizardStep");
                    //}
                }
                else {
                    validator.focusInvalid();
                }
            }

            self.RoundTrip = function (action) {
                $.ajax({
                    url: options.url + action,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: ko.toJSON(self.Model),
                    success: function (data) {
                        //self.UpdateOnChange = false;
                        ko.mapping.fromJS(data, self.Model);
                        //self.UpdateOnChange = true;

                        SetServerErrors(data.Errors);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    }
                });
            }
        }

        var vm = new ViewModel(options.model, options.mapping);

        ko.applyBindings(vm, this[0]);

        //vm.UpdateOnChange = true;

        return this;
    };
}));