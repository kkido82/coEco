(function (angular) {

    angular.module('coeco').factory('coecoDataService', dataService);

    dataService.$inject = ['breeze', '$q', 'breezeUriBuilder', '$http', 'common', '$window'];
    function dataService(breeze, $q, breezeUriBuilder, $http, common, $window) {
        var endpoint, manager, metadata, isReady;
        activate();

        var service = {
            saveChanges: saveChanges,
            update: update,
            add: add,
            manager: manager,
            createEntity: createEntity,
            saveEntity: saveEntity,
            rejectChanges: rejectChanges,
            saveEntities: saveEntities,
            list: list,
            getAll: getAll,
            getById: getById,
            fetchEntity: fetchEntity,
            resolveEntityResourceName: resolveEntityResourceName,
            url: url,
            exportToExcel: exportToExcel,
            exportToText: exportToText,
            remove: remove,
            clearServerErrors: clearServerErrors,
            getEntityType: getEntityType,
            base64ToBlob: base64ToBlob,
            getEndDay: getEndDay,
            getDuration: getDuration,
            getStringDaysOFWeek: getStringDaysOFWeek,
            importcoecoFile: importcoecoFile,
            toggleApproved: toggleApproved,
            ensureMetadata: ensureMetadata,
            refreshEntity:refreshEntity,

            get isReady() {
                return !!isReady;
            }
        };

        return service;

        function activate() {


            endpoint = '/breeze/coeco';
            manager = new breeze.EntityManager(endpoint);
            manager.metadataStore.metadataFetched.subscribe(function (e) {
                if (angular.isString(e.rawMetadata)) {
                    return;
                }
                metadata = e.rawMetadata;
                isReady = true;

            });
        }
        // refresh a specific SalePeriod
        function refreshEntity(entity) {
            // Todo: add parameter error checking?
            var query = breeze.EntityQuery.fromEntities([entity]);
            return manager.executeQuery(query).then(success, console.log(query));

            function success(data) {
                return data.results[0]; // queries return arrays; caller wants the first.
            }          
        }
    
        function getEndDay(startDay, duration) {
            var endDay = startDay + duration;
            if (endDay > common.daysOfWeekArr.length) {
                endDay = endDay - common.daysOfWeekArr.length;
            }
            return endDay;
        }
        function getDuration(startDay, endDay) {
            var duration = (endDay - startDay);
            if (duration < 0) {
                duration = duration + (common.daysOfWeekArr.length - 1);
            }
            return duration + 1;
        }
        function getStringDaysOFWeek(startNight, lastNight) {
            //get duration
            var duration = getDuration(startNight, lastNight);
            var text = "";
            var index = startNight;
            var counter = 0;
            while (counter < duration) {
                if (index >= common.daysOfWeekArr.length)
                { index = index - common.daysOfWeekArr.length; }

                counter++;
                if (index == 0) index++;
                text += common.daysOfWeekArr[index] + ",";
                index++;
            }
            text = text.substring(0, text.length - 1);
            return text;
        }

        function importcoecoFile(fileType, uploader) {

            var deferred = $q.defer();

            uploader.url = '/File/UploadExcelMembersFile/?fileType=' + fileType;
            uploader.upload();

            uploader.onSuccess = function (response, status, headers) {
                deferred.resolve([response, status]);
            };
            uploader.onError = function (response, status, headers) {
                deferred.reject([response, status]);
            };

            return deferred.promise;
        }

        function handleError(e) {
            vm.errors = e.entityErrors;
            return common.$q.reject(e);
        }

        function ensureMetadata() {
            if (isReady) {
                return $q.when(isReady);
            }
            var deferred = $q.defer();
            manager.metadataStore.metadataFetched.subscribe(function (e) {
                deferred.resolve(true);

            });
            manager.fetchMetadata();

            return deferred.promise;
        }

        function save(entity) {
            var toBeSaved = angular.isArray(entity) ? entity : [entity];
            return manager.saveChanges(toBeSaved);
        }

        function saveEntity(entity) {
            if (entity.entityAspect.entityState === breeze.EntityState.Detached) {
                manager.addEntity(entity);
            }
            return save(entity);
        }



        function saveChanges() {
            return save();
        }

        function update(entity) {
            if (angular.isArray(entity)) {
                return save(entity);
            }
            return save([entity]);
        }
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
        function createEntity(entityType, init) {
            return ensureMetadata().then(() =>  manager.createEntity(entityType, init));
        }

        function rejectChanges(entity) {

            entity.entityAspect.rejectChanges();

            if (entity.entityAspect.entityState.isDetached()) {
                return createEntity(entity.entityType);
            }

            return $q.when(entity);
        }

        function saveEntities(entities) {
            return save(entities);
        }
        function getAll(funcName, compareName, compareVal) {
            var query = breeze.EntityQuery
                    .from(funcName);

            if (compareName) {
                query = query.where(compareName, "==", compareVal);
            }

            return manager.executeQuery(query);
        }
        function list(queryObj, overideChanges) {
            var query = new breeze.EntityQuery(queryObj);
            if (overideChanges) {
                query = query.using(breeze.MergeStrategy.OverwriteChanges);
            }
            return manager.executeQuery(query);
        }

        function getById(entityType, id, expand, init) {
            if (id === 0 || id ==="0") {
                if (init) {
                    return createEntity(entityType, init);
                }

                else {
                    return createEntity(entityType, { CreatedBy: 'Me', UpdatedBy: 'Me' });
                }
            }
            return resolveEntityResourceName(entityType).then((rn) => {
                //SYS_TBL_SyncFromRedRecords
                let qobj = rn != "SYS_TBL_SyncFromRedRecords" ? { from: rn, where: { ID: id } } : { from: rn, where: { RowId: id } };
                if (expand) {
                    qobj.expand = expand;
                }
                var query = new breeze.EntityQuery(qobj);

                return manager.executeQuery(query)
                    .then((d) => d.results.length ? d.results[0] : null);

            });
            //return manager.fetchEntityByKey(entityType, id);
        }



        function resolveEntityResourceName(entityType) {
            return ensureMetadata()
                        .then(() => manager.metadataStore._getEntityType(entityType, false)).then(et=>et.defaultResourceName);
        }

        function fetchEntity(entityType, id) {
            return manager.fetchEntityByKey(entityType, id, true).then((data) => data.entity);
        }

        function url(queryObj) {
            var query = new breeze.EntityQuery(queryObj);
            var uri = breezeUriBuilder.buildUri(query, manager.metadataStore);
            return endpoint + '/' + uri;
        }

        function remove(...entities) {
            for(let entity of entities) {
                entity.entityAspect.setDeleted();
            }

            return save(entities);
        }

        function clearServerErrors(entity) {
            entity.entityAspect.getValidationErrors().forEach((err) => {
                if (err.isServerError) { entity.entityAspect.removeValidationError(err); }
            });
        }

        function getEntityT(entityName) {
            return ensureMetadata().then(() => {
                var entityType = manager.metadataStore.getEntityType(entityName);
                return entityType;
            });

        }

        function getEntityType(entityResourceName) {
            return ensureMetadata().then(() => {
                var entityType = manager.metadataStore.getEntityTypeNameForResourceName(entityResourceName);
                return manager.metadataStore._getEntityType(entityType, false);
            });

        }
        function toggleApproved(item) {
            item.IsApprovedByAdmin = !item.IsApprovedByAdmin;
            if (!item.entityAspect.entityState.isUnchanged()) {
                saveEntity(item).then(() => common.notify.success('הפריט נשמר בהצלחה'), handleError);

            }
        }

        function updateQueryStringParameter(uri, key, value) {
            var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
            var separator = uri.indexOf('?') !== -1 ? "&" : "?";
            if (uri.match(re)) {
                return uri.replace(re, '$1' + key + "=" + value + '$2');
            }
            else {
                return uri + separator + key + "=" + value;
            }
        }

        function exportToExcel(queryObject) {
            var urlT = url(queryObject);
            urlT = updateQueryStringParameter(urlT, "fType", "excel");
            $window.open(urlT);
        }

        function exportToText(queryObject) {
            var urlT = url(queryObject);
            urlT = updateQueryStringParameter(urlT, "fType", "text");
            $window.open(urlT);
        }

        function base64ToBlob(i_base64String, i_mimeType, i_sliceSize) {
            var v_mimeType = i_mimeType || '';
            var v_sliceSize = i_sliceSize || 512;

            var byteCharacters = atob(i_base64String);
            var byteArrays = [];

            for (var offset = 0; offset < byteCharacters.length; offset += v_sliceSize) {
                var slice = byteCharacters.slice(offset, offset + v_sliceSize);

                var byteNumbers = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                var byteArray = new Uint8Array(byteNumbers);

                byteArrays.push(byteArray);
            }

            var blob = new Blob(byteArrays, { type: v_mimeType });
            return {
                "object": blob,
                "url": URL.createObjectURL(blob)
            };
        }

    }


})(angular);