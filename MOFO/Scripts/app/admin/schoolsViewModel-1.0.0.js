function SchoolsViewModel() {
    var self = this;
    self.schoolName = ko.observable();
    self.clearForm = function () {
        $("#citySelect").select2("val", "0");
        $("#filterSelect").select2("val", "0");
        self.schoolName("");
    };
    self.search = function () {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            url: "/admin/SearchSchools",
            data: {
                schoolName: self.schoolName(),
                cityId: $("#citySelect").val(),
                status: $("#filterSelect").val()
            },
            success: function (data) {
                if (data.status == "OK") {
                    self.schools.removeAll();
                    for (var i in data.schools) {
                        self.schools.push(data.schools[i]);                       
                    }
                }
            }
        });
    }
    self.schools = ko.observableArray();

    $("#citySelect").select2({
        ajax: {
            url: window.location.protocol + "//" + window.location.host + "/account/searchCity",
            data: function (params) {
                var query = {
                    query: params.term
                }
                return query;
            }
        }
    });
    $("#filterSelect").select2();
}
var vm = new SchoolsViewModel();
ko.applyBindings(vm);