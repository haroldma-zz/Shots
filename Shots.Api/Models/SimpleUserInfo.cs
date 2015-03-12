using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using Shots.Api.Utilities;

namespace Shots.Api.Models
{
    public enum FriendState
    {
        None,
        Private,
        Added,
        Requested
    }

    public class SimpleUserInfo : NotifyPropertyChangedObject
    {
        private bool _isFriend;
        private bool _isRequested;

        [JsonIgnore]
        public FriendState FriendState
        {
            get
            {
                if (IsRequested) return FriendState.Requested;
                return IsFriend ? FriendState.Added : (Privacy ? FriendState.Private : FriendState.None);
            }
        }

        public string Bio { get; set; }
        public int Color { get; set; }

        [JsonIgnore]
        public SolidColorBrush ColorBrush
        {
            get { return ShotsColor.GetColorById(Color).Brush; }
        }

        [JsonProperty("fname")]
        public string FirstName { get; set; }

        public string Id { get; set; }

        public bool IsFriend
        {
            get { return _isFriend; }
            set
            {
                _isFriend = value;
                OnPropertyChanged("FriendState");
            }
        }

        public bool IsRequested
        {
            get { return _isRequested; }
            set
            {
                _isRequested = value;
                OnPropertyChanged("FriendState");
            }
        }

        [JsonProperty("lname")]
        public string LastName { get; set; }

        public string Place { get; set; }
        public bool Privacy { get; set; }

        [JsonProperty("profile_photo_small")]
        public string ProfilePhotoSmall { get; set; }

        public bool Requested { get; set; }
        public string Username { get; set; }
        public string Website { get; set; }
    }
}