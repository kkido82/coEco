(function (angular) {
    "use strict";
    angular.module('coeco.common').factory('common', factory);

    factory.$inject = ['$q', 'authService', 'toastr', 'confirm'];
    function factory($q, authService, toastr, confirm) {

        return {
            activateController: activateController,
            authService: authService,
            $q: $q,
            notify: toastr,
            confirm: confirm,
            clean: clean,
            daysOfWeekArr: ['לילות', 'א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ש'],
            daysOfWeek: { 1: 'א', 2: 'ב', 3: 'ג', 4: 'ד', 5: 'ה', 6: 'ו', 7: 'ש' },
            cleanArray: cleanArray
        };

        function activateController(promises) {
            return $q.all(promises);
        }
        function cleanArray(actual) {
            var newArray = new []();
            for (var i = 0; i < actual.length; i++) {
                if (actual[i]) {
                    newArray.push(actual[i]);
                }
            }
            return newArray;
        }
        function clean(obj) {
            for (var propName in obj) {
                //if object at property is null or empty then we delete the property
                if (obj[propName] === null || obj[propName] === undefined || obj[propName] === '') {
                    delete obj[propName];
                } else {
                    for (var innerPropName in obj[propName]) {
                        //if object at inner property is null or empty then we delete the property (every property can have a single operator)
                        if (obj[propName][innerPropName] === null || obj[propName][innerPropName] === undefined || obj[propName][innerPropName] === '') {
                            delete obj[propName];
                        }
                    }
                }
            }
            //if object is empty then we make it null
            return isEmpty(obj) ? null : obj;
        }
        function isEmpty(obj) {
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop))
                    return false;
            }

            return true;
        }


    }
})(angular);