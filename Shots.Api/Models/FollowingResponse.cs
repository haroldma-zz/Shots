using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shots.Api.Models
{
    public class FollowingResponse : BaseResponse
    {
        public PageInfo PageInfo { get; set; }
        public List<UserInfo> Following { get; set; }
    }
}
