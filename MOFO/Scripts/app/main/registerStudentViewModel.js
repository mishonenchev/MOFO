
function RegisterStudentViewModel() {
    var self = this;
    self.name = ko.observable().extend({ addSubObservables: true });
    self.email = ko.observable().extend({ addSubObservables: true });
    self.telephone = ko.observable().extend({ addSubObservables: true });
    self.password = ko.observable().extend({ addSubObservables: true });
    self.confirmPassword = ko.observable().extend({ addSubObservables: true });
    self.submitEnable = ko.observable(true);

    self.name.subscribe(function () {
        self.name($.trim(self.name()));
        if (self.name() == undefined || self.name() == '') {
            invalid(self.name, "Името е задължително поле.")
        } else if (!/^[a-zA-ZА-я\s]+$/.test(self.name())) {
            invalid(self.name, "Името трябва да съдържа само букви.")
        } else if (self.name().length > 100) {
            invalid(self.name, "Твърде много символи.")
        } else {
            valid(self.name);
        }

    })
    self.telephone.subscribe(function () {
        self.telephone($.trim(self.telephone()));
        if (self.telephone() == undefined || self.telephone() == '') {
            invalid(self.telephone, "Телефонът е задължително поле.")
        } else {
            $.ajax({
                type: "GET",
                url: "/account/telephoneValidation",
                dataType: "json",
                data: {
                    telephone: self.telephone()
                },
                success: function (data) {
                    if (data.status == "OK") {
                        if (!data.isValid) {
                            invalid(self.telephone, "Телефонът е невалиден.")
                        } else {
                            valid(self.telephone);
                        }
                    }
                }
            });
        }
    });
    self.email.subscribe(function () {
        self.email($.trim(self.email()));
        if (self.email() == undefined || self.email() == '') {
            invalid(self.email, "Имейлът е задължително поле.")
        } else if (!/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(self.email())) {
            invalid(self.email, "Имейлът е невалиден.")
        } else {
            $.ajax({
                type: "GET",
                url: "/account/emailValidation",
                dataType: "json",
                data: {
                    email: self.email()
                },
                success: function (data) {
                    if (data.status == "OK") {
                        if (data.isValid) {
                            valid(self.email);
                        } else {
                            invalid(self.email, "Вече съществува потребител с такъв имейл.")
                        }
                    }
                }
            });
        }
    });
    self.password.subscribe(function () {
        if (self.password().length > 5 && self.password.length < 400) {
            valid(self.password);
        } else {
            invalid(self.password, "Паролата е твърде кратка");
        }
    })
    self.confirmPassword.subscribe(function () {
        if (self.password() == self.confirmPassword()) {
            valid(self.confirmPassword);
        } else {
            invalid(self.confirmPassword, "Паролите не съвпадат");
        }
    })
    self.checkView = function () {
        self.firstName.validate();
        self.lastName.validate();
        self.telephone.validate();
        self.email.validate();
        if (self.password().length > 5 && self.password.length < 400) {
            valid(self.password);
        } else {
            invalid(self.password, "Паролата е твърде кратка");
        }
        if (self.password() == self.confirmPassword()) {
            valid(self.confirmPassword);
        } else {
            invalid(self.confirmPassword, "Паролите не съвпадат");
        }

        return !self.firstName.isInvalid() && !self.lastName.isInvalid() && !self.telephone.isInvalid() && !self.password.isInvalid() && !self.confirmPassword.isInvalid() && !self.email.isInvalid()

    }
    self.validFields = ko.observableArray();

}
var vm = new RegisterStudentViewModel();
ko.applyBindings(vm);