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
function openModal(selector) {
    $(selector).addClass("in");
    $(selector).attr("style", "display: block");
    $("body").addClass("modal-open");
}
function closeModal(selector) {
    $(selector).removeClass("in");
    $(selector).attr("style", "display: none");
    $("body").removeClass("modal-open");
}
ko.bindingHandlers.numeric = {
    init: function (element, valueAccessor) {
        $(element).on("keydown", function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: . ,
                (event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
    }
};