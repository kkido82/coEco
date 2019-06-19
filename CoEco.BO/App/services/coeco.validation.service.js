(function (angular) {

    angular.module('coeco').factory('coecoValidationService', validationService);

    validationService.$inject = ['$q', 'common'];
    function validationService($q, common) {

        var service = {
            validateBeforeSave: validateBeforeSave
        };

        return service;
       

        function validateBeforeSave(vm) {
            vm.errors = [];
            var err = false;
            //switch (vm.entityType) {
            //    case 'PhoneNumber':
            //        if (!vm.entity.Names || vm.entity.Names.length == 0) {
            //            vm.errors.push('יש למלא שם איש קשר');
            //            err = true;
            //        }
            //        if (!vm.entity.Number1) {
            //            vm.errors.push('יש למלא שם מספר טלפון 1');
            //            err = true;
            //        }
            //        if (vm.entity.Number1 && (vm.entity.Number1.length < 9 || vm.entity.Number1.length > 10)) {
            //            vm.errors.push('מספר טלפון 1 לא תקין');
            //            err = true;
            //        }
            //        if (vm.entity.Number2 && (vm.entity.Number2.length < 9 || vm.entity.Number2.length > 10)) {
            //            vm.errors.push('מספר טלפון 2 לא תקין');
            //            err = true;
            //        }
            //        if (vm.PhoneNumberCategories.length == 0) {
            //            vm.errors.push('יש לבחור קטגוריה אחת לפחות');
            //            err = true;
            //        }
            //        break;
            //    case 'PhoneNumberCategory':
            //        if (!vm.entity.Names || vm.entity.Names.length == 0) {
            //            vm.errors.push('יש למלא שם קטגוריה');
            //            err = true;
            //        }
            //        break;
            //    case 'Regulation':
            //        if (!vm.entity.Title || vm.entity.Title.length == 0) {
            //            vm.errors.push('יש למלא שם פקודה/נוהל');
            //            err = true;
            //        }
            //        if (!vm.entity.Text || vm.entity.Text.length == 0) {
            //            vm.errors.push('יש למלא את פירוט הפקודה/הנוהל');
            //            err = true;
            //        }
            //        if (vm.RegulationCategories.length == 0) {
            //            vm.errors.push('יש לבחור קטגוריה אחת לפחות');
            //            err = true;
            //        }
            //        break;
            //    case 'RegulationCategory':
            //        if (!vm.entity.Names || vm.entity.Names.length == 0) {
            //            vm.errors.push('יש למלא שם קטגוריה');
            //            err = true;
            //        }
            //        break;
            //    case 'Messages':
            //        if (!vm.entity.Title || vm.entity.Title.length == 0) {
            //            vm.errors.push('יש למלא כותרת');
            //            err = true;
            //        }
            //        if (!vm.entity.ValidFrom) {
            //            vm.errors.push('יש למלא תאריך תחילת תוקף ');
            //            err = true;
            //        }
            //        if (!vm.entity.ValidTo) {
            //            vm.errors.push('יש למלא תאריך תום תוקף ');
            //            err = true;
            //        }
            //        break;
            //    default:
            //        break;
            //}
            return err;
        }
    }


})(angular);