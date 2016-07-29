/// <reference path="E:\CodeFirstApproachDemo\FirebaseAngular\FirebaseAngular\Scripts/angular.js" />

//var feedApp = angular.module('feedDataApp',[]);

//feedApp.controller('feedListController', function ($scope, $firebase) {
//    debugger;
//    var config = {
//        apiKey: "AIzaSyAUnGXHtvn0x8ujYiTp6qre5gIGoo8X99o",
//        authDomain: "angulardemo-fc09d.firebaseapp.com",
//        databaseURL: "https://angulardemo-fc09d.firebaseio.com",
//        storageBucket: "angulardemo-fc09d.appspot.com",
//    };
//    firebase.initializeApp(config);
//    var fbURL = new Firebase("https://angulardemo-fc09d.firebaseio.com");
//    $scope.feedsList = $firebase(fbURL);

//    $scope.save = function () {
//        $scope.feedsList.$add({
//            Name: $scope.feedsList.Name,
//            Url: $scope.feedsList.Url,
//            Description: $scope.feedsList.Description

//        });
//        $(":text").val('');
//    }
//});

var app = angular.module("sampleApp", ["firebase"]);
app.controller("SampleCtrl", function ($scope, $firebaseArray,$window) {
    debugger;
    var ref = new $window.Firebase("https://angulardemo-fc09d.firebaseio.com");
    $scope.feedsList = $firebase(fbURL);
    $scope.save = function () {
        debugger;
        $scope.feedsList.$add({
            Name: $scope.feedsList.Name,
            Url: $scope.feedsList.Url,
            Description: $scope.feedsList.Description

        });
        $(":text").val('');
    }
    // create a synchronized array
    // click on `index.html` above to see it used in the DOM!
    $scope.messages = $firebaseArray(ref);
});