(function (angular) {
    "use strict";

    var app = angular.module('coeco.nav');

    app.factory('navService', navService);

    navService.$inject = ['$state'];
    function navService($state) {

        return {
            getNavModel: getNavModel,
            getBreadcrumbs: getBreadcrumbs,
            stateHasSubMenu: stateHasSubMenu
        };

        function getNavModel() {
            var allNavs = $state.get();

            return allNavs.filter(function (s) {
                return !s.abstract && s.name.indexOf('.') < 0;
            }).map(function (s) {
                return {
                    name: s.name,
                    title: s.data ? s.data.title : '',
                    icon: s.data ? (s.data.icon || 'link') : '',
                    hasSubMenu: s.data.hasSubMenu ? s.data.hasSubMenu : '',
                    requiresPermission: s.data ? s.data.requiresPermission : '',
                    isCurrent: $state.current.name === s.name,
                    orderIndex: getNavModelOrder(s)
                };
            });
        }

        function getNavModelOrder(s) {
            switch (s.data.title) {
                case 'מנהלי מערכת':
                    return 1;
                case 'משתמשי מערכת':
                    return 2;
                case 'ניהול פריטים':
                    return 3;
                case 'ניהול יחידות':
                    return 4;
                case 'ניהול שאלות דירוג':
                    return 5;
                case 'ניהול מסרונים':
                    return 6;
                case 'ניהול פרופילי הרשאות':
                    return 7;
                case 'ניהול משתמשים':
                    return 8;
                case 'ניהול דוח פרטי הזמנה':
                    return 9;
                case 'דוחות':
                    return 10
                default:
                    return 11;
            }
        }

        function getBreadcrumbs(evaluatedState) {
            var bc = [];

            var current = evaluatedState || $state.current;
            if (!current.name) {
                return undefined;
            }
            if (current.data.parent && !current.name.includes(current.data.parent + '.')) {
                current.name = current.data.parent + '.' + current.name;
            }

            var components = current.name.split('.');
            var point = '';
            for (var i = 0; i < components.length; i++) {
                var component = components[i];
                point = point + (i === 0 ? '' : '.') + component;
                var state = $state.get(point);

                if (state) {
                    if (!state.abstract) {
                        bc.push({
                            title: state.data.title,
                            name: state.name,
                            data: state.data
                        });
                    }
                }
                else {
                    state = $state.get(component);
                    if (state) {
                        bc.push({
                            title: state.data.title,
                            name: state.name,
                            data: state.data
                        });
                    }
                }

            }


            return bc;


        }

        function stateHasSubMenu(evaluatedState) {
            var bc = getBreadcrumbs(evaluatedState);
            return bc.length && bc[0].data && bc[0].data.hasSubMenu;
        }



    }


})(angular);
