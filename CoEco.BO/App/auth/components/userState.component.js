(function (angular) {
    'use-strict';
    angular.module('coeco.auth').component('userState',
    {
        controllerAs: 'vm',
        templateUrl: '/app/auth/components/userState.tmpl.html',
        controller: Controller

    });
    Controller.$inject = ['authService', '$state', '$rootScope'];
    function Controller(authService, $state, $rootScope) {
        var vm = this;
        vm.$onInit = oninit;
        vm.signout = signout;


        function oninit() {
            vm.authStatus = authService.state;
            $rootScope.$on('mft:authSet', () => vm.authStatus = authService.state);
        }

        function signout() {
            authService.signout().then(() => $state.go('login'));
        }
    }

})(angular);