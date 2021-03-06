define(['jquery', 'knockout'], function ($, ko) {

    // VM definition
    function PageViewModel() {
        var self = this;
        if (!(this instanceof PageViewModel)) {
            throw new Error("Please instantiate this ViewModel by using new PageViewModel() to a var.");
        }
        self.firstName = "Parveen";
        self.lastName = "Kumar";
        self.Clock = ko.observable(new Date().toLocaleString());

        var clock;
        var stopClock = function () {
            if (clock) {
                clearInterval(clock);
            }
        }
        var startClock = function () {
            clock = setInterval(function () {
                var time = new Date().toLocaleString();
                console.log("time changed to " + time);
                self.Clock(time); // observable has to be updated as shown.
            }, 1000);
        }(); // ### NOTE: () will invoke startclock when VM is being built... 

        self.startClock = startClock;
        self.stopClock = stopClock;
    }

    // Page Init
    var go = function (options) {
        // "options" set by View when it loads and is a Json object of the Model
        // options: lets you load/setup/configure any initial User-iteraction components on page before other data is loaded.

        console.log("## Page go fired....")
        var vm = new PageViewModel();
        ko.applyBindings(vm, $("viewroot")[0]); //applyBindingsSafe
    }
    return { go: go }
});