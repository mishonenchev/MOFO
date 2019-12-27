function CitiesViewModel() {
    var self = this;
    self.cityName = ko.observable();
    self.clearForm = function () {
        $("#filterSelect").select2("val", "0");
        self.cityName("");
    };
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
                if (data.status == "OK") {
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
}
var vm = new CitiesViewModel();
ko.applyBindings(vm);