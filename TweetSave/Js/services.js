angular.module("TweetSaveService", ["ngResource"])
   .factory("TweetSave", function ($resource) {
       return $resource(
          "/api/tweet/:Id",
          { Id: "@TweetId" },
          { "update": { method: "PUT" } }
      );
   });