(function (window, angular) {
    'use strict';

    angular.module('coeco.common', ['ngAnimate', 'toastr', 'breeze.angular', 'daterangepicker'])
        .value('coecoMetaContext', window.coecoMetaContext)
        .constant('commonConfig', commonConfig()).constant('pluralize',
        function (et) {
            return et.toLowerCase().endsWith('y') ? et.slice(0, -1) + 'ies' : (et.toLowerCase().endsWith("status") ? et
                    : (et.toLowerCase() === 'bankbranch' ? 'BankBranches' : (et === 'Months' ? et : et + 's')));
        }).constant('CoecoUploadFileType', {
            UploadFileType: {
                CoecoMembers: 'CoecoMembers'
            }
        });

    function commonConfig() {
        return {
            defaultPagingContext: {
                totalRecords: 0,
                itemsPerPage: 20,
                pageIndex: 0,
                pagingOptions: [1, 2, 10, 20, 50, 100]
            }
        };
    }


})(window, angular);
