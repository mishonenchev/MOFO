function CityViewModel() {
    var self = this;
    self.getSchools = function () {
        switchViews();
        loadSchools();
    };
    self.getCities = function () {
        switchViews();
        loadCities();

    };
    self.openConfirmModal = function () {
        var vm = new ConfirmCityModal();
        self.confirmCityModal(vm);
        $("#confirmCityModal").addClass("in");
        $("#confirmCityModal").attr("style", "display: block");
        $("#confirmCityModal").scrollTop(0);
        $("body").addClass("modal-open");
    };
    self.createNewCity = function () {
        var vm = new AddCityModal();
        self.addCityModal(vm);
        $("#addCityModal").addClass("in");
        $("#addCityModal").attr("style", "display: block");
        $("#addCityModal").scrollTop(0);
        $("body").addClass("modal-open");
    };
    self.openMergeModal = function () {
        var vm = new MergeCityModal();
        self.mergeCityModal(vm);
        $("#mergeCityModal").addClass("in");
        $("#mergeCityModal").attr("style", "display: block");
        $("#mergeCityModal").scrollTop(0);
        $("body").addClass("modal-open");

        $('#citySelect').select2({
            ajax: {
                url: window.location.protocol + "//" + window.location.host + "/admin/searchCitiesModal",
                data: function (params) {
                    var query = {
                        query: params.term,
                        currentCityId: cityId
                    };
                    return query;
                }
            },
            placeholder: "Населено място"
        });
    };
    self.confirmCityModal = ko.observable();
    self.addCityModal = ko.observable();
    self.mergeCityModal = ko.observable();

    function MergeCityModal() {
        var self = this;
        self.modalCss = ko.observable("modal-success");
        self.modalHeader = ko.observable("Сливане на населено място");
        self.saveBtnText = ko.observable("Сливане");
        self.saveChanges = function () {
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/admin/MergeCity",
                data: {
                    currentCityId: cityId,
                    mergeCityId: $("#mergeCityModal").find("#citySelect").val(),
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status === "OK") {
                        $("#mergeCityModal").removeClass("in");
                        $("#mergeCityModal").attr("style", "display: none");
                        $("body").removeClass("modal-open");
                        refresh();
                    }
                }
            });
        };

        self.closeBtn = function () {
            $("#mergeCityModal").removeClass("in");
            $("#mergeCityModal").attr("style", "display: none");
            $("body").removeClass("modal-open");
        };
    }
    function AddCityModal() {
        var self = this;
        self.id = ko.observable();
        self.modalCss = ko.observable("modal-success");
        self.modalHeader = ko.observable("Добавяне на населено място");
        self.saveBtnText = ko.observable("Добавяне");
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
                    url: "/admin/CreateCity",
                    data: {
                        name: self.name(),
                        __RequestVerificationToken: token
                    },
                    success: function (data) {
                        if (data.status === "OK") {
                            $("#addCityModal").removeClass("in");
                            $("#addCityModal").attr("style", "display: none");
                            $("body").removeClass("modal-open");
                            refresh();
                        }
                    }
                });
            }
        };

        self.closeBtn = function () {
            $("#addCityModal").removeClass("in");
            $("#addCityModal").attr("style", "display: none");
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
    function switchViews() {
        $("#schools-section").toggleClass("d-none");
        $("#schools-link").toggleClass("active");
        $("#cities-link").toggleClass("active");
        $("#cities-section").toggleClass("d-none");
    }
    function loadCities() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/GetCities",
            success: function (data) {
                if (data.status === "OK") {
                    self.cities.removeAll();
                    for (var i in data.cities) {
                        self.cities.push(data.cities[i]);
                    }
                }
            }
        });
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
    self.cities = ko.observableArray();
    self.schools = ko.observableArray();
    function refresh() {
        loadSchools();
        loadCities();
    }
    
}
var vm = new CityViewModel();
ko.applyBindings(vm);
vm.getSchools();