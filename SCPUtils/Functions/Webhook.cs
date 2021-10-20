using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;

namespace SCPUtils
{
    public static class DiscordWebHook
    {
        public static void Message(string userid, Exiled.API.Features.Player player)
        {      

            if (ScpUtils.StaticInstance.Config.WebhookUrl == "None") return;
            WebRequest wr = (HttpWebRequest)WebRequest.Create(ScpUtils.StaticInstance.Config.WebhookUrl);

            wr.ContentType = "application/json";
            wr.Method = "POST";

            using (var vr = new StreamWriter(wr.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    username = ScpUtils.StaticInstance.Config.WebhookNickname,
                    embeds = new[]
                    {
                        new
                        {
                            
                            title = "Mute evasion report!",
                            description = $"Mute evasion detected! Userid of muted user: {userid}\n" +
                            $"Player info:\n" +
                            $"Username: {player.Nickname}\n" +
                            $"User-ID: {player.UserId}\n" +
                            $"Temporarily ID: {player.Id}",                          
                            color = "25233"
                        }
                    }
                });
                 vr.Write(json);                 
            }            
             //var response = (HttpWebResponse)wr.GetResponse();       
        }       

    }
}