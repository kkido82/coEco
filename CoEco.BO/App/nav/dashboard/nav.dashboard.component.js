(function (angular) {
    angular.module('coeco.nav').component('navDashboard', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/nav/dashboard/nav.dashboard.tmpl.html'

    });

    Controller.$inject = ['navService', 'authService'];
    function Controller(navService, authService) {
        var vm = this;
        vm.$onInit = oninit;
        vm.items = [];


        function oninit() {
            authService.privileges().then(function (record) {
                vm.permissions = record;
                vm.items = navService.getNavModel();
            });

        }

    }


})(angular);