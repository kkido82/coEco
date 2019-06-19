
(function (angular) {
    angular.module('coeco').component('permissionsProfilesList', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/permissionsProfiles/permissionsProfiles.list.tmpl.html'
    });

    Controller.$inject = ['coecoControllerInitializer', 'coecoDataService', '$state', '$q' , '$http'];
    function Controller(coecoControllerInitializer, coecoDataService, $state, $q, $http) {
        var vm = this;
        vm.permissionName = 'permissionsProfiles';
        vm.entityType = 'PermissionsProfiles';
        vm.resetSearchOverride = resetSearchOverride;
        vm.searchOverride = searchOverride;
        vm.$onInit = onInit; 
        vm.remove = remove;
        vm.orderBy = 'ID';


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
