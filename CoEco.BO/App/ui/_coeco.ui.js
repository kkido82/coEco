(function () {
    'use strict';

    angular.module('coeco.ui', ['angular-loading-bar', 'ngAnimate', 'ui.bootstrap', 'ngScrollbars']).config([
        'cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.spinnerTemplate = '<div class="spin-container"><span class="fa fa-4x fa-spinner fa-spin"></span></div>';
            cfpLoadingBarProvider.parentSelector = '#loading-bar-container';
        }
    ]).constant('datePickerLocale', {
        "format": "DD/MM/YYYY",
        "separator": " - ",
        "applyLabel": "בחר",
        "cancelLabel": "בטל",
        "fromLabel": "מ",
        "toLabel": "עד",
        "customRangeLabel": "מותאם אישית",
        "daysOfWeek": [
            "א",
            "ב",
            "ג",
            "ד",
            "ה",
            "ו",
            "ש"
        ],
        "monthNames": [
            "ינואר",
            "פברואר",
            "מרץ",
            "אפריל",
            "מאי",
            "יוני",
            "יולי",
            "אוגוסט",
            "ספטמבר",
            "אוקטובר",
            "נובמבר",
            "דצמבר"
        ],
        "firstDay": 0,
        "direction": "rtl"
    });
})();
