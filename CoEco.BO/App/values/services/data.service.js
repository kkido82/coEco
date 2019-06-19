﻿(function (angular) {
    angular.module('coeco.values').factory('dataService', dataService);

    dataService.$inject = ['breeze', '$q', 'breezeUriBuilder'];
    function dataService(breeze, $q, uriBuilder) {
        var endpoint, manager, metadata, isReady;
        activate();

        var service = {
            manager: manager,
            create: create,
            add: add,
            saveChanges: saveChanges,
            fetchMetadata: fetchMatadata,
            list: list,
            url: url,
            //exportToExcel: exportToExcel,
            update: update,
            getById: getById,
            get isReady() {
                return !!isReady;
            }
        };

        return service;

        function add(entity) {
            var deferred = $q.defer();

            if (entity.entityType.autoGeneratedKeyType === breeze.AutoGeneratedKeyType.None) {
                isEntityExist(entity).then(function () {
                    manager.addEntity(entity);
                    deferred.resolve('התווסף בהצלחה!');
                }, function () {
                    deferred.reject('מזהה קיים במערכת!');
                });
            } else { // Identity or KeyGenerator
                manager.addEntity(entity);
                deferred.resolve('התווסף בהצלחה!');
            }

            return deferred.promise;
        }

        function isEntityExist(entity) {
            var deferred = $q.defer();

            getById(entity.entityType, entity.ID).then(function (result) {
                if (result.entity === null) {
                    deferred.resolve();
                }
                else {
                    deferred.reject();
                }
            });

            return deferred.promise;
        }


        function update(entity) {
            manager.saveChanges([entity]);
        }

        function activate() {
            endpoint = '/breeze/values';
            manager = new breeze.EntityManager(endpoint);
            manager.metadataStore.metadataFetched.subscribe(function (e) {
                if (angular.isString(e.rawMetadata)) {
                    return;
                }
                metadata = e.rawMetadata;
                isReady = true;

            });

        }

        function saveChanges() {
            return manager.saveChanges();
        }

        function create(entityType, init) {
            if (this.isReady)
                return $q.when(manager.createEntity(entityType, init, breeze.EntityState.Detached));

            return manager.fetchMetadata().then(function () { return manager.createEntity(entityType, init, breeze.EntityState.Detached); });
        }

        function getById(entityType, id) {
            return manager.fetchEntityByKey(entityType, id);
        }

        function fetchMatadata() {
            return manager.fetchMetadata();
        }

        function list(entitiesType, orderBy, pageIndex, itemsPerPage) {
            var query = getQuery(entitiesType, orderBy, pageIndex, itemsPerPage);
            return manager.executeQuery(query);
        }

        function url(entitiesType, orderBy, pageIndex, itemsPerPage) {
            var query = getQuery(entitiesType, orderBy, pageIndex, itemsPerPage);
            var uri = uriBuilder.buildUri(query, manager.metadataStore);
            return endpoint + '/' + uri + '&fType=excel';
        }

        function getQuery(entitiesType, orderBy, pageIndex, itemsPerPage) {
            var query = breeze.EntityQuery.from(entitiesType).orderBy(orderBy || 'CreatedOn');
            if (pageIndex) {
                query = query.skip(pageIndex * (itemsPerPage || 15));
            }

            query = query.take(itemsPerPage).inlineCount(true);

            return query;
        }

    }


})(angular);