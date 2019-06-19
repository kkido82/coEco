(function (angular) {
    "use strict";

    var app = angular.module('coeco');

    app.component('unitsDistances', {
        controller: Controller,
        controllerAs: 'vm',
        templateUrl: '/app/units/units.distance.tmpl.html',
    });

    const Unit = (id, name) => ({ id, name });
    const UnitDistance = (a, b, distance) => ({ a, b, distance });
    const getKey = (i, j) => {
        let arr = i > j ? [j, i] : [i, j];
        return arr.join("_");
    };

    const isValid = units => {
        const length = units.length;
        const numDistances = (length * length - length) / 2;
        return (inputs = {}) =>
            Object.keys(inputs).filter(key => !!inputs[key]).length ===
            numDistances;
    };
    const mapFromInput = (inputs = {}) =>
        Object.keys(inputs).map((key = "0_0") => {
            let [a, b] = key.split("_").map(Number);
            let distance = inputs[key];
            return UnitDistance(a, b, distance);
        });

    const mapToInputs = (units = [], distances = []) => {
        const ids = units.map(a => a.id);
        const contains = value => ids.findIndex(a => a === value) > -1;
        const inputs = {};
        distances
            .filter(d => contains(d.a) && contains(d.b))
            .forEach(d => {
                let key = getKey(d.a, d.b);
                inputs[key] = d.distance;
            });
        return inputs;
    };

    const save = ($http, success, fail) => inputs => {
        let data = mapFromInput(inputs);
        $http.post('/api/Coeco/DistanceUnits', data).then(() => {
            success();
        }).catch(err => {
            fail(err.data.Message);
        })
    };

    const loadUnits = (dataService) => {
        var unitsQuery = { from: 'Units', select: ['ID', 'UnitName'] };
        return dataService.list(unitsQuery, true).then(res => {
            return res.results.map(u => Unit(u.ID, u.UnitName));
        })
    };

    const loadDistances = (dataService) => {
        var query = { from: 'DistanceUnits', select: ['FirstUnitID', 'SecondUnitID', 'Distance'] };
        return dataService.list(query, true).then(res => {
            return res.results.map(u => UnitDistance(u.FirstUnitID, u.SecondUnitID, u.Distance));
        });
    }

    Controller.$inject = ['coecoDataService', '$q', '$http', 'common'];
    function Controller(coecoDataService, $q, $http, common) {
        console.log(common)
        var vm = this;
        vm.$onInit = function () {
            $q.all([loadUnits(coecoDataService), loadDistances(coecoDataService)]).then(res => {
                const [units, distances] = res;
                vm.units = units;
                vm.inputs = mapToInputs(units, distances);
                vm.isValid = isValid(units);
                console.log(vm.imputs)
            })
        };
        vm.getKey = getKey;
        vm.save = save($http, onSaveSuccess, onSaveError);
        vm.isValid = () => false;
        return vm;

        function onSaveSuccess() {
            common.notify.success('המרחקים נשמרו בהצלחה')
        }

        function onSaveError(err) {
            common.notify.error('העדכון נכשל: ' + err);
        }
    }
})(angular);