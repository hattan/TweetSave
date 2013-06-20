function TweetCtrl($scope, $http, TweetSave) {
    


    $scope.saveTweet = function (tweet) {
        var savedTweet = new TweetSave();
        savedTweet.from_user_name = tweet.user.screen_name;
        savedTweet.text = tweet.text;
        savedTweet.from_user = tweet.user.name;
        savedTweet.profile_image_url = tweet.user.profile_image_url;
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