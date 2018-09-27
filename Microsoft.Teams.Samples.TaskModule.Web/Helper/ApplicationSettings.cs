using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microsoft.Teams.Samples.TaskModule.Web.Helper
{
    public static class ApplicationSettings
    {
        public static string BaseUrl { get; set; }

        public static string MicrosoftAppId { get; set; }

        public static string DeepLink { get; set; }

        static ApplicationSettings()
        {
            BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            MicrosoftAppId = ConfigurationManager.AppSettings["MicrosoftAppId"];
            DeepLink = $"https://teams.microsoft.com/l/task/{MicrosoftAppId}?url={HttpUtility.UrlEncode(BaseUrl)}%2FcustomForm&title=TestTitle&completionBotId={MicrosoftAppId}";
        }
    }
}