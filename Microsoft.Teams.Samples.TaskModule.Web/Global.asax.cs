using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System.Reflection;
using Autofac;
using System.Configuration;

namespace Microsoft.Teams.Samples.TaskModule.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Conversation.UpdateContainer(
               builder =>
               {
                   builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                   // Using Azure Table Storage
                   // var store = new TableBotDataStore(ConfigurationManager.AppSettings["AzureWebJobsStorage"]); // requires Microsoft.BotBuilder.Azure Nuget package 

                   // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
                   // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 
                   var store = new InMemoryDataStore(); // volatile in-memory store

                   builder.Register(c => store)
                      .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                      .AsSelf()
                      .SingleInstance();

               });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
