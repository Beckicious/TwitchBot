using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public class Chatters
    {
        public int ChatterCount { get; }
        public List<string> Staff { get; }
        public List<string> Admins { get; }
        public List<string> GlobalModerators { get; }
        public List<string> Moderators { get; }
        public List<string> VIPs { get; }
        public List<string> Viewers { get; }
        public List<string> AllViewers => new List<string>().Union(Staff).Union(Admins).Union(GlobalModerators).Union(Moderators).Union(VIPs).Union(Viewers).ToList();

        private Chatters(int chatterCount, List<string> staff, List<string> admins, List<string> globalMods, List<string> mods, List<string> vips, List<string> plebs)
        {
            this.ChatterCount = chatterCount;
            this.Staff = staff;
            this.Admins = admins;
            this.GlobalModerators = globalMods;
            this.Moderators = mods;
            this.VIPs = vips;
            this.Viewers = plebs;
        }

        public static Chatters GetChatters(string channelname)
        {
            HttpClient client = new HttpClient();
            var responseString = client.GetStringAsync($"http://tmi.twitch.tv/group/user/{channelname}/chatters").Result;
            JObject jObject = JObject.Parse(responseString);

            int chatterCount = (int)jObject["chatter_count"];
            var staff = jObject["chatters"]["staff"].Select(t => (string)t).ToList();
            var admins = jObject["chatters"]["admins"].Select(t => (string)t).ToList();
            var global_mods = jObject["chatters"]["global_mods"].Select(t => (string)t).ToList();
            var mods = jObject["chatters"]["moderators"].Select(t => (string)t).ToList();
            var vips = jObject["chatters"]["vips"].Select(t => (string)t).ToList();
            var plebs = jObject["chatters"]["viewers"].Select(t => (string)t).ToList();

            return new Chatters(chatterCount, staff, admins, global_mods, mods, vips, plebs);
        }
    }
}
