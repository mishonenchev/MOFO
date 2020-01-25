using Microsoft.AspNet.SignalR;
using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace MOFO.Hubs
{
    public class FeedHub :Hub
    {
        private readonly IUserService _userService;
        public FeedHub(IUserService userService )
        {
            _userService = userService;
        }
        public string JoinRoom(string authkey=null)
        {
            User user = null;
            if (!String.IsNullOrWhiteSpace(authkey))
            {
                user = _userService.GetUserByAuth(authkey);
            }
            else
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    user = _userService.GetUserByUserId(Context.User.Identity.GetUserId());
                }
            }
            if (user != null)
            {
                if (user.Session != null)
                {
                    var sessionId = user.Session.Id;
                    Groups.Add(Context.ConnectionId, sessionId.ToString());
                }
            }
            return null;
            
        }
        public void EvokeSync(string authkey)
        {
           // Clients.All.feedSync();
        }
        public void LeaveRoom()
        {
           // Clients.Client().stopClient();
        }
        public override Task OnDisconnected(bool stopCalled)
        { 
            return base.OnDisconnected(stopCalled);
        }
    }
}