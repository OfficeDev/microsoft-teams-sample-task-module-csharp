using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Sample.SimpleEchoBot;
using Microsoft.Teams.Samples.TaskModule.Web.Helper;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microsoft.Teams.Samples.TaskModule.Web.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else if (activity.Type == ActivityTypes.Invoke)
            {
                return HandleInvokeMessages(activity);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private HttpResponseMessage HandleInvokeMessages(Activity activity)
        {
            var activityValue = activity.Value.ToString();
            if (activity.Name == "task/fetch")
            {
                var action = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FetchActionDetails>(activityValue);
                Models.TaskInfo taskInfo = new Models.TaskInfo()
                {
                    Title = "Task Module",
                    Height = "medium",
                    Width = "medium"
                };

                // Check the card vs html
                if (action.AdditionalInfo.Contains("html"))
                    taskInfo.Url = ApplicationSettings.BaseUrl + "/customform";
                else
                    taskInfo.Card = AdaptiveCardHelper.GetAdaptiveCard();// Attachment AdaptiveCardHelper.GetAdaptiveCard();

                Models.TaskEnvelope taskEnvelope = new Models.TaskEnvelope
                {
                    Task = new Models.Task()
                    {
                        Type = Models.TaskType.Continue,
                        TaskInfo = taskInfo
                    }
                };
                return Request.CreateResponse(HttpStatusCode.OK, taskEnvelope);

            }
            else if (activity.Name == "task/submit")
            {
                Console.WriteLine(activity.Value);

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity reply = activity.CreateReply("Received = " + activity.Value.ToString());
                connector.Conversations.ReplyToActivity(reply);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);


        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.InstallationUpdate)
            {
                // Handle add/remove from contact lists
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
