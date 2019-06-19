(function (angular) {

    "use strict";

    var config = {
        valueEntityTypes:
        [
            {
                name: 'Street',
                title: 'רחובות',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם רחוב', value: 'StreetName' }, { title: 'שם עיר', value: 'City.CityName' }]
            },
            {
                name: 'City',
                title: 'ערים',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם עיר', value: 'CityName' }]
            },
            {
                name: 'Gender',
                title: 'מגדר',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'GenderName' }]
            },
            {
                name: 'Bank',
                title: 'בנק',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'BankName' }]
            },
            {
                name: 'BankBranch',
                title: 'סניף בנק',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'BranchName' }, { title: 'מס\' סניף', value: 'BranchCode' }, { title: 'בנק', value: 'Bank.BankName' }]
            },
            //{
            //    name: 'AppointmentRanking',
            //    title: 'דירוג מינוי',
            //    fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'AppointmentRankingName' }]
            //},

            {
                name: 'CourseType',
                title: 'סוגי קורס',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'CourseName' }]
            },
            {
                name: 'EducationType',
                title: 'השכלה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'EducationTypeName' }]
            },
            {
                name: 'FamilyStatus',
                title: 'מצב משפחתי',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'FamilyStatusName' }]
            },
            {
                name: 'Profession',
                title: 'מקצוע',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'ProfessionName' }]
            },
            {
                name: 'Rank',
                title: 'דרגה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'RankName' }]
            },
            {
                name: 'SalaryRanking',
                title: 'דירוג משכורת',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'SalaryRankingName' }]
            },
            {
                name: 'DurationUnits',
                title: 'יחידות זמן',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'DurationUnitName' }]
            },

            {
                name: 'EmploymentStatus',
                title: 'מצב תעסוקה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'EmploymentStatusName' }]
            },
            {
                name: 'EntryReason',
                title: 'סיבת כניסה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'EntryReasonName' }, { title: 'סוג שרות', value: 'ServiceTypeId' }, { title: 'מזהה סיבה', value: 'EntryReasonId' }]
            },
            //{
            //    name: 'FundTypeBL',
            //    title: 'קרן',
            //    fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'FundName' }]
            //},

            {
                name: 'KindergartenType',
                title: 'גני ילדים',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'KindergartenTypeName' }]
            },
            //{
            //    name: 'LiabilityType',
            //    title: 'סוגי התחייבות',
            //    fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'LiabilityTypeName' }]
            //},
            {
                name: 'PaymentType',
                title: 'סוגי תשלומים',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'PaymentTypeName' }]
            },
            {
                name: 'PopulationType',
                title: 'סוג אוכלוסיה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'PopulationTypeName' }, { title: 'זמן תוקף לאחר תאריך סיום (בימים)', value: 'ValidDaysAfterEndDate' }, { title: 'סדר מיון', value: 'PreferenceOrder' }]
            },
            {
                name: 'ServiceStatus',
                title: 'סטטוס שרות',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'ServiceStatusName' }]
            },
            {
                name: 'TaxCoordinationType',
                title: 'תיאום מס',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'TaxCoordinationTypeName' }]
            },
            {
                name: 'TaxExemptionGrantType',
                title: 'מענק פטור ממס',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'TaxExemptionGrantName' }]
            },
            {
                name: 'ServiceType',
                title: 'סוג שרות',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'ServiceTypeName' }]
            },
            {
                name: 'FundType',
                title: 'סוגי קרנות',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'FundTypeName' }]
            },
            {
                name: 'SocialSecurityExemptionType',
                title: 'סוג פטור ביטוח לאומי',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'SocialSecurityExemptionTypeName' }]
            },
            {
                name: 'NotificationConfig',
                title: 'סוגי דיווח',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'Title' }]
            },
            {
                name: 'EducationReason',
                title: 'סיבות השכלה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'EducationReasonName' }]
            },
            {
                name: 'PensionType',
                title: 'סוג פנסיה',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'PensionTypeName' }]
            },

            {
                name: 'PartnerEmploymentStatus',
                title: 'מצב תעסוקת בן זוג',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'PartnerEmploymentStatusName' }]
            },
            {
                name: 'Aguda',
                title: 'אגודות',
                fields: [{ title: '#', value: 'ID' }, { title: 'שם', value: 'AgudaName' }]
            },
            {
                name: 'ChildStatus',
                title: 'סטטוס ילדים',
                fields: [{ title: '#', value: 'ID' }, {
                    title: 'שם', value: 'ChildStatusName'
                }]
            },
            {
                name: 'CivilianEmployeeSalaryRanking',
                title: 'דירוג שכר אע"צ',
                fields: [{ title: '#', value: 'ID' }, {
                    title: 'שם', value: 'CivilianEmployeeSalaryRankingName'
                }]
            },
            {
                name: 'EducationInstitution',
                title: 'מוסדות לימוד',
                fields: [{ title: '#', value: 'ID' }, {
                    title: 'שם', value: 'Name'
                }]
            },
            {
                name: 'EducationMajor',
                title: 'מגמה ראשית',
                fields: [{ title: '#', value: 'ID' }, {
                    title: 'שם', value: 'Name'
                }]
            },


        {
            name: 'FinancialAssistanceType',
            title: 'סוג עזרה כספית',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'FinancialAssistanceTypeName'
            }]
        },
        {
            name: 'HousingSolutionType',
            title: 'סוגי פתרונות דיור',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'HousingSolutionTypeName'
            }]
        },
        {
            name: 'Months',
            title: 'חודשים',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'MonthName'
            }]
        },
        {
            name: 'MutavType',
            title: 'סוגי מוטבים',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'MutavTypeName'
            }]
        },
        {
            name: 'PensionerType',
            title: 'סוגי פנסיונרים',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'PensionerTypeName'
            }]
        },
        {
            name: 'PensionPercentage',
            title: 'אחוז פנסיה',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'PensionPercentageName'
            }]
        },
        {
            name: 'RentReasonType',
            title: 'סיבות שכירות',
            fields: [{
                title: '#', value: 'ID'
            },
            
             {
                 title: 'שם', value: 'RentReasonName'
             },
             {
                 title: 'נשוי בלבד', value: 'IsMarried | yesNo'

             }]
        },
        {
            name: 'TaxCoordinationReason',
            title: 'סיבות תיאום מס',
            fields: [{
                title: '#', value: 'ID'
            }, {
                title: 'שם', value: 'Reason'
            }]
        },
            {
                name: 'TaxExemptionReason',
                title: 'סיבות פטור מס',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'Reason'
                }]
            }, {
                name: 'CompensationStudy',
                title: 'פיצוי לימודים',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'CompensationStudyName'
                }]
            }, {
                name: 'InquirySubject',
                title: 'נושא פניה',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'נושא', value: 'SubjectName'
                },
                 { title: 'מדור', value: 'Department.DepartmentName' }]
            }, {
                name: 'TicketStatus',
                title: 'מצב קובץ',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'name'
                }]
            }, {
                name: 'ContentSubject',
                title: 'נושא תוכן',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'Node Id', value: 'SubjectNodeId'
                }]
            }, {
                name: 'ApartmentTransferReason',
                title: 'סיבת מעבר דירה',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'ApartmentTransferReasonName'
                }, {
                    title: 'נשוי בלבד', value: 'IsMarried | yesNo'
                }, {
                    title: 'נדרש קובץ', value: 'IsFileAttached | yesNo'
                }]
            }, {
                name: 'ApartmentTransferType',
                title: 'סוג מעבר דירה',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'ApartmentTransferTypeName'
                }]
            }, {
                name: 'Department',
                title: 'מדור',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'DepartmentName'
                }]
            }, {
                name: 'ChildRefundType',
                title: 'סוגי החזר ילדים',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'ChildRefundTypeName'
                }]
            }, {
                name: 'ChildRefundAmount',
                title: 'סכום החזר ילדים',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'סוג', value: 'ChildRefundTypeId'
                }, {
                    title: 'סכום', value: 'Amount'
                }]
            }, {
                name: 'ChildRefundPaymentType',
                title: 'החזרי ילדים סוגי תשלום',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'Name'
                }]
            }, {
                name: 'TaxBenefitsType',
                title: 'סוגי הטבות מס',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'TaxBenefitName'
                }]
            }, {
                name: 'TaxBenefitsReason',
                title: 'סיבות הטבות מס',
                fields: [{
                    title: '#', value: 'ID'
                }, {
                    title: 'שם', value: 'ReasonName'
                }, {
                    title: 'סוג', value: 'TaxBenefitTypeId'
                }, {
                    title: 'נקודות זיכוי', value: 'CreditAmount'
                }]
            }
        ]

    };

    angular.module('coeco.values')
        .constant("coecoValuesConfig", config)
        .config([
            'entityTableDefinitionsProvider', 'coecoValuesConfig', function (entityTableDefinitionsProvider, coecoConfig) {
                angular.forEach(coecoConfig.valueEntityTypes, function (type) {
                    entityTableDefinitionsProvider.registerDefinition(type.name, type);

                });

            }
        ]);


})(angular);