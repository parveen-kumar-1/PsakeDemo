define(['jquery', 'knockout', 'underscore'], function ($, ko, _) {

    // VM definition
    function PageViewModel() {
        var self = this;
        if (!(this instanceof PageViewModel)) {
            throw new Error("Please instantiate this ViewModel by using new PageViewModel() to a var.");
        }
        self.Persons = ko.observableArray([]);

        self.PersonTemplate = ko.observable('');

        self.StatesList = ['Northamptonshire', 'Hertfordshire', 'Ayrshire'];

        var newPerson = null;
        var persons = [];
        // not fully coded yet
        self.GetPerson = function(p) {
            $.ajax({
                type: "GET",
                url: "/Home/Create",
                success: function(data, status, jqXHR) {
                    
                },
                error: function(jqXHR, status, errorString) {
                }
            });
        };

        self.NewPerson = function() {
            newPerson = new PersonViewModel();
            var arr = self.Persons();

            newPerson.Container = arr;
            var addresses = [];
            var addr = _(arr[0].Addresses[0]).clone();
            addr.AddressId = 0;
            addr.AddressLine1 = "";
            addr.City = "";
            addr.State = ko.observableArray(self.StatesList);
            addresses.push(addr);

            newPerson.Addresses = addresses;

            self.PersonTemplate(newPerson);
        };

        self.SavePerson = function() {
            // not getting text values back from form
            // value from Click was coming black coz my textbox was bound to "text" instead of "value" property.
            var perobj = { Id: newPerson.Id(), Name: newPerson.Name() };

            persons = self.Persons();
            persons.push(perobj);
            self.Persons(persons);

            self.PersonTemplate('');
        }; //.bind(this);
    }

    function PersonViewModel() {
        var self = this;

        self.Id = ko.observable('');
        self.Name = ko.observable('');
        self.Addresses = [];

        self.Container = [];

    };

// Page Init
    var go = function (options) {
        var vm = new PageViewModel();
        vm.Persons(options); // Here Persons are being set as list of Persons
        ko.applyBindings(vm, $("viewroot")[0]); //applyBindingsSafe
    }
    return { go: go }
});
