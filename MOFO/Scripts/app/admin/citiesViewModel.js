function CitiesViewModel() {
    var self = this;
    self.cityName = ko.observable();
    self.clearForm = function () {
        $("#filterSelect").select2("val", "0");
        self.cityName("");
    };
    function refresh() {
        vm.search();
    }
    self.search = function () {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/SearchCities",
            data: {
                cityName: self.cityName(),
                status: $("#filterSelect").val()
            },
            success: function (data) {
                if (data.status === "OK") {
                    self.cities.removeAll();
                    for (var i in data.cities) {
                        self.cities.push(data.cities[i]);
                    }
                }
            }
        });
    };
    self.cities = ko.observableArray();
    $("#filterSelect").select2();
    self.createNewCity = function () {
        var vm = new AddCityModal();
        self.addCityModal(vm);
        $("#addCityModal").addClass("in");
        $("#addCityModal").attr("style", "display: block");
        $("#addCityModal").scrollTop(0);
        $("body").addClass("modal-open");
    };
    self.addCityModal = ko.observable();
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
                        status: 0
                    };
                    return query;
                }
            },
            placeholder: "Населено място"
        });
        $('#mergeSelect').select2({
            ajax: {
                url: window.location.protocol + "//" + window.location.host + "/admin/searchCitiesModal",
                data: function (params) {
                    var query = {
                        query: params.term,
                        status: 2,
                        exclusionId: $("#mergeCityModal").find("#citySelect").val()
                    };
                    return query;
                }
            },
            placeholder: "Населено място"
        });
        $('#mergeSelect').prop('disabled', true);
        $('#citySelect').on('select2:select', function (e) {
            $('#mergeSelect').prop('disabled', false);
        });
    };
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
                    currentCityId: $("#mergeCityModal").find("#mergeSelect").val(),
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
}
var vm = new CitiesViewModel();
vm.search();
ko.applyBindings(vm);