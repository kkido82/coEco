(function () {
    'use strict';

    angular
        .module('coeco.ui')
        .directive('entitiesDropdown', EntitiesDropdown);

    EntitiesDropdown.$inject = ['dataService'];

    function EntitiesDropdown(dataService) {
        // Usage:
        //     <entities-dropdown></entities-dropdown>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A',
            require: 'ngModel'


        };
        return directive;

        function link(scope, element, attrs, ngmodel) {
            var entityType = attrs.entitiesDropdown;
            var valueField = attrs.valueField || 'ID';
            var textField = attrs.textField || 'Name';
            var field = attrs.ngModel;

            dataService.list(entityType).then(initList);

            scope.$watch(field, function (v) {
                element.val(v);
            });

            function initList(data) {
                element.empty();
                var $option = $('<option value="0">בחר</option>');
                element.append($option);
                var value = ngmodel.$modelValue || '';
                angular.forEach(data.results, function (r) {
                    var selected = r[valueField].toString() === value.toString() ? 'selected="selected"' : '';
                    element.append('<option ' + selected + ' value="' + r[valueField] + '">' + r[textField] + '</option>');
                });
            }
        }
    }

})();