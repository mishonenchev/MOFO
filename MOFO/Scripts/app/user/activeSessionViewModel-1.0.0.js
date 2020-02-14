
function ActiveSessionViewModel() {
    var self = this;
    self.messages = ko.observableArray();
    self.usersCount = ko.observable('-');
    self.usersCountLong = ko.observable('потребители');
    self.users = ko.observableArray();
    var lastMessage = 0;
    var lastRequestDone = true;
    self.sync = function () {
        if (lastRequestDone) {
            $.ajax({
                type: "GET",
                url: "/user/ActiveSessionSync?lastMessage=" + lastMessage,
                dataType: "json",
                success: function (data) {
                    if (data.status == "OK") {
                        for (var i in data.messages) {
                            if (data.messages[i].message != undefined) {
                                data.messages[i].message = urlify(data.messages[i].message);
                            }
                            if (data.messages[i].fileName != undefined) {
                                data.messages[i].fileExtension = data.messages[i].fileName.split('.')[1];
                                data.messages[i].isImage = data.messages[i].fileExtension == "jpg" ? true : (data.messages[i].fileExtension == "png" ? true : false);
                            }
                            if (data.messages[i].isImage) {
                                data.messages[i].imageSrc = "/content/files/" + data.messages[i].fileName;
                            }
                            self.messages.unshift(data.messages[i]);
                            if (data.messages[i].id > lastMessage) {
                                lastMessage = data.messages[i].id;
                            }
                        }
                        self.usersCount(data.usersCount);
                        if (data.usersCount == 1) {
                            self.usersCountLong("потребител");
                        } else {
                            self.usersCountLong("потребители");
                        }
                        lastRequestDone = true;

                    } else if (data.status == "NO SESSION") {
                        window.location = "/";
                        self.usersCount("-");
                        self.usersCountLong("потребители");
                    }
                },
                error: function () {
                    alert("Няма връзка с интернет");
                    lastRequestDone = false;
                    self.usersCount("-");
                    self.usersCountLong("потребители");
                }
            });
            lastRequestDone = false;
        }
    }
    function urlify(text) {
        var urlRegex = /(https?:\/\/[^\s]+)/g;
        return text.replace(urlRegex, function (url) {
            return '<a href="' + url + '">' + url + '</a>';
        })
    }
    self.submitDisabled = ko.observable(true);
    self.message = ko.observable("");
    self.message.subscribe(function () {
        if (self.message() != undefined) {
            if (self.message().length > 0) {
                self.submitDisabled(false);

            } else {
                self.submitDisabled(true);
                if (self.hasFile() == true) {
                    self.submitDisabled(false);
                }
               
            }
        }
    });
    self.uploadFile = function () {
        var formData = new FormData();
        if ($(".inputfile")[0].files.length > 0) {
            formData.append($(".inputfile")[0].files[0].name, $(".inputfile")[0].files[0]);
        }
            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
        formData.append("__RequestVerificationToken", token);
        formData.append("message", self.message());

            $.ajax({
                url: "/file/fileUpload",
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: formData,
                beforeSend: function () {
                    $("#sendBtn").append('<i class="loader fas fa-spinner"></i>');
                    self.submitDisabled(true);
                    self.hasFile(false);
                    self.message("");
                    $(".inputfile").val('');
                    $("#filelabel").text("Избери файл ...");
                },
                success: function () {
                    $("#sendBtn").append('Изпрати');
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
    }
    self.ShowActiveUsers = function () {
        $.ajax({
            type: "GET",
            url: "/user/activeUsers",
            dataType: "json",
            success: function (data) {
                if (data.status == "OK") {

                    self.users(data.users);
                    openModal("#activeUsersView");

                } else {
                    window.location = "/";
                    self.usersCount("-");
                    self.usersCountLong("потребители");
                }
            },
            error: function () {
                alert("Няма връзка с интернет");
                lastRequestDone = false;
                self.usersCount("-");
                self.usersCountLong("потребители");
            }
        });
    }
    self.ShowLeaveModal = function () {
        openModal("#leaveRoomView");
    }
    self.leaveRoom = function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/room/LogoutDesktop",
                data: {
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    if (data.status === "OK") {
                        window.location = '/user/activeSession';
                    }
                }
            });
    };
    self.hasFile = ko.observable(false);
    self.downloadFile = function (item) {
        window.location = "/user/downloadfile?downloadCode=" + item.downloadCode;
    }
}
var vm = new ActiveSessionViewModel();
vm.sync();
setInterval(vm.sync, 3000);
ko.applyBindings(vm);

(function fileInput() {
    var inputs = document.querySelectorAll('.inputfile');
    Array.prototype.forEach.call(inputs, function (input) {
        var label = input.nextElementSibling,
            labelVal = label.innerHTML;

        input.addEventListener('change', function (e) {
            var fileName = '';
            fileName = e.target.value.split('\\').pop();
            if (fileName) {
                label.innerHTML = fileName;
                vm.hasFile(true);
                vm.submitDisabled(false);
            }
            else {
                label.innerHTML = labelVal;
            }
        });
    });
})();