(function () {
    'use strict';

    angular
        .module('coeco.ui')
        .component('wrappedDatepicker',
        {
            bindToController: true,
            bindings: {
                startDate: '<',
                endDate: '<',
                onChange: '&',
                cssClass: '@',
                settings:'<'
            },
            template: `<input ng-if="vm.dateRange" date-range-picker ng-class="vm.cssClass" class="date-picker"  ng-model="vm.dateRange"  options="vm.options" />`,
            controller: Controller,
            controllerAs: 'vm'
        });

    Controller.$inject = ['datePickerLocale'];
    function Controller(locale) {
        let vm = this;
        let prev = null;
        vm.$onInit = oninit;
        vm.$onChanges = onchanges;
        
       
        function oninit() {
            var defaultOptions = {
                locale: locale,
                opens: 'left',
                eventHandlers: {
                    'apply.daterangepicker': onSelect,
                    'outsideClick.daterangepicker': onSelect

                }
            };
            vm.options = angular.extend(defaultOptions, vm.settings || {});
            vm.dateRange = vm.dateRange ||
            {
                startDate: moment(),
                endDate: moment()

            };
            set();
        }

        function onchanges(cs) {
            if (cs.startDate || cs.endDate) {
                set();
            }
        }


        function onSelect() {
            if (vm.dateRange.startDate > vm.dateRange.endDate) {
                vm.dateRange = angular.copy(prev);
                return;
            }
            vm.onChange({
                dr: {
                    startDate: vm.dateRange.startDate.startOf('day').toDate(),
                    endDate: vm.dateRange.endDate.startOf('day').toDate()
                }
            });
        }

        function set() {
            if (!vm.startDate || !vm.endDate) {
                return;
            }
            vm.dateRange = {
                startDate: moment(vm.startDate),
                endDate: moment(vm.endDate)
            };
            prev = angular.copy(vm.dateRange);
        }


    }


})();