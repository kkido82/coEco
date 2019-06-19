(function (angular) {
    "use strict";
    angular.module('coeco.common').factory('entityController', factory);

    factory.$inject = ['dataService', 'commonConfig', 'pluralize'];

    function factory(dataService, commonConfig, pluralize) {

        return function (vm, entityTypeSingle, entityTypePlural, entityDefinitions) {
            entityTypePlural = entityTypePlural || pluralize(entityTypeSingle);
            vm.entityDefitions = entityDefinitions;
            vm.entityTypes = entityTypePlural;
            vm.entityType = entityTypeSingle;
            vm.items = [];
            vm.errors = [];
            vm.newItem = {};
            vm.pagingContext = angular.copy(commonConfig.defaultPagingContext);

            vm.getPager = getPagerArray;
            vm.setPage = setPage;
            vm.requery = requery;
            vm.toggleDisabled = toggleDisable;
            vm.isReady = isReady;
            vm.activate = activate;
            vm.export = exportUrl;
            vm.setPageSize = setPageSize;
            vm.getNumPages = getNumPages;
            function activate() {
                requery();
            }


            function requery(resetPageIndex) {
                if (resetPageIndex) {
                    vm.pagingContext.pageIndex = 0;
                }
                return dataService.list(vm.entityTypes, 'CreatedOn', vm.pagingContext.pageIndex, vm.pagingContext.itemsPerPage).then(function (s) {
                    vm.items = s.results;
                    vm.pagingContext.totalRecords = s.inlineCount;
                });
            }

            function exportUrl() {
                var url = dataService.url(vm.entityTypes, 'CreatedOn', vm.pagingContext.pageIndex, vm.pagingContext.itemsPerPage);
                return url;
            }

            function isReady() {
                return dataService.isReady;
            }

            function toggleDisable(entity) {
                entity.Disabled = !entity.Disabled;
                dataService.update(entity);
            }


            function getPagerArray() {
                if (vm.pagingContext.totalRecords <= vm.pagingContext.itemsPerPage) {
                    return [];
                }

                var num = parseInt(vm.pagingContext.totalRecords / vm.pagingContext.itemsPerPage) + (vm.pagingContext.totalRecords % vm.pagingContext.itemsPerPage ? 1 : 0);
                return new Array(num);
            }

            function setPage(pageIndex) {
                if (vm.pagingContext.pageIndex === pageIndex) {
                    return;
                }
                vm.pagingContext.pageIndex = pageIndex;
                activate();
            }

            function setPageSize(pageSize) {
                vm.pagingContext.itemsPerPage = pageSize;
                vm.requery(true);
            }

            function getNumPages() {
                return parseInt(vm.pagingContext.totalRecords / vm.pagingContext.itemsPerPage) + (vm.pagingContext.totalRecords % vm.pagingContext.itemsPerPage ? 1 : 0);
            }
        };

    }


})(angular);