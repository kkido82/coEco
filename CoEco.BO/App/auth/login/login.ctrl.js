(function (angular) {
    "use strict";
    angular.module('coeco.auth').controller('loginCtrl', Controller);

    Controller.$inject = ['authService', '$state'];
    function Controller(authService, $state) {
        var vm = this;
        vm.errors = [];

        vm.authData = {
            username: '',
            password: ''
        };

        vm.login = login;
        activate();
        function activate() {
            authService.signout();
        }

        function login() {
            var authData = angular.copy(vm.authData);
            authService.login(authData).then(function () {
                $state.go('home');
            }, onError);

        }

        function onError(result) {
            if (result.data && result.data.modelState && result.data.modelState.errors) {
                vm.errors = result.data.modelState.errors;
            } else {
                vm.errors = [result.data.message];
            }

        }

    }
})(angular);