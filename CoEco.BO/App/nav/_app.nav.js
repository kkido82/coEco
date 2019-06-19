(function (angular) {
    "use strict";

    var app = angular.module('coeco.nav', ['coeco.common', 'ui.router']);

    app.run(setup);

    setup.$inject = ['$rootScope', '$state', 'navService','$timeout'];
    function setup($rootScope, $state, navService,$timeout) {
        $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                $rootScope.submenuOpen = navService.stateHasSubMenu(toState);

            });
    }

})(angular);
