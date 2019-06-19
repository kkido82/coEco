(function (angular) {
    "use strict";

    angular.module('coeco.ui').component('itemsPager', {
        controller: Controller,
        controllerAs: "vm",
        templateUrl: '/app/ui/components/itemsPager.component.html',
        bindings: {
            onPaging: '&',
            onPageSizeChange: '&',
            numPages: '<',
            pageSize: '<',
            pageIndex: '<',
            pagingOptions:'<'

          
         
        },
    });

    Controller.$inject = [];



    function Controller() {
        var vm = this;
        vm.setPage = setPage;
        vm.setPageSize = setPageSize;
        vm.pagerArray = [];
        


        vm.$onInit = oninit;
        vm.$onChanges = onChanges;


        function oninit() {
            initPagerArray();
        }

        function setPage(page) {
            if (page >= 0 && page <= vm.numPages - 1) {
                vm.onPaging({ page: page });
                initPagerArray();
             
            }
           
        }

        function setPageSize(pageSize) {
          
            vm.onPageSizeChange({ pageSize: pageSize });
            initPagerArray();
        }
        function onChanges(changes) {
            if (changes.numPages) {
                initPagerArray();
            }
        }
        function initPagerArray() {
            if (vm.pageSize <= 1) {
                vm.pagerArray = [];
            }

           
            vm.pagerArray = new Array(vm.numPages);

 
        }

       






    }


})(angular);