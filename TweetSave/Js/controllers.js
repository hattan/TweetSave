function TweetCtrl($scope, $http, TweetSave) {
    $scope.doSearch = function (term) {
        var url = 'http://search.twitter.com/search.json?callback=JSON_CALLBACK&q=' + term;

        $http.jsonp(url)
             .success(function (data) {
                 $scope.results = data.results;
             });
    };

    $scope.saveTweet = function (tweet) {
        var savedTweet = new TweetSave();
        angular.copy(tweet, savedTweet);
        savedTweet.$save(function () {
            $scope.message = "Tweet Saved!";
        });
    };
}

function SavedTweetCtrl($scope, TweetSave) {
    $scope.savedTweets = TweetSave.query();

    $scope.deleteTweet = function (tweet) {
        tweet.$delete(function () {
            $scope.message = "Tweet Deleted!";
            $scope.savedTweets = TweetSave.query();
        });
    };
}