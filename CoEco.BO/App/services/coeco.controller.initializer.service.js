(function (angular) {
    "use strict";
    angular.module('coeco').factory('coecoControllerInitializer', factory);

    factory.$inject = ['commonConfig', 'coecoDataService', '$stateParams', '$state', '$window', '$rootScope', 'common', 'datePickerLocale', '$uibModal', '$filter', '$q'];

    function factory(commonConfig, coecoDataService, $stateParams, $state, $window, $rootScope, common, datePickerLocale, modal, $filter, $q) {

        function checkPermissions(vm) {
            //Move to data service?
            switch (vm.permissionName) {
                case 'users':
                    common.authService.privileges().then(p => setPermission(vm, p.users));
                    break;
                case 'usersAdmin':
                    common.authService.privileges().then(p => setPermission(vm, p.usersAdmin));
                    break;
                case 'userAdminEdit':
                    common.authService.privileges().then(p => setPermission(vm, p.userAdminEdit));
                    break;
                case 'manageItems':
                    common.authService.privileges().then(p => setPermission(vm, p.manageItems));
                    break;
                case 'units':
                    common.authService.privileges().then(p => setPermission(vm, p.units));
                    break;
                case 'ratingQuestions':
                    common.authService.privileges().then(p => setPermission(vm, p.ratingQuestions));
                    break;
                case 'notifications':
                    common.authService.privileges().then(p => setPermission(vm, p.notifications));
                    break;
                case 'permissionsProfiles':
                    common.authService.privileges().then(p => setPermission(vm, p.permissionsProfiles));
                    break;
                case 'members':
                    common.authService.privileges().then(p => setPermission(vm, p.members));
                    break;
                case 'orderDataReport':
                    common.authService.privileges().then(p => setPermission(vm, p.orderDataReport));
                    break;

            }
            switch (vm.uploadPermissionName) {
                case 'users':
                    common.authService.privileges().then(p => vm.canUpload = p.nofashimUploadFiles > 1);
                    break;
                default:
                    common.authService.privileges().then(p => vm.canUpload = 1);
                    break;
            }
        }

        function setPermission(vm, permissionName) {
            vm.canEdit = permissionName > 1;
            vm.canShow = permissionName > 0;
        }

        return {
            initListController: initListController,
            initDetailsController: initDetailsController,
            initPermissionsOnly: initPermissionsOnly
        };

        function initListController(vm, callback) {

            vm.pagingContext = angular.copy(commonConfig.defaultPagingContext);
            vm.setPageSize = setPageSize;
            vm.setPage = setPage;
            vm.getNumPages = getNumPages;
            vm.resetSearch = resetSearch;
            vm.checkPermissions = checkPermissions;
            vm.search = search;
            vm.exportToExcel = exportToExcel;
            vm.exportToText = exportToText;
            vm.getWhereClause = getWhereClause;
            vm.locale = datePickerLocale;
            vm.reorderBy = reorderBy;
            vm.expand = vm.expand || [];
            vm.setToDateStyle = setToDateStyle;


            checkPermissions(vm);
            

            function reorderBy(orderBy) {
                if (vm.orderBy === orderBy) {
                    vm.orderBy = vm.orderBy + ' desc';
                }
                else {
                    vm.orderBy = orderBy;
                }
                vm.search();
            }

            function resetSearch() {
                vm.filters = null;
                vm.pagingContext.pageIndex = 0;
                vm.search();

                $.ajax({
                    url: '/coeco/logger/log',
                    //data: { id: id }
                }).done(function () {
                    alert('Reset');
                });


            }

            function setToDateStyle(element_id) {
                //var id_selector = '#' + element_id;
                //$($($(id_selector).parent()).find('.dropdown-menu')).css('left', 0);
            }


            function exportToExcel() {
                coecoDataService.exportToExcel(buildQuery(vm));
            }

            function exportToText() {
                coecoDataService.exportToText(buildQuery(vm));
            }

            function buildQuery() {
                let queryObj = {
                    from: vm.entityType,
                    where: {
                        'ID': {
                            'eq': vm.id
                        }
                    },
                    expand: vm.expand || [],
                    inlineCount: true,
    
                    orderBy: [vm.orderBy],
                    skip: vm.pagingContext.itemsPerPage * vm.pagingContext.pageIndex,
                    take: vm.pagingContext.itemsPerPage
                };
                if (vm.getWhereClause) {
                    queryObj.where = vm.getWhereClause();
                }
                if (vm.hasParent) {
                    var pspIdObj = {
                        ParentSalePeriodId: vm.parent
                    };
                    if (queryObj.where) {
                        angular.extend(queryObj.where, queryObj.where, pspIdObj);
                    }
                    else {
                        queryObj.where = pspIdObj;
                    }
                }
                if (vm.PersonalNumber) {
                    var pspIdObj = {
                        PersonalNumber: vm.PersonalNumber
                    };
                    if (queryObj.where) {
                        angular.extend(queryObj.where, queryObj.where, pspIdObj);
                    }
                    else {
                        queryObj.where = pspIdObj;
                    }
                }

                if (vm.Id) {
                    var idObj = {
                        Id: vm.Id
                    };
                    if (queryObj.where) {
                        angular.extend(queryObj.where, queryObj.where, idObj);
                    }
                    else {
                        queryObj.where = idObj;
                    }
                }
                //the order by is an array of one containing the string devided by comma that represnt the sorting fields
                if (queryObj.orderBy) {
                    queryObj.orderBy = queryObj.orderBy && queryObj.orderBy[0] ? queryObj.orderBy[0].split(',') : '';
                }

                return queryObj;
            }

            function getStatusValueByName(name) {
                return $filter('filter')(name, 'name');
            }

            function setPage(pageIndex) {
                if (vm.pagingContext.pageIndex === pageIndex) {
                    return;
                }
                vm.pagingContext.pageIndex = pageIndex;
                vm.search();
            }

            function setPageSize(pageSize) {
                vm.pagingContext.itemsPerPage = pageSize;
                vm.search(true);
            }

            function getNumPages() {
                return parseInt(vm.pagingContext.totalRecords / vm.pagingContext.itemsPerPage) + (vm.pagingContext.totalRecords % vm.pagingContext.itemsPerPage ? 1 : 0);
            }

            function getWhereClause() {

                if (vm.filters) {
                    for (var i in vm.filters) {//delete id of empty imput field
                        if (vm.filters[i] == "" || vm.filters[i] == null)
                            delete vm.filters[i];
                    }
                    if (vm.filters.Id && !vm.filters.Id.eq) {
                        delete vm.filters.Id;
                    }
                    if (vm.filters.ID && !vm.filters.ID.eq) {
                        delete vm.filters.ID;
                    }
                    if (vm.filters.ServiceDepartmentId && !vm.filters.ServiceDepartmentId.eq) {
                        delete vm.filters.ServiceDepartmentId;
                    }
                    if (vm.filters.ServiceSubjectId && !vm.filters.ServiceSubjectId.eq) {
                        delete vm.filters.ServiceSubjectId;
                    }
                }
                if (vm.filters && Object.keys(vm.filters).length === 0) {
                    vm.filters = null;
                }

                return vm.filters;
            }

            //var search =
            function search(resetPageIndex) {
                if (resetPageIndex) {
                    vm.pagingContext.pageIndex = 0;
                }
                coecoDataService.list(buildQuery(), true).then(function (data) {
                    vm.entities = data.results;
                    vm.items = data.results;
                    vm.pagingContext.totalRecords = data.inlineCount;
                    return vm.entities;
                });
            }
            //return search;
            if (callback) {
                callback();
            }

            return coecoDataService.list(buildQuery(), true).then(function (data) {
                vm.entities = data.results;
                vm.items = data.results;
                vm.pagingContext.totalRecords = data.inlineCount;
                return vm.entities;
            });
        }

        function initDetailsController(vm, init) {
            vm.errors = [];
            vm.update = update;
            vm.updateAlways = updateAlways;
            vm.expand = vm.expand || [];
            vm.setPage = setPage;
            vm.setPageSize = setPageSize;
            vm.checkPermissions = checkPermissions;
            vm.getNumPages = getNumPages;
            vm.pagingContext = angular.copy(commonConfig.defaultPagingContext);
            //vm.isNew = vm.id === '0';
            vm.reset = reset;
            vm.setToDateStyle = setToDateStyle;

            checkPermissions(vm);
            vm.save = save;
            vm.setBasePropertiesBeforeSave = setBasePropertiesBeforeSave;


            vm.handleErrors = (e) => {
                vm.errors = e.entityErrors;
                return common.$q.reject(e);
            };

            vm.returnToList = () => {
                var currentState = $state.$current.name;
                return $state.go(currentState.replace('.details', '.list'));

            };
            function save() {
                setBasePropertiesBeforeSave().then(function (data) {
                    coecoDataService.saveEntity(vm.entity)
                       .then(vm.returnToList, vm.handleErrors)
                       .then(() => common.notify.success('הפריט נשמר בהצלחה'));
                });

            }

            function setToDateStyle(element_id) {
                var id_selector = '#' + element_id;
                $($($(id_selector).parent()).find('.dropdown-menu')).css('left', 0)
            }

            function setBasePropertiesBeforeSave() {
                var deferred = $q.defer();
                if (vm.entity.ID < 1){
                    vm.entity.CreatedOn = new Date();
                }
                vm.entity.UpdatedOn = new Date();
                deferred.resolve(vm.entity);
                return deferred.promise;
            }

            function reset() {
                coecoDataService.rejectChanges(vm.entity)
                    .then((item) => vm.entity = item);
            }


            function setPage(pageIndex) {
                if (vm.pagingContext.pageIndex === pageIndex) {
                    return;
                }
                vm.pagingContext.pageIndex = pageIndex;
                vm.search();
            }
            function setPageSize(pageSize) {
                vm.pagingContext.itemsPerPage = pageSize;
                vm.search(true);
            }
            function getNumPages() {
                return parseInt(vm.pagingContext.totalRecords / vm.pagingContext.itemsPerPage) + (vm.pagingContext.totalRecords % vm.pagingContext.itemsPerPage ? 1 : 0);
            }

            var listener = $rootScope.$on('$stateChangeStart', (e) => {
                if (vm.entity && (vm.entity.entityAspect.entityState.isModified() || vm.entity.entityAspect.entityState === breeze.EntityState.Added)) {
                    if (!confirm('לא שמרת את השינויים שלך, האם להמשיך?')) {
                        e.preventDefault();
                        return;
                    } else {
                        if (vm.entity.entityAspect.entityState.isModified()) {
                            vm.entity.entityAspect.rejectChanges();
                        } else {
                            vm.entity.entityAspect.setDeleted();
                        }
                    }
                }

                listener();
            });

            function update(item) {
                if (!item.entityAspect.entityState.isUnchanged()) {
                    coecoDataService.saveEntity(item)
                            .then(vm.returnToList, vm.handleErrors)
                            .then(() => common.notify.success('הפריט נשמר בהצלחה'));
                }
            }

            function updateAlways(item) {
                //if (!item.entityAspect.entityState.isUnchanged()) {
                if (item.entityAspect.entityState) {
                    coecoDataService.saveEntity(item)
                            .then(vm.returnToList, vm.handleErrors)
                            .then(() => common.notify.success('הפריט נשמר בהצלחה'));
                }
            }

            return coecoDataService.getById(vm.entityType, vm.id, vm.expand, init).then((data) => {
                if (data) {
                    vm.entity = data;
                }
                return vm.entity;
            });
        }

        function initPermissionsOnly(vm) {
            checkPermissions(vm);
        }
    }

})(angular);