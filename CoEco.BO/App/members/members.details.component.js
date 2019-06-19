(function (angular) {
    "use strict";

    var app = angular.module('coeco');

    app.component('membersDetails', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/members/members.details.tmpl.html',
    });

        Controller.$inject =['$stateParams', '$state', 'coecoControllerInitializer', 'pickers', '$q', 'common', 'coecoDataService'];
    function Controller($stateParams, $state, coecoControllerInitializer, pickers, $q, common, coecoDataService) {

        var vm = this;
        vm.permissionName = 'members';
        vm.entityType = 'Member';
        vm.expand = ['PermissionsProfile', 'Unit'];
        vm.id = $stateParams.id;
        vm.$onInit = onInit;
        vm.isNewMember = false;

        function onInit() {
            coecoControllerInitializer.initDetailsController(vm);
            if (vm.id === "0") {
                vm.isNewMember = true;
            }
        }

        function setTimeFormat(timeToFormat) {
            var hh ="";
            var mm = "";
            var j;
            var begin = false;
            for (var i = 0 ; i < timeToFormat.length; i++) {               
                if (timeToFormat[i] == 0 || parseInt(timeToFormat[i])) {
                    begin = true;
                    hh += timeToFormat[i];
                }
                else if (begin == true) {
                    j = i;
                    break;
                }
            }
            begin = false;
            for (var i = j ; i < timeToFormat.length; i++) {
                if (timeToFormat[i]==0 || parseInt(timeToFormat[i])) {
                    begin = true;
                    mm += timeToFormat[i];
                }
                else if (begin == true) {
                    break;
                }
            }
            if (hh.length < 2)
                hh = '0' + hh;
            if (mm.length < 1)
                mm = '00';
            if (mm.length < 2)
                mm = '0' + mm;
            var retVal = hh + ':' + mm;
            return retVal;
        }
    }
})(angular);