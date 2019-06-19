(function () {
    'use strict';

    var app = angular.module('coeco.usersAdmin');

    app.controller('usersAdminListCtrl', Controller);

    Controller.$inject = ['coecoControllerInitializer', 'usersService', '$window', '$http', 'common', '$state'];
    function Controller(coecoControllerInitializer, usersService, $window, $http, common, $state) {

        var vm = this;
        vm.permissionName = 'usersAdmin';
        //vm.entityType = 'AspNetUsers';

        vm.items = [];
        vm.pagingContext = {
            totalRecords: 0,
            itemsPerPage: 20,
            pageIndex: 0,
            pagingOptions: [1, 2, 10, 20, 50, 100]
        };
        vm.filterContext = {

        };
        vm.$onInit = onInit;

        vm.fetchData = fetchData;
        vm.search = search;
        vm.setPageSize = setPageSize;
        vm.getNumPages = getNumPages;
        vm.setPage = setPage;
        vm.resetSearch = resetSearch;
        vm.exportToExcel = exportToExcel;
        vm.remove = remove;
        activate();
        vm.filterContext = {

        };

        function onInit() {
            coecoControllerInitializer.initPermissionsOnly(vm);
        //    coecoControllerInitializer.initListController(vm);
        }

        function activate() {
            fetchData();
        }

        function exportToExcel() {
          usersService.listUsersExport();
        }

        function fetchData() {
            var filterData = angular.copy(vm.filterContext);

            filterData.skip = vm.pagingContext.pageIndex * vm.pagingContext.itemsPerPage;
            filterData.take = vm.pagingContext.itemsPerPage;

            usersService.listUsers(filterData).then(function(response) {
                vm.items = response.data.Users;
                vm.pagingContext.totalRecords = response.data.NumResults;

            });
        }

        function search() {
            vm.pagingContext.pageIndex = 0;
            fetchData();
        }

        function setPageSize(pageSize) {
            vm.pagingContext.itemsPerPage = pageSize;
            fetchData();
        }

        function getNumPages() {
            return parseInt(vm.pagingContext.totalRecords / vm.pagingContext.itemsPerPage) + (vm.pagingContext.totalRecords % vm.pagingContext.itemsPerPage ? 1 : 0);
        }

        function setPage(pageIndex) {
            if (vm.pagingContext.pageIndex === pageIndex) {
                return;
            }
            vm.pagingContext.pageIndex = pageIndex;
            fetchData();
        }

        function resetSearch() {
            vm.filterContext = {};
            vm.pagingContext.pageIndex = 0;
            fetchData();
        }

        function remove(user) {
            $http({
                url: '/breeze/coeco/RemoveAspNetUser',
                data: { userId: user.id },
                method: "POST",
                contentType: "application/json",
            }).error(function () {
                common.notify.error('מחיקת המשתמש נכשלה');
            }).success(function () {
                common.notify.success("המשתמש נמחק בהצלחה");
                var indexOfItemToRemove = vm.items.indexOf(user);
                vm.items.splice(indexOfItemToRemove);
            });
        }

    }


})();