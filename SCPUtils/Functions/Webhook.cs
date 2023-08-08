namespace SCPUtils
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public static class DiscordWebHook
    {
        public static async Task<WebResponse> Message(string userid, PluginAPI.Core.Player player)
        {
            WebResponse response = null;

            WebRequest wr = (HttpWebRequest)WebRequest.Create(ScpUtils.StaticInstance.GetWebhookConfig.Url);

            wr.ContentType = "application/json";
            wr.Method = "POST";
            wr.Timeout = 1350;

            using (var vr = new StreamWriter(wr.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    username = ScpUtils.StaticInstance.GetWebhookConfig.Nickname,
                    embeds = new[]
                    {
                        new
                        {
                            title = ScpUtils.StaticInstance.GetWebhookConfig.EmbedTitle,
                            description = ScpUtils.StaticInstance.GetWebhookConfig.EmbedContent.Replace("$muted", userid).Replace("$username", player.Nickname).Replace("$steamid", $"{player.PlayerId}").Replace("$playerid", $"{player.PlayerId}"),
                            color = ScpUtils.StaticInstance.GetWebhookConfig.EmbedColor
                        }
                    }
                });
                vr.Write(json);
            }
            return response = await wr.GetResponseAsync();

        }

    }
}