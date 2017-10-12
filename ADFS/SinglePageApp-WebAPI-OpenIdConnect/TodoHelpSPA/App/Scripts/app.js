﻿'use strict';
angular.module('todoApp', ['ngRoute','AdalAngular'])
    //http://slopjong.de/2015/09/01/set-global-constants-and-variables-in-angularjs/
    .constant('config', {
        apiURL: "https://localhost:44321/"
    })

    .config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/TodoList", {
        controller: "todoListCtrl",
        templateUrl: "/App/Views/TodoList.html",
        requireADLogin: true,
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html",
    }).otherwise({ redirectTo: "/Home" });
    
    Logging = {
        level: 3,
        log: function (message) {
            console.log(message);
        }
    };

    adalProvider.init(
        {
            instance: 'https://adfs.lordchinzilla.com/', 
            tenant: 'adfs',
            clientId: 'a5fceeb9-80cf-4bc5-b34f-840006dc4819',
            extraQueryParameter: 'nux=1',
            //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
            endpoints: { "https://todolistapi.lordchinzilla.com": "https://localhost:44321/" }
        },
        $httpProvider
        );
   
}]);
