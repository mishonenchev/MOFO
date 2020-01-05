function CardsViewModel() {
    var self = this;
    self.refNumber = ko.observable();
    self.search = function () {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/SearchCards",
            data: {
                refNumber: self.refNumber(),
                cityId: $("#citySelect").val(),
                schoolId: $("#schoolSelect").val(),
                roomId: $("#roomSelect").val()
            },
            success: function (data) {
                if (data.status == "OK") {
                    self.cards.removeAll();
                    for (var i in data.items) {
                        self.cards.push(data.items[i]);
                    }
                }
            }
        });
    }
    self.openActivateCardModal = function () {
        self.activateCardModal(new ActivateCardModal());
        openModal("#activateCardModal");
    }
    self.printCards = function () {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/GeneratePdf",
            data: {
                refNumber: self.refNumber(),
                cityId: $("#citySelect").val(),
                schoolId: $("#schoolSelect").val(),
                roomId: $("#roomSelect").val()
            },
            success: function (data) {
                if (data.status == "OK") {
                    window.open(window.location.protocol + "//" + window.location.host + "/admin/getFile?path=" + data.fName);
                }
            }
        });
    }
    self.licenseRenewModal = function () {
        var lvm = new LicenseModal({ refNumber: self.refNumber(), cityId: $("#citySelect").val(), schoolId: $("#schoolSelect").val(), roomId: $("#roomSelect").val() });
        self.licenseModal(lvm);
        openModal("#licenseModal");
        // open license modal
    }
    self.createCardModal = function () {
        self.newCardModal(new CreateCardModal());
        SelectInitCardModal();
        openModal("#newCardModal");
    }
    self.clearForm = function () {
        $("#citySelect").select2("val", "0");
        $("#schoolSelect").select2("val", "0");
        $("#roomSelect").select2("val", "0");
        $('#schoolSelect').prop('disabled', true);
        $('#roomSelect').prop('disabled', true);
        self.refNumber("");
    }
    self.cards = ko.observableArray();
    self.newCardModal = ko.observable();
    self.activateCardModal = ko.observable();
    self.licenseModal = ko.observable();
    self.deleteCard = function () {

    }
    self.activateCard = function (item) {
        var avm = new ActivateCardModal();
        avm.refNumber(item.refNumber);
        self.activateCardModal(avm);
        openModal("#activateCardModal");

    }

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
    $('#schoolSelect').select2({
        ajax: {
            url: window.location.protocol + "//" + window.location.host + "/admin/searchSchoolSelect",
            data: function (params) {
                var query = {
                    query: params.term,
                    cityId: $('#citySelect').val()
                };
                return query;
            }
        },
        placeholder: "Училище"
    });
    $('#schoolSelect').prop('disabled', true);
    $('#roomSelect').select2({
        ajax: {
            url: window.location.protocol + "//" + window.location.host + "/admin/searchRoomSelect",
            data: function (params) {
                var query = {
                    query: params.term,
                    schoolId: $('#schoolSelect').val()
                };
                return query;
            }
        },
        placeholder: "Стая"
    });
    $('#roomSelect').prop('disabled', true);
    $('#citySelect').on('select2:select', function (e) {
        $('#schoolSelect').prop('disabled', false);
        $("#schoolSelect").select2("val", "0");
        $("#roomSelect").select2("val", "0");
    });
    $('#schoolSelect').on('select2:select', function (e) {
        $('#roomSelect').prop('disabled', false);
        $("#roomSelect").select2("val", "0");
    });

    function CreateCardModal() {
        var self = this;
        self.cardCount = ko.observable().extend({ addSubObservables: true }); 
        self.validationProperty = ko.observable().extend({ addSubObservables: true }); 
        self.generateCards = function () {
            if ($('#citySelectModal').val() != null && $('#schoolSelectModal').val() != null && $('#roomSelectModal').val() != null && $("#licenseSelect").children(":selected").attr("id") != "0" && self.cardCount() != undefined) {
                var form = $('#__AjaxAntiForgeryForm');
                var token = $('input[name="__RequestVerificationToken"]', form).val();
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/admin/CreateCards",
                    data: {
                        roomId: $('#roomSelectModal').val(),
                        license: $("#licenseSelect").children(":selected").attr("id"),
                        cardCount: self.cardCount(),
                        __RequestVerificationToken: token
                    },
                    success: function (data) {
                        if (data.status == "OK") {
                            closeModal("#newCardModal");
                            loadCards();
                        }
                    }
                });
            } else {
                invalid(self.validationProperty, "Всички полета са задължителни!");
            }

            openModal("#newCardModal");
        }
        self.closeBtn = function () {
            closeModal("#newCardModal");
        }
        
    }
    function ActivateCardModal() {
        var self = this;
        var ignoreOnce = false;
        self.refNumber = ko.observable().extend({ addSubObservables: true });;
        self.code = ko.observable().extend({ addSubObservables: true });
        self.saveBtn = function () {
            self.refNumber.validate();
            self.code.validate();
            if (!self.refNumber.isInvalid() && !self.code.isInvalid()) {
                var form = $('#__AjaxAntiForgeryForm');
                var token = $('input[name="__RequestVerificationToken"]', form).val();
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/admin/ActivateCard",
                    data: {
                        refNumber: self.refNumber(),
                        code: self.code(),
                        __RequestVerificationToken: token
                    },
                    success: function (data) {
                        if (data.status == "OK") {
                            ignoreOnce = true;
                            self.refNumber("");
                            ignoreOnce = true;
                            self.code("");
                            loadCards();
                        }
                    }
                });
            }
        }
        self.refNumber.subscribe(function () {
            if (!ignoreOnce) {
                self.refNumber($.trim(self.refNumber()));
                if (self.refNumber() != undefined && self.refNumber() != '') {
                    $.ajax({
                        type: "GET",
                        dataType: "json",
                        contentType: "application/json",
                        url: "/admin/ValidateRefNumber",
                        data: {
                            refNumber: self.refNumber()
                        },
                        success: function (data) {
                            if (data.status == "OK") {
                                valid(self.refNumber);
                            } else {
                                invalid(self.refNumber, "Невалиден референтен номер!");
                            }
                        }
                    })
                } else {
                    invalid(self.refNumber, "Референтният номер е задължинелно поле!");
                }
            } else {
                ignoreOnce = false;
            }
            });
    
        self.code.subscribe(function () {
            if (!ignoreOnce) {
                self.code($.trim(self.code()));
                if (self.code() != undefined && self.code() != '') {
                    $.ajax({
                        type: "GET",
                        dataType: "json",
                        contentType: "application/json",
                        url: "/admin/ValidateCardCode",
                        data: {
                            code: self.code()
                        },
                        success: function (data) {
                            if (data.status == "OK") {
                                valid(self.code);
                            } else {
                                invalid(self.code, "Този RFID код вече е използван!");
                            }
                        }
                    });
                } else {
                    invalid(self.code, "RFID кодът е задължинелно поле!");
                }
            } else {
                ignoreOnce = false;
            }
        });
        self.closeBtn = function () {
            closeModal("#activateCardModal");
        }
    }
    function LicenseModal(filter) {
        var self = this;
        self.filter = filter;
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        self.endLicense = function () {

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/admin/EndCardLicense",
                data: {
                    roomId: filter.roomId,
                    cityId: filter.cityId,
                    schoolId: filter.schoolId,
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status == "OK") {
                        closeModal("#licenseModal");
                        loadCards();
                    }
                }
            });
        }
        self.saveBtn = function () {
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/admin/UpdateCardLicense",
                data: {
                    roomId: filter.roomId,
                    cityId: filter.cityId,
                    schoolId: filter.schoolId,
                    period: $("#licenseModalSelect").children(":selected").attr("id"),
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status == "OK") {
                        closeModal("#licenseModal");
                        loadCards();
                    }
                }
            });
        }
        self.closeBtn = function () {
            closeModal("#licenseModal");
        }
    }
    function SelectInitCardModal() {
        $('#citySelectModal').select2({
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
        $('#schoolSelectModal').select2({
            ajax: {
                url: window.location.protocol + "//" + window.location.host + "/admin/searchSchoolSelect",
                data: function (params) {
                    var query = {
                        query: params.term,
                        cityId: $('#citySelectModal').val()
                    };
                    return query;
                }
            },
            placeholder: "Училище"
        });
        $('#schoolSelectModal').prop('disabled', true);
        $('#roomSelectModal').select2({
            ajax: {
                url: window.location.protocol + "//" + window.location.host + "/admin/searchRoomSelect",
                data: function (params) {
                    var query = {
                        query: params.term,
                        schoolId: $('#schoolSelectModal').val()
                    };
                    return query;
                }
            },
            placeholder: "Стая"
        });
        $('#roomSelectModal').prop('disabled', true);
        $('#citySelectModal').on('select2:select', function (e) {
            $('#schoolSelectModal').prop('disabled', false);
            $("#schoolSelectModal").select2("val", "0");
            $("#roomSelectModal").select2("val", "0");
        });
        $('#schoolSelectModal').on('select2:select', function (e) {
            $('#roomSelectModal').prop('disabled', false);
            $("#roomSelectModal").select2("val", "0");
        });
    }
    function loadCards() {
        self.search();
    }
}
var vm = new CardsViewModel();
ko.applyBindings(vm);