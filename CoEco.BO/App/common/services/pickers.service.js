(function () {
    'use strict';

    angular
        .module('coeco.common')
        .factory('pickers', Pickers);


    Pickers.$inject = ['$uibModal', '$q', '$injector'];

    function Pickers(modal, $q, $injector) {
        var service = {
            picker: picker,
            multipickAdd: multipickAdd,
        };

        return service;

        var dataService;
        function picker(serviceName, entityTypeOrSettings, fields, enableMultipicks, filter, excludes) {
            dataService = $injector.get(serviceName);
            if (angular.isObject(entityTypeOrSettings)) {
                return pick(entityTypeOrSettings, serviceName);
            }


            let settings = {
                entityType: entityTypeOrSettings,
                enableMultipicks,
                filter,
                fields,
                excludes,
                take: filter.take

            };
            return pick(settings, serviceName);
        }


        function pick(settings, serviceName) {
            return modal.open({
                bindToController: true,
                controllerAs: 'vm',
                templateUrl: '/app/common/modals/picker.html',
                controller: 'picker',
                resolve: {
                    settings: () => settings,
                    serviceName: () => serviceName
                }
            }).result;
        }

        function multipickAdd(modalSettings, entity, navItemType, navitemsPropname, leftIdPropertyName, rightIdPropertyName) {
            modalSettings.enableMultipicks = true;
            return pick(modalSettings).then((items) => {
                let itemsToInsert = items.filter(a=> {
                    var indx = entity[navitemsPropname].findIndex(aa=> aa[rightIdPropertyName].ID === a.ID && aa.entityAspect.entityState !== breeze.EntityState.Detached);
                    return indx < 0;
                });
                var promises = [];
                for(let item of itemsToInsert) {
                    var tr = dataService.createEntity(navItemType, { rightIdPropertyName: item, leftIdPropertyName: entity });
                    promises.push(tr);

                }
                return $q.all(promises).then((trToAdd) => dataService.saveEntities(trToAdd));


            });
        }


    }




})();