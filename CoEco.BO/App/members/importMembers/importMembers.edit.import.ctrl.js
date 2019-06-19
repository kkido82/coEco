(function () {
    'use strict';

    angular
        .module('coeco')
        .controller('MembersUploadCtrl', MembersUploadCtrl);

    MembersUploadCtrl.$inject = ['$uibModalInstance', 'coecoDataService', 'FileUploader', '$state', 'common', 'CoecoUploadFileType'];

    function MembersUploadCtrl($uibModalInstance, coecoDataService, FileUploader, $state, common, CoecoUploadFileType) {
        /* jshint validthis:true */
        this.isApproved = false;
        this.isUploading = false;
        this.buttonName = "ייבא";
        var vm = this;
        vm.ok = ok;
        vm.cancel = cancel;
        vm.file = new FileUploader();

        function ok() {
            vm.buttonName = "נתונים בבדיקה";   
            vm.isUploading = true;
            vm.errors = [];

            if (vm.file.queue.length <= 0) {
                return;
            }

            let fileType = CoecoUploadFileType.UploadFileType.CoecoMembers;

            coecoDataService.importcoecoFile(fileType, vm.file.queue[0])
                .then(handleSuccess, handleErrors);
        }

        //CallBack
        vm.file.onAfterAddingFile = function (fileItem) {
            //Called only if on chhose differnet file selected
            differentFileChoosedToUpload();
        };

        function differentFileChoosedToUpload() {
            vm.isApproved = false;
            vm.isUploading = false;
            vm.buttonName = "ייבא";
            vm.errors = [];
        }

        function handleErrors(response) {
            $uibModalInstance.close();
            return common.notify.error('ייבוא נכשל, סיבה:  ' + response);
        }

        function handleSuccess(response) {
            var responseObject = response[0];
            var FileReportPath = responseObject.FileReportPath;
            var FileUploadMsg = responseObject.FileUploadMsg;
            var FileUploadPath = responseObject.FileUploadPath;
            var isFileOKToUpload = responseObject.FileContentOk;
            if (FileReportPath) { //error
                FileUploadMsg = FileUploadMsg + " " + FileReportPath;
            } else {
                if (FileUploadPath) { //success
                    FileUploadMsg = FileUploadMsg + " " + FileUploadPath;
                }
            }
            if (vm.isApproved) {
                if (isFileOKToUpload) {
                    $uibModalInstance.close();
                    common.notify.success(FileUploadMsg);
                    return $state.go('coeco.members.list')
                        .then(() => common.notify.success(FileUploadMsg));
                } else {
                    handleErrors(FileUploadMsg);
                }
            } else {
                vm.errors.push(FileUploadMsg);
                vm.isUploading = false;
                if (isFileOKToUpload) {
                    vm.isApproved = true;
                    vm.buttonName = " ייבא בכל זאת";
                } else {
                    vm.buttonName = "אינך יכול להמשיך בתהליך";
                    vm.file.queue = [];
                }
            }
        }



        function cancel() {
            $uibModalInstance.dismiss();
        }

    }
})();