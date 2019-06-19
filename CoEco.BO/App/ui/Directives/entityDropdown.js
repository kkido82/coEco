(function () {
    'use strict';

    angular
        .module('coeco.ui')
        .directive('entityDropdown', entityDropdown);

    entityDropdown.$inject = ['$filter', '$injector'];

    function entityDropdown($filter, $injector) {
        var directive = {
            link: link,
            restrict: 'A',
            require: 'ngModel'

        };
        return directive;

        function link(scope, element, attrs, ngmodel) {
            var entityType = attrs.entityDropdown;
            var dataService = $injector.get(attrs.serviceName);
            var valueField = attrs.valueField || 'ID';
            var textField = attrs.textField || 'Name';
            var orderBy = attrs.orderby || 'UpdatedOn';
            var field = attrs.ngModel;
            
            dataService.list(entityType, orderBy).then(initList);

            scope.$watch(field, function (v) {
                element.val(v);
            });

            function initList(data) {
                $(element).empty();
                
                var $option = $('<option>בחר</option>');
                element.append($option);
                angular.forEach(data.results, function (r) {
                    element.append('<option value="' + r[valueField] + '">' + r[textField] + '</option>');
                });
               

            }
        }
    }

})();