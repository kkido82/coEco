(function () {
    'use strict';

    angular
        .module('coeco.usersAdmin')
    .directive('compareTo', compareTo);

    function compareTo() {
        return {
            require: "ngModel",
            scope: {
                otherModelValue: "=compareTo"
            },
            link: function(scope, element, attributes, ngModel) {

                ngModel.$validators.compareTo = function(modelValue) {

                    if ((modelValue === undefined && scope.otherModelValue === "") ||
                    (modelValue === "" && scope.otherModelValue === undefined)) {
                        return true;
                    }
                    return modelValue === scope.otherModelValue;

                };

                scope.$watch("otherModelValue",
                    function() {
                        ngModel.$validate();
                    });
            }
        };
    }

})();