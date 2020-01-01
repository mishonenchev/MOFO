function CityViewModel() {
    var self = this;
    self.getSchools = function () {
        loadSchools();
    };
    self.openConfirmModal = function () {
        var vm = new ConfirmCityModal();
        self.confirmCityModal(vm);
        $("#confirmCityModal").addClass("in");
        $("#confirmCityModal").attr("style", "display: block");
        $("#confirmCityModal").scrollTop(0);
        $("body").addClass("modal-open");
    };
    self.openEditModal = function () {
        var vm = new EditCityModal();
        self.editCityModal(vm);
        $("#editCityModal").addClass("in");
        $("#editCityModal").attr("style", "display: block");
        $("#editCityModal").scrollTop(0);
        $("body").addClass("modal-open");
    };
    
    self.confirmCityModal = ko.observable();
    self.editCityModal = ko.observable();

    function EditCityModal() {
        var self = this;
        self.id = ko.observable();
        self.modalCss = ko.observable("modal-success");
        self.modalHeader = ko.observable("Редактиране на населено място");
        self.saveBtnText = ko.observable("Запази");
        self.checkView = function () {
            self.name.validate();
            return !self.name.isInvalid();
        };
        self.saveChanges = function () {
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();

            if (self.checkView() === true) {
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/admin/EditCity",
                    data: {
                        cityId: cityId,
                        name: self.name(),
                        __RequestVerificationToken: token
                    },
                    success: function (data) {
                        if (data.status === "OK") {
                            $("#editCityModal").removeClass("in");
                            $("#editCityModal").attr("style", "display: none");
                            $("body").removeClass("modal-open");
                            location.reload();
                        }
                    }
                });
            }
        };

        self.closeBtn = function () {
            $("#editCityModal").removeClass("in");
            $("#editCityModal").attr("style", "display: none");
            $("body").removeClass("modal-open");
        };
    
    self.name = ko.observable().extend({ addSubObservables: true });
    self.name.subscribe(function () {
        self.name($.trim(self.name()));
        if (self.name() === undefined || self.name() === '') {
            invalid(self.name, "Името на населеното място е задължително поле.");
        } else if (self.name().length > 130) {
            invalid(self.name, "Твърде много символи.");
        } else {
            valid(self.name);
        }
    });
    }
    function ConfirmCityModal() {
        var self = this;
        self.cityName = ko.observable(cityName);
        self.confirmCity = function () {
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/admin/ConfirmCity",
                data: {
                    cityId: cityId,
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status === "OK") {
                        $("#confirmCityModal").removeClass("in");
                        $("#confirmCityModal").attr("style", "display: none");
                        $("body").removeClass("modal-open");
                        location.reload();
                    }
                }
            });
        };
        self.closeBtn = function () {
            $("#confirmCityModal").removeClass("in");
            $("#confirmCityModal").attr("style", "display: none");
            $("body").removeClass("modal-open");
        };
    }
    function loadSchools() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/GetSchoolsInCity",
            data: {
                cityId: cityId
            },
            success: function (data) {
                if (data.status === "OK") {
                    self.schools.removeAll();
                    for (var i in data.schools) {
                        self.schools.push(data.schools[i]);
                    }
                }
            }
        });
    }
    self.schools = ko.observableArray();
}
var vm = new CityViewModel();
ko.applyBindings(vm);
vm.getSchools();