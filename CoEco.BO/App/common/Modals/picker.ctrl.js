(function () {
    'use strict';

    angular
        .module('coeco.common')
        .controller('picker', Picker);

    Picker.$inject = ['settings', 'serviceName', '$uibModalInstance', '$q', '$injector'];

    function Picker(settings, serviceName, $uibModalInstance, $q, $injector) {
        /* jshint validthis:true */
        var vm = this;
        var entityTypeObj;
        var dataService = $injector.get(serviceName);
        activate();

        function activate() {
            search();
            vm.settings = settings;
            vm.ok = ok;
            vm.cancel = cancel;
            vm.search = search;
            vm.keydown = keydown;
            vm.isPicked = isPicked;
            vm.toggle = toggle;
            vm.excludes = vm.settings.excludes || [];
            vm.pickedItems = [];
            vm.inExcludes = inExcludes;

        }
       

        function search() {
            ensureEntityType().then(() => {
                let query = {
                    from: settings.entityType,
                  //  where: angular.copy(settings.filter),
                    take: settings.take || 25
                };
                if (vm.searchTerm && settings.fields && settings.fields.length) {
                    let searchPredicates = settings.fields.filter(filterStringProperties).map(jsonMapper);
                    if (!isNaN(vm.searchTerm)) {
                        searchPredicates.push({ ID: parseInt(vm.searchTerm) });
                    }
                    query.where = { and: [query.where, { or: searchPredicates }] };
                }

                dataService.list(query).then(data => {
                    vm.items = data.results;
                    var stop = 0;
                }, function (err) {
                });
            });
        }

        function isPicked(item) {
            return vm.pickedItems.indexOf(item) > -1 || inExcludes(item);
        }


        function toggle(item) {
            let index = vm.pickedItems.indexOf(item);
            if (index < 0) {
                vm.pickedItems.push(item);
            } else {
                vm.pickedItems.splice(index, 1);
            }
        }

        function keydown(event) {
            if (event.keyCode === 13) {
                vm.search();
            }
        }

        function ok(item) {
            if (item) {
                $uibModalInstance.close(item);
            } else {
                $uibModalInstance.close(vm.pickedItems);
            }

        }

        function pickableItems() {
            if (vm.excludes.length === 0) {
                return vm.items;
            }
            return vm.items.filter(i=> !inExcludes(i));
        }

        function inExcludes(item) {
            return vm.excludes.indexOf(item.ID) >= 0;
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function ensureEntityType() {
            if (entityTypeObj) {
                return $q.when(entityTypeObj);
            }

            return dataService.getEntityType(settings.entityType).then(et => {
                entityTypeObj = et;
                return et;
            });
        }

        function jsonMapper(s) {
            console.log(entityTypeObj);
            return JSON.parse(`{"${s}":{"contains":"${vm.searchTerm}"}}`);
        }

        function filterStringProperties(p) {
            let prop = entityTypeObj.dataProperties.find(dt => dt.name === p);
            return prop && prop.dataType === breeze.DataType.String;
        }

    }
})();
