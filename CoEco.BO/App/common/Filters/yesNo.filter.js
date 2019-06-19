(function (angular) {
    "use strict";
    angular.module('coeco.common').filter('yesNo', function () {
        return function (yes) {
            return yes ? 'כן' : 'לא';
        };

    });

})(angular);