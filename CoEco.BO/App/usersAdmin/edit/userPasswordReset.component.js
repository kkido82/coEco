(function (angular) {
    "use strict";

    var app = angular.module('coeco.usersAdmin');

    app.component('userPasswordReset', {
        bindings: {

            onreset: '&'
        },
        templateUrl: '/App/usersAdmin/edit/userPasswordReset.tmpl.html',
        controllerAs: 'vm',
        controller: Controller


    });
    Controller.$inject = ['coecoMetaContext'];
    function Controller(coecoMetaContext) {
        var vm = this;
        vm.passwordResetContext = {};

        vm.resetPassword = resetPassword;


        function resetPassword() {

            vm.onreset({ value: vm.passwordResetContext.newPassword }).then(function () {
                vm.passwordResetContext.newPassword = '';
                vm.passwordResetContext.confirmNewPassword = '';
            });
        }

    }


})(angular);