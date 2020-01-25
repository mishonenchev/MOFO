function indexViewModel() {
    var self = this;
    self.activeSession = ko.observable();
    self.sessions = ko.observableArray();
    self.loadContent = function ()
    {
        $.ajax({
            type: "GET",
            url: "/user/getindexcontent",
            dataType: "json",
            success: function (data) {
                if (data.status == "OK") {
                    self.activeSession(data.activeSession);
                    self.sessions(data.sessions);
                }
            }
        });

    };
    self.openActiveSession = function () {
        window.location = "/user/activeSession";
    }
}
var vm = new indexViewModel();
vm.loadContent();
setInterval(vm.loadContent, 2000);
ko.applyBindings(vm);