var requirejsConfig = {
    baseUrl: "/Scripts",
    enforceDefine: true,
    paths: { // NOTE : ensure comma after each element
        "mvcappStartup": "mvcappStartup",
        "jqueryUV": "jquery.validate.unobtrusive",
        "jquery": "jquery-2.1.4",
        "jqueryV": "jquery.validate",
        "modernizr": "modernizr-2.8.3",
        "bootstrap": "bootstrap.min",
        "respond": "respond.min",
        "respondMA": "respond.matchmedia.addlistener.min",
        "require": "require",
        "r": "r",
        "knockout": "knockout-3.3.0",
        "knockoutV": "knockout.validation",
        "underscore": "underscore"
    }
};

require.config(requirejsConfig);
requirejs.config(requirejsConfig);

require(['mvcappStartup'], function(startup) {});

console.log("requirejs configured");
