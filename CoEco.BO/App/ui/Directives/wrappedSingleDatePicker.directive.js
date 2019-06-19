(function () {
    'use strict';

    angular.module('coeco.ui').directive('wrappedSingleDatePicker', wrappedSingleDatePicker);

    wrappedSingleDatePicker.$inject = ['datePickerLocale'];

    function wrappedSingleDatePicker(datePickerLocale) {

        return {
            link: link,
            restrict: 'E',
            template: '<input date-range-picker type="text" placeholder="{{placeholder}}" class="{{cssClass}}" options="options" ng-model="dateValue" ng-disabled="isDisabled" />',
            require: 'ngModel',
            scope: {
                ngModel: '=',
                isDisabled: '=',
                cssClass: '@',
                placeholder: '@'
            }
        };

        function link(scope, element, attrs, ngmodelController) {

            var defaultOptions = {
                startDate: scope.ngModel || moment(),
                endDate: scope.ngModel || moment(),
                singleDatePicker: true,
                showDropdowns: true,
                locale: datePickerLocale,
                eventHandlers: {
                    'apply.daterangepicker': onSelect,
                    'outsideClick.daterangepicker': onSelect
                }
            };

            scope.options = angular.extend(defaultOptions, scope.$eval(attrs.options) || {});
            scope.placeholder = attrs.placeholder;
            scope.cssClass = attrs.cssClass;
            scope.dateValue = scope.ngModel;

            function onSelect() {
                ngmodelController.$setViewValue(scope.dateValue.toDate());
            }

            scope.$watch(scope.ngModel, function () {
                if (moment.isMoment(scope.ngModel)) {
                    scope.dateValue = scope.ngModel;
                } else {
                    scope.dateValue = moment(scope.ngModel);
                }
            });

        }
    }

})();