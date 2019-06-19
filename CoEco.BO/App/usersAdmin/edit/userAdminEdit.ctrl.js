(function () {
    'use strict';

    var app = angular.module('coeco.usersAdmin');

    app.controller('usersAdminEditCtrl', Controller);

    Controller.$inject = ['usersService', '$stateParams', '$state', 'coecoMetaContext', 'common'];
    function Controller(usersService, $stateParams, $state, coecoMetaContext, common) {
        var originalRoles = [];
        var originalData = {};
        var vm = this;
        vm.editedUser = {};
   
        vm.resetPassword = resetPassword;
        vm.save = save;
        vm.reset = reset;
        vm.resetPermissions = resetPermissions;
      
        vm.isNew = false;
        vm.errors = [];
        vm.savePermissions = savePermissions;

        activate();


        function activate() {
            if ($stateParams.id) {
                var promises = [usersService.getById($stateParams.id), common.authService.getPermissionItems()];

                common.activateController(promises).then(function (results) {
                    var data = results[0];
                    var permissions = results[1];
                    originalData = angular.copy(data);
                    originalRoles = angular.copy(data.Roles);
                    vm.roles = data.Roles;
                    vm.permissions = permissions;
                    reset();
                    resetPermissions();

                });

            } else {
                vm.isNew = true;
            }

        }

        function resetPassword(newPassword) {
           return usersService.changePassword(vm.editedUser.Id, newPassword).then(function (r) {
                common.notify.success('הסיסמה שונתה בהצלחה');
                return r;
            },onerror);
        }

        function save() {
            usersService.save(vm.editedUser).then(function (result) {
                if (result.IsSuccess) {
                    common.notify.success('הפריט נשמר בהצלחה');
                    $state.go('usersAdmin');
                } else {
                    vm.errors = result.errors;
                }
            });
        }

        function reset() {
            // maybe this function responsible for not cleaning SSN and Email
            vm.editedUser = angular.copy(originalData);
        }

        function resetPermissions() {
            vm.roles = angular.copy(originalRoles);
        }

       

      

       

        function savePermissions() {
            usersService.savePermissions(vm.editedUser.Id, vm.roles).then(function(r) {
                if (r.IsSuccess) {
                    common.notify.success('הרשאות עודכנו בהצלחה');
                } else {
                    common.notify.error(r.errors);
                }

            });
        }


        function onerror(err) {
           if (!err || !err.data) {
               return;
           }
            if (err.data.modelState) {
                common.notify.error(err.data.modelState.errors);
            } else {
                common.notify.error(err.data.message);
            }
        }


    }


})();