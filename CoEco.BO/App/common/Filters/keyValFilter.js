(function (angular) {
    "use strict";
    angular.module('coeco.common').filter('registerStatus', function () {
        return function (registerStatus) {
            var returnValue = '';

            switch (registerStatus) {
                case 'Registered':
                    returnValue = 'רשום';
                    break;
                case 'RegisterOnProbation':
                    returnValue = 'רשום על תנאי';
                    break;
                case 'Cancelled':
                    returnValue = 'רשום';
                    break;
            }
            return returnValue;
        };
    });

})(angular);