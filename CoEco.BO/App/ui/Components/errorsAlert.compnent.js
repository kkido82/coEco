(function (angular) {
    "use strict";
    
    angular.module('coeco.ui').component('errorsAlert', {
        template: `<div class="row">
                        <div class ="col-lg-4 col-md-6 col-sm-12">
                            <div class ="alert alert-{{vm.alertType || 'danger'}}" ng-if="vm.errors.length">
                                   <h4><i class ="icon fa fa-ban"></i> שגיאות!</h4>
                                    <ul class="list-unstyled">
                                        <li ng-repeat="err in vm.errors">{{err.errorMessage || err}}</li>
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