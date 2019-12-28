function SchoolViewModel() {
    var self = this;
    self.getInfo = function () {
        switchViews();
        loadInfo();
    }
    self.getRooms = function () {
        switchViews();
        loadRooms();
       
    }
    self.openRoomModal = function (item) {
        if (item === false) {
            var cvm = new RoomEditModal(false);
            self.roomEditModal(cvm);
            

        } else {
            var evm = new RoomEditModal(true);
            evm.id(item.id);
            evm.name(item.name);
            evm.description(item.description);
            self.roomEditModal(evm);
        }
        $("#roomEditModal").addClass("in");
        $("#roomEditModal").attr("style", "display: block");
        $("#roomEditModal").scrollTop(0);
        $("body").addClass("modal-open");
    }
    self.openConfirmModal = function () {
        var vm = new ConfirmSchoolModal();
        self.confirmSchoolModal(vm);
        $("#confirmRoomModal").addClass("in");
        $("#confirmRoomModal").attr("style", "display: block");
        $("#confirmRoomModal").scrollTop(0);
        $("body").addClass("modal-open");
    }
    self.createNewRoom = function () {
        self.openRoomModal(false);
    }
    self.rooms = ko.observableArray();
    self.openInfoEdit = function () {

    }
    self.infos = ko.observableArray();
    self.roomEditModal = ko.observable();
    self.infoEditModal = ko.observable();
    self.confirmSchoolModal = ko.observable();
    function RoomEditModal(isEdit) {
        var self = this;
        self.id = ko.observable();
        self.modalCss = ko.observable();
        self.modalHeader = ko.observable();
        self.saveBtnText = ko.observable();
        self.isEdit = ko.observable(isEdit);
        self.checkView = function () {
            self.name.validate();
            self.description.validate();
            return !self.name.isInvalid() && !self.description.isInvalid();
        };  
        self.saveChanges = function () {
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            if (isEdit) {
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/admin/EditRoom",
                    data: {
                        name: self.name(),
                        description: self.description(),
                        roomId: self.id(),
                        __RequestVerificationToken: token
                    },
                    success: function (data) {
                        if (data.status == "OK") {
                            $("#roomEditModal").removeClass("in");
                            $("#roomEditModal").attr("style", "display: none");
                            $("body").removeClass("modal-open");
                            refreshRooms();
                        }
                    }
                });
            } else {
                if (self.checkView() == true) {
                    $.ajax({
                        type: "POST",
                        dataType: "json",
                        url: "/admin/CreateRoom",
                        data: {
                            name: self.name(),
                            description: self.description(),
                            schoolId: schoolId,
                            __RequestVerificationToken: token
                        },
                        success: function (data) {
                            if (data.status == "OK") {
                                $("#roomEditModal").removeClass("in");
                                $("#roomEditModal").attr("style", "display: none");
                                $("body").removeClass("modal-open");
                                refreshRooms();
                            }
                        }
                    });
                }
            }
        }
        self.closeBtn = function () {
            $("#roomEditModal").removeClass("in");
            $("#roomEditModal").attr("style", "display: none");
            $("body").removeClass("modal-open");
        }
        self.name = ko.observable().extend({ addSubObservables: true });
        self.description = ko.observable().extend({ addSubObservables: true });
        if (isEdit) {
            self.modalCss("modal-warning");
            self.modalHeader("Редакция на клас");
            self.saveBtnText("Запази");
        } else {
            self.modalCss("modal-success");
            self.modalHeader("Добавяне на клас");
            self.saveBtnText("Добавяне");
        }
        self.deleteClass = function () {
            //open modal for confirmation
        }
        self.name.subscribe(function () {
            self.name($.trim(self.name()));
            if (self.name() == undefined || self.name() == '') {
                invalid(self.name, "Името на стаята е задължително поле.")
            } else if (self.name().length > 130) {
                invalid(self.name, "Твърде много символи.")
            } else {
                valid(self.name);
            }
        })
        self.description.subscribe(function () {
            self.description($.trim(self.description()));
            if (self.description().length > 130) {
                invalid(self.description, "Твърде много символи.")
            } else {
                valid(self.description);
            }
        })
        
    }
    function ConfirmSchoolModal() {
        var self = this;
        self.schoolName = ko.observable(schoolName);
        self.confirmSchool = function () {
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/admin/ConfirmSchool",
                data: {
                    schoolId: schoolId,
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status == "OK") {
                        $("#confirmRoomModal").removeClass("in");
                        $("#confirmRoomModal").attr("style", "display: none");
                        $("body").removeClass("modal-open");
                        location.reload();
                    }
                }
            });
        }
        self.closeBtn = function () {
            $("#confirmRoomModal").removeClass("in");
            $("#confirmRoomModal").attr("style", "display: none");
            $("body").removeClass("modal-open");
        }
    }
    function InfoEditModal() {

    }
    function switchViews() {
        $("#room-section").toggleClass("d-none");
        $("#room-link").toggleClass("active");
        $("#info-link").toggleClass("active");
        $("#info-section").toggleClass("d-none");
    }
    function loadRooms() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/GetSchoolRooms",
            data: {
                schoolId: schoolId
            },
            success: function (data) {
                if (data.status == "OK") {
                    self.rooms.removeAll();
                    for (var i in data.rooms) {
                        self.rooms.push(data.rooms[i]);
                    }
                }
            }
        });
    }
    function loadInfo() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/GetSchoolInfo",
            data: {
                schoolId: schoolId
            },
            success: function (data) {
                if (data.status == "OK") {
                    self.infos.removeAll();
                    for (var i in data.items) {
                        self.infos.push(data.items[i]);
                    }
                }
            }
        });
    }
    function refreshRooms() {
        loadRooms();
    }
   
}
var vm = new SchoolViewModel();
ko.applyBindings(vm);
vm.getRooms();