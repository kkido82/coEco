(function (angular) {
    "use strict";

    var app = angular.module('coeco.nav');

    app.component('coecoMenu', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/nav/menu/menu.tmpl.html'
    });

    Controller.$inject = ['authService', '$rootScope', 'navService', '$state'];
    function Controller(authService, $rootScope, navService, $state) {
        var vm = this;
        vm.$onInit = oninit;
        vm.toggleSubMenu = toggleSubMenu;
        vm.signout = signout;


        function oninit() {
            createNav();
            $rootScope.$on('mft:authSet', createNav);
        }
        function createNav() {
            authService.privileges().then(setPermissions);


        }

        function signout() {
            authService.signout().then(r => $state.go('login'));
        }

        function setPermissions(record) {
            vm.permissions = record;
            for (var key in vm.permissions)
            {
                if (key.includes('Category') || key.includes('Status') || key.includes('userAdmin') || key.includes('message'))
                    delete vm.permissions[key];
            }
            var nav = navService.getNavModel();
            vm.nav = nav;
        }

        function toggleSubMenu() {

            $rootScope.submenuOpen = !$rootScope.submenuOpen;


        }
    }


})(angular);