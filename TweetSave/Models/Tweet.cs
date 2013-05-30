using Newtonsoft.Json;

namespace TweetSave.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }

        [JsonProperty(PropertyName = "from_user_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "from_user")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "profile_image_url")]
        public string ImageUrl { get; set; }
    }
}