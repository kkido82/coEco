(function (angular) {
    "use strict";
    angular.module('coeco.common').factory('cachedDataService', factory);

    var cache = {};

    factory.$inject = ['dataService', '$q'];
    function factory(dataService, $q) {

        return {
            getList(tblName) {
                if (!cache[tblName]) {

                    return dataService.list(tblName).then(res=> {
                        cache[tblName] = res.results;
                        return res.results;
                    });
                } else {
                    return $q.when(cache[tblName]);
                }
            }

        };
    }
})(angular);

