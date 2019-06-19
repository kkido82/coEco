(function () {
    'use strict';

    angular
        .module('coeco.ui')
        .factory('confirm', Factory);

    Factory.$inject = ['$uibModal'];

    function Factory(modal) {
     

        return open;

        function open(header, body) {
            return modal.open({
                templateUrl: '/app/ui/services/confirm.modal.tmpl.html',
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                controllerAs: 'vm',
                controller: ['$uibModalInstance', 'model', Controller],
                bindToController:true,
                resolve: { model: () => { return { header: header, body: body }; } }
            }).result;

        }
      
        function Controller($uibModalInstance, model) {
            let vm = this;
            vm.model = model;
            vm.ok = function () {
                
                $uibModalInstance.close();
            };

            vm.cancel = function () {
                
                $uibModalInstance.dismiss('cancel');
            };
        }
    }
})();