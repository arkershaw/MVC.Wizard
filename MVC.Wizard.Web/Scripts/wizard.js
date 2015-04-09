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

            if (stepIndex === model.StepNames().length) {
                var form = $(element).closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            }
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var stepIndex = ko.unwrap(valueAccessor());
            var model = bindingContext.$data.Model;

            if (stepIndex === model.StepIndex())
                $(element).show();
            else
                $(element).hide();
        }
    };

    $.fn.Wizard = function (options) {
        function setServerErrors(errorsList) {
            var form = $('#' + options.formId);
            var validator = form.validate();

            form.find('.field-validation-error span').each(function () {
                validator.settings.success($(this));
            });

            validator.resetForm();

            if (errorsList && errorsList.length > 0) {
                var errors = {};
                for (var i = 0; i < errorsList.length; i++) {
                    if (errorsList[i].MemberName !== '') {
                        errors[errorsList[i].MemberName] = errorsList[i].Message;
                    }
                }

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

            self.Next = function () {
                self.ValidateAndPost('NextWizardStep');
            }

            self.Previous = function () {
                self.PostForm('PreviousWizardStep');
            }

            self.Update = function () {
                if (self.UpdateOnChange)
                    self.ValidateAndPost('UpdateWizardStep');
                else
                    self.ValidateAndPost();
            }

            self.ValidateAndPost = function (action) {
                var form = $('#' + options.formId);
                var validator = form.validate();

                if (form.valid()) {
                    if (action)
                        self.PostForm(action);
                }
                else
                    validator.focusInvalid();
            }

            self.PostForm = function (action) {
                $.ajax({
                    url: options.url + action,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: ko.toJSON(self.Model),
                    success: function (data) {
                        self.UpdateOnChange = false;
                        ko.mapping.fromJS(data, self.Model);
                        self.UpdateOnChange = true;

                        setServerErrors(data.Errors);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    }
                });
            }
        }

        var viewModel = new ViewModel(options.model, options.mapping);

        ko.applyBindings(viewModel, this[0]);

        viewModel.UpdateOnChange = true;

        return this;
    };
}));