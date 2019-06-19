
(function (angular) {
    angular.module('coeco').component('orderDataReportList', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/orderDataReport/orderDataReport.list.tmpl.html'
    });

    Controller.$inject = ['coecoControllerInitializer', 'coecoDataService', '$state', '$q' , '$http'];
    function Controller(coecoControllerInitializer, coecoDataService, $state, $q, $http) {
        var vm = this;
        vm.permissionName = 'orderDataReport';
        vm.entityType = 'OrderDataReports';       
        vm.resetSearchOverride = resetSearchOverride;
        vm.searchOverride = searchOverride;
        vm.$onInit = onInit;        
        vm.orderBy = 'CreatedOn';

      
        function onInit() {
            var a = coecoControllerInitializer.initListController(vm);     
        }

        function resetSearchOverride() {
            vm.fromDate = vm.toDate = null;
            vm.resetSearch();
        }

        function searchOverride() {
            setDateFilter();
            vm.search(true);
        }
        function setDateFilter() {
            if (vm.fromDate || vm.toDate) {
                vm.filters = vm.filters || {};

                if (vm.fromDate) {
                    vm.filters.OrderDate = vm.filters.OrderDate || {};
                    vm.filters.OrderDate.ge = new Date(vm.fromDate);
                }
                if (vm.toDate) {
                    vm.filters.OrderDate = vm.filters.OrderDate || {};
                    vm.filters.OrderDate.le = new Date(vm.toDate);
                }
            }
        }

    }
})(angular);
