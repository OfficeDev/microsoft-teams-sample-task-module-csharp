using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Microsoft.Teams.Samples.TaskModule.Web.Helper;
using Microsoft.Teams.Samples.TaskModule.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async System.Threading.Tasks.Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async System.Threading.Tasks.Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            var messageText = message.GetTextWithoutMentions();
            var reply = message.CreateReply();

            // Get deeplink Uri
            var deeplinkUlr = new Uri(ApplicationSettings.DeepLink);
            ThumbnailCard card = GetTaskModuleOptions(deeplinkUlr);
            Attachment adaptiveCard = GetTaskModuleOptionsAdaptiveCard(deeplinkUlr);

            reply.Attachments.Add(card.ToAttachment());
            reply.Attachments.Add(adaptiveCard);

            await context.PostAsync(reply);
            context.Wait(MessageReceivedAsync);
        }

        private static Attachment GetTaskModuleOptionsAdaptiveCard(Uri deeplinkUlr)
        {
            var card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                    {
                        new AdaptiveTextBlock(){Text="Task Module Invocation from Adaptive Card",Weight=AdaptiveTextWeight.Bolder,Size=AdaptiveTextSize.ExtraLarge}
                    },
                Actions = new List<AdaptiveAction>()
               {
                    new AdaptiveSubmitAction()
                    {
                        Title="Custom Form",
                        Data = new AdaptiveCardTaskModuleFetchAction() { AdditionalInfo = "html"}
                    },
                    new AdaptiveSubmitAction()
                    {
                        Title="Adaptive Card",
                        Data = new AdaptiveCardTaskModuleFetchAction() { AdditionalInfo = "adaptivecard"  }
                    },
                    new AdaptiveOpenUrlAction()
                    {
                        Title="Task Module - Deeplink",
                        Url=deeplinkUlr
                    }
               },
            };
            return new Attachment() { ContentType = AdaptiveCard.ContentType, Content = card };
        }

        private static ThumbnailCard GetTaskModuleOptions(Uri deeplinkUlr)
        {
            ThumbnailCard card = new ThumbnailCard();
            card.Title = "Task Module Invocation from Thumbnail Card";
            card.Buttons = new List<CardAction>();
            card.Buttons.Add(new CardAction("invoke", "Custom Form", null,
                new FetchActionDetails()
                {
                    AdditionalInfo = "html"
                }));
            card.Buttons.Add(new CardAction("invoke", "Adaptive Card", null,
                new FetchActionDetails()
                {
                    AdditionalInfo = "adaptivecard"
                }));
            card.Buttons.Add(new CardAction("openUrl", "Task Module - Deeplink", null, deeplinkUlr));
            return card;
        }
    }
}