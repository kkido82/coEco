(function () {
    'use strict';

    angular.module('coeco.ui').directive('validateVacationCycle', validateVacationCycle);

    validateVacationCycle.$inject = [];

    function validateVacationCycle() {

        return {
            require: 'ngModel',
            link: function (scope, element, attr, mCtrl) {
                function myValidation(value) {
                    if (value > 0) {
                        mCtrl.$setValidity('charE', true);
                    } else {
                        mCtrl.$setValidity('charE', false);
                    }
                    return value;
                }
                mCtrl.$parsers.push(myValidation);
            }
        };
}

})();