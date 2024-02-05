using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SCPUtils
{
    public static class DiscordWebHook
    {
        public static async Task<WebResponse> Message(string userid, Exiled.API.Features.Player player)
        {
            WebResponse response = null;

            WebRequest wr = (HttpWebRequest)WebRequest.Create(ScpUtils.StaticInstance.Config.WebhookUrl);

            wr.ContentType = "application/json";
            wr.Method = "POST";
            wr.Timeout = 1350;

            using (var vr = new StreamWriter(wr.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    username = ScpUtils.StaticInstance.Config.WebhookNickname,
                    embeds = new[]
                    {
                        new
                        {

                            title = $"{ScpUtils.StaticInstance.Translation.Report}",
                            description = $"{ScpUtils.StaticInstance.Translation.Description} {userid}\n" +
                            $"{ScpUtils.StaticInstance.Translation.PlayerInfo}\n" +
                            $"{ScpUtils.StaticInstance.Translation.Username}: {player.Nickname}\n" +
                            $"{ScpUtils.StaticInstance.Translation.UserId}: {player.UserId}\n" +
                            $"{ScpUtils.StaticInstance.Translation.TemporaryId}: {player.Id}",
                            color = "25233"
                        }
                    }
                });
                vr.Write(json);
            }
            return response = await wr.GetResponseAsync();

        }

    }
}