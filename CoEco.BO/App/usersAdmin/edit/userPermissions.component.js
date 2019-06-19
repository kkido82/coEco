(function (angular) {
    "use strict";

    var app = angular.module('coeco.usersAdmin');

    app.component('userPermissions', {
        bindings: {
          
            editedUser:'='
        },
        templateUrl: '/App/usersAdmin/edit/userPermissions.tmpl.html',
        controllerAs: 'vm',
        controller: Controller


    });
    Controller.$inject = ['usersService', 'common', '$scope'];
    function Controller(usersService, common, $scope) {
        var vm = this;
        var originalRoles = [];
        vm.permissions = [];
        vm.resetPermissions = resetPermissions;
        vm.togglePermissions = togglePermissions;
        vm.savePermissions = savePermissions;

        vm.$onInit = onInit;
   

       

        function onInit() {
           // originalRoles = angular.copy(vm.roles);
            common.authService.getPermissionItems().then(function(p) {
                vm.permissions = p;
            });
        }


        function resetPermissions() {
            vm.editedUser.roles = angular.copy(originalRoles);
        }



     

        function togglePermissions(key) {
            if (hasPermissions(key)) {
                var index = vm.roles.indexOf(key);
                vm.roles.splice(index, 1);
            } else {
                vm.roles.push(key);
            }
        }



        function savePermissions() {
            usersService.savePermissions(vm.userId, vm.roles).then(function (r) {
                if (r.isSuccess) {
                    common.notify.success('הרשאות עודכנו בהצלחה');
                } else {
                    common.notify.error(r.errors);
                }

            });
        }


        function hasPermissions(key) {
            if (!vm.editedUser || !vm.editedUser.roles) {
                return false;
            }
            return vm.editedUser.roles.indexOf(key) > -1;
        }

        

    }


})(angular);