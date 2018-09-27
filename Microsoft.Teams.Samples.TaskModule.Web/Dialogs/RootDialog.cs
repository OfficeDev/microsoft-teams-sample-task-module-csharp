// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Bot Framework: http://botframework.com
// Microsoft Teams: https://dev.office.com/microsoft-teams
//
// Bot Builder SDK GitHub:
// https://github.com/Microsoft/BotBuilder
//
// Bot Builder SDK Extensions for Teams
// https://github.com/OfficeDev/BotBuilder-MicrosoftTeams
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Microsoft.Teams.Samples.TaskModule.Web.Helper;
using Microsoft.Teams.Samples.TaskModule.Web.Models;
using System;
using System.Collections.Generic;

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
                        new AdaptiveTextBlock(){Text="Task Module Invocation from Adaptive Card",Weight=AdaptiveTextWeight.Bolder,Size=AdaptiveTextSize.Large}
                    },
                Actions = new List<AdaptiveAction>()
               {
                    new AdaptiveSubmitAction()
                    {
                        Title="Custom Form",
                        Data = new AdaptiveCardFetchAction() { AdditionalInfo = "html"}
                    },
                    new AdaptiveSubmitAction()
                    {
                        Title="Adaptive Card",
                        Data = new AdaptiveCardFetchAction() { AdditionalInfo = "adaptivecard"  }
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
                new FetchAction()
                {
                    AdditionalInfo = "html"
                }));
            card.Buttons.Add(new CardAction("invoke", "Adaptive Card", null,
                new FetchAction()
                {
                    AdditionalInfo = "adaptivecard"
                }));
            card.Buttons.Add(new CardAction("openUrl", "Task Module - Deeplink", null, deeplinkUlr));
            return card;
        }
    }
}