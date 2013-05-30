angular.module('TwitterFilters', [])
   .filter('TwitterLink', function () {
       return function (input) {
           return input.parseUsername().parseHashtag();
       };
   });

String.prototype.parseUsername = function () {
    return this.replace(/[@]+[A-Za-z0-9-_]+/g, function (u) {
        var username = u.replace("@", "");
        return u.link("//twitter.com/" + username);
    });
};

String.prototype.parseHashtag = function () {
    return this.replace(/[#]+[A-Za-z0-9-_]+/g, function (h) {
        var hashTag = h.replace("#", "");
        return h.link("//twitter.com/search?src=hash&q=%23" + hashTag);
    });
};