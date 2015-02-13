using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class SuggestedResponse : BaseResponse
    {
        public List<SimpleUserInfo> Suggested { get; set; }
    }
}