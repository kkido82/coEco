(function(angular) {
    "use strict";

    angular.module('coeco.values').component('valuesNavbar', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: "/app/values/nav/values.navbar.tmpl.html"
    });

    Controller.$inject = ['coecoValuesConfig', '$stateParams', '$rootScope'];

    function Controller(config, $stateParams, $rootScope) {
        var vm = this;
        vm.valueEntityTypes = config.valueEntityTypes;
        vm.navState = vm.valueEntityTypes[0];

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams) {
            vm.navState = config.valueEntityTypes.filter(function (et) { return et.name.toLowerCase() === (toParams.entityType || 'city').toLowerCase(); })[0];
        });

    }

})(angular);