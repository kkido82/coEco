(function () {
    'use strict';

    angular
        .module('coeco.ui')
        .directive('coecoTableHeader', coecoTableHeader);

    coecoTableHeader.$inject = [];

    function coecoTableHeader() {
        // Usage:
        //     <directive1></directive1>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                entityTypeDefintions: '='
            }
        };
        return directive;

        function link(scope, element, attrs) {
            if (!scope.entityTypeDefintions || !scope.entityTypeDefintions.fields || !angular.isArray(scope.entityTypeDefintions.fields)) {
                return;
            }

            angular.forEach(scope.entityTypeDefintions.fields, function (field) {
                element.append('<th>' + field.title + '</th>');
            });
            element.append('<th>נוצר על ידי</th><th>נוצר ב</th><th>עודכן על ידי</th><th>עודכן ב</th><th></th>');

        }
    }

   

})();