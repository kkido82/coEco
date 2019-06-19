(function (angular) {
    "use strict";

    var app = angular.module('coeco.nav');
    app.component('coecoHeader', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/nav/header/header.tmpl.html'
    });

    Controller.$inject = ['$rootScope', '$state', '$stateParams', 'coecoValuesConfig'];
    function Controller($rootScope, $state, $stateParams, coecoValuesConfig) {
        var vm = this;
        vm.$onInit = oninit;
        function oninit() {
            vm.state = $state.current;
            $rootScope.$on('$stateChangeSuccess', init);
        }


        function init(e, toState) {

            vm.state = toState;

            vm.title = '';
            if ($stateParams.entityType && typeof $stateParams.entityType != 'undefined') {
                let item = coecoValuesConfig.valueEntityTypes.find(function (f) {
                    return f.name === $stateParams.entityType;
                });

                vm.title = item ? item.title : '';

            }
        }
    }


})(angular);
