(function (angular) {
    "use strict";
    
    angular.module('coeco.ui').component('notificationAlert', {
        template: `<div class="row">
                        <div class ="col-lg-8 col-md-6 col-sm-12">
                            <div class ="alert alert-{{vm.alertType || 'danger'}}" ng-if="vm.errors.length">
                                  <ul class ="list-unstyled" >
                                        <li ng-repeat="err in vm.errors">{{err.errorMessage || err}} </li>
                                    </ul>
                             </div>
                        </div>
                    </div>`,
        bindings: {
            errors: '=',
            hideHeader: '@',
            alertType:'@'
        },
        controllerAs: 'vm',
        controller: function () {}

    
    });
})(angular);