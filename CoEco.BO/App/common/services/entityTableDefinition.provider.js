(function (angular) {
    "use strict";

    angular.module('coeco.common').provider('entityTableDefinitions', Provider);

    function Provider() {
        var definitions = [];

        this.registerDefinition = function (name, options) {
            definitions[name.toLowerCase()] = options;
            return this;
        };

        this.$get = function () {
            return definitions;
        };


    }
})(angular);