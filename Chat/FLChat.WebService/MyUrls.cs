using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FLChat.WebService
{
    public class MyUrls
    {
        private UrlDetector _detector;

        private const string PAGE_ALIVE = "alive";
        private const string PAGE_USER = "users/{id}";
        private const string PAGE_USER_AVATAR = "users/{id}/avatar";
        //private const string PAGE_PROFILE = "profile";

        public MyUrls(string baseAddress)
        {
            _detector = new UrlDetector(baseAddress);
        }

        public bool IsAlive(Uri uri)
        {
            return _detector.IsUrl(PAGE_ALIVE, uri);
        }

        public bool IsUser(Uri uri, out string userId)
        {
            return _detector.IsUrl(PAGE_USER, uri, out userId);
        }

        public bool IsUserAvatar(Uri uri, out string userId)
        {
            return _detector.IsUrl(PAGE_USER_AVATAR, uri, out userId);
        }
        //public bool IsProfile(Uri uri)
        //{
        //    return _detector.IsUrl(PAGE_PROFILE, uri);
        //}
    }
}
