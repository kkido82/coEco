
(function (angular) {
    angular.module('coeco').component('membersList', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/members/members.list.tmpl.html'
    });

    Controller.$inject = ['coecoControllerInitializer', 'coecoDataService', '$state', '$q', '$http', '$uibModal'];
    function Controller(coecoControllerInitializer, coecoDataService, $state, $q, $http, modal) {
        var vm = this;
        vm.permissionName = 'members';
        vm.entityType = 'Members';
        vm.expand = ['PermissionsProfile', 'Unit'];
        vm.resetSearchOverride = resetSearchOverride;
        vm.searchOverride = searchOverride;
        vm.$onInit = onInit; 
        vm.remove = remove;
        vm.orderBy = 'ID';
        vm.importMembers = importMembers;


        function onInit() {
            var a = coecoControllerInitializer.initListController(vm);
        }

        function resetSearchOverride() {
            vm.fromDate = vm.toDate = null;
            vm.resetSearch();
        }

        function searchOverride() {
            vm.search(true);
        }

        function importMembers() {
            openForm().then(commit, revert)
                              .catch(console.log);
        }

        function openForm() {
            return modal.open({
                bindToController: true,
                controllerAs: 'vm',
                templateUrl: '/app/members/importMembers/importMembers.edit.import.tmpl.html',
                controller: 'MembersUploadCtrl'
            }).result;
        }

        function commit(entity) {
            $state.reload();
        }

        function revert(entity) {
        }
        function setDateFilter() {
            if (vm.fromDate || vm.toDate) {
                vm.filters = vm.filters || {};

                if (vm.fromDate) {
                    vm.filters.DATE_HIT = vm.filters.DATE_HIT || {};
                    vm.filters.DATE_HIT.ge = new Date(vm.fromDate);
                }
                if (vm.toDate) {
                    vm.filters.DATE_HIT = vm.filters.DATE_HIT || {};
                    vm.filters.DATE_HIT.le = new Date(vm.toDate);
                }
            }
        }
        function remove(entity) {
            coecoDataService.remove(entity);
            $state.reload();
        }


        function buildQuery() {
            let queryObj = {
                from: vm.entityType,
                expand: vm.expand || [],
                inlineCount: true,
                where: undefined,
                orderBy: ['ID'],
                skip: 0,
                take: 10000000
            };
            return queryObj;
        }
     

    }
})(angular);
