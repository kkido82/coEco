
(function (angular) {
    angular.module('coeco').component('unitsList', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/units/units.list.tmpl.html'
    });

    Controller.$inject = ['coecoControllerInitializer', 'coecoDataService', '$state', '$q' , '$http'];
    function Controller(coecoControllerInitializer, coecoDataService, $state, $q, $http) {
        var vm = this;
        vm.permissionName = 'units';
        vm.entityType = 'Units';
        //vm.expand = ['IconStore'];
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

        //function getDropDownVals() {
        //    vm.vehicleCodes = [];
        //    vm.sectionCodes = [];
        //    vm.supplierCodes = [];
        //    vm.statuses = [];
        //    coecoDataService.list(buildQuery(), true).then(function (data) {
        //        var allWorkOrders = data.results;
        //        angular.forEach(allWorkOrders, function (w_value, w_key) {
        //            generateDropDown('KOD_RECHEV', vm.vehicleCodes, w_value, w_key);
        //            generateDropDown('NAME_MADOR', vm.sectionCodes, w_value, w_key);
        //            generateDropDown('NAME_SAPAK', vm.supplierCodes, w_value, w_key);
        //            generateDropDown('NAME_STATUS_SHEDER', vm.statuses, w_value, w_key);
        //        });
        //    });
        //}

        function generateDropDown(val_name, arr, w_value, w_key) {
            var v_keepGoing = true;
            if (arr.length == 0) {
                arr.push({ key: w_key, value: w_value[val_name] })
            }
            else {
                angular.forEach(arr, function (v_value, v_key) {
                    if (v_keepGoing) {
                        if (w_value[val_name] == v_value.value) {
                            v_keepGoing = false;
                        }
                        else if (v_key == arr.length - 1) {
                            arr.push({ key: w_key, value: w_value[val_name] })
                        }
                    }

                });
            }
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



        //function exportToPDF(workOrder) {
        //    var base64ToBlobHelper = hovalaDataService.base64ToBlob;
        //    //this.excelService.exportAsExcelFile([workOrder],workOrder.NUM_HZM);    


        //        $http({
        //            url: '/breeze/hovala/ExportToPDF',
        //            data: {
        //                NUM_TM: workOrder.ID
        //            },
        //            method: "POST",
        //            contentType: "application/json",
        //        }).error(function (response) {
        //        }).success(function (response) {
        //            if (response == false) {
        //                common.notify.error('שליחת התראה נכשלה');
        //            }
        //            else {
        //                var i_objectToSave = JSON.parse(response);
        //                //if (i_objectToSave.failed)
        //                //    this.router.navigate(['/loginPage']);
        //                i_objectToSave = i_objectToSave.results[0];

        //                var _a = document.createElement('A');
        //                var _evt = document.createEvent('MouseEvents');
        //                var _blob = base64ToBlobHelper(i_objectToSave.data, "application/pdf", 512);

        //                _evt.initEvent('click', true, false);
        //                var date = new Date();
        //                var fileName = workOrder.NUM_HZM + '_' + date + '.pdf';//+ this.datepipe.transform(new Date(), 'dd.MM.yy: hh:mm:ss') + '.pdf';
        //                _a.setAttribute("download", fileName);
        //                _a.setAttribute("href", _blob.url);
        //                _a.dispatchEvent(_evt);
        //                common.notify.success("התראה נשלחה בהצלחה");
        //            }
        //        });

        //}

    }
})(angular);
