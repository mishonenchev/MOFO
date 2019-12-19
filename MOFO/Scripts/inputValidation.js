ko.extenders.addSubObservables = function (target) {
    var initialFocus = false;
    //add some sub-observables to our observable
    target.isInvalid = ko.observable(true);
    target.validClass = ko.observable();
    target.message = ko.observable();
    target.isDisabled = ko.observable(false);
    target.isFocused = ko.observable();

    target.isFocused.subscribe(function () {
        if (target.isFocused() == false && initialFocus) {
            if (target() == undefined) {
                target(" ");
            }
        } else {
            initialFocus = true;
        }
    })
    target.validate = function () {
        if (target() == undefined) {
            target(" ");
        } else {
            target(target() + " ");
        }
    }
    //return the original observable
    return target;
};
function invalid(target, message) {
    target.validClass("is-invalid");
    target.isInvalid(true);
    target.message(message);
}
function valid(target) {
    target.validClass("is-valid");
    target.isInvalid(false);
    target.message("");
}