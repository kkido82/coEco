(function () {
    'use strict';

    angular.module('coeco.ui').directive('beautifyXml', beautifyXml);

    beautifyXml.$inject = [];

    function beautifyXml() {

        return {
            link: link,
            restrict: 'A',
            replace: true,
            require: 'ngModel',
            scope: {
                ngModel: '=',
            }
        };

        function link(scope, element, attrs) {

            return element.text(vkbeautify.xml(scope.ngModel, 50));
        }
    }

})();