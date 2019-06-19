(function (angular) {
    "use strict";

    var app = angular.module('coeco.nav');

    app.component('coecoBreadcrumbs', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/nav/breadcrumbs/breadcrumbs.tmpl.html'
    });

    Controller.$inject = ['$rootScope', 'navService'];
    function Controller($rootScope, navService) {
        var vm = this;
        vm.$onInit = oninit;
        function oninit() {
            vm.bc = navService.getBreadcrumbs();
            $rootScope.$on('$stateChangeSuccess', init);
        }


        function init() {
            vm.bc = navService.getBreadcrumbs();
        }


    }


})(angular);
