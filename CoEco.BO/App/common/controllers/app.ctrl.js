(function (angular) {
    var app = angular.module('coeco');

    app.controller('appCtrl', Controller);

    Controller.$inject = ['authService'];
    function Controller(authService) {
        var vm = this;

        vm.authStatus = authService.authentication;
    }

})(angular);