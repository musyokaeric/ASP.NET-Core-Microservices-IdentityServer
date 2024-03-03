namespace Movies.Client.Models
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(Dictionary<string, string> userInfoDictionary)
        {
            UserInfoDictionary = userInfoDictionary;
        }

        public Dictionary<string, string> UserInfoDictionary { get; private set; }
    }
}
