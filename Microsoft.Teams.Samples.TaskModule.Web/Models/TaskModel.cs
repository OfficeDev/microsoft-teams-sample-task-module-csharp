using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Teams.Samples.TaskModule.Web.Models
{
    public class TaskEnvelope
    {
        [JsonProperty("task")]
        public Task Task { get; set; }
    }

    public class Task
    {
        [JsonProperty("value")]
        public TaskInfo TaskInfo { get; set; }

        [JsonProperty("type")]
        public TaskType Type { get; set; }
    }

    public class TaskInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("card")]
        public Attachment Card { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("fallbackUrl")]
        public string FallbackUrl { get; set; }

        [JsonProperty("completionBotId")]
        public string CompletionBotId { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum TaskType
    {
        /// <summary>
        /// Teams will display the value of value in a popup message box.
        /// </summary>
        [EnumMember(Value = "message")]
        Message,

        /// <summary>
        /// Allows you to "chain" sequences of Adaptive cards together in a wizard/multi-step experience.
        /// </summary>
        [EnumMember(Value = "continue")]
        Continue
    }

    public class FetchActionType
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "task/fetch";
    }

    public class FetchActionDetails : FetchActionType
    {
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    public class AdaptiveCardTaskModuleFetchAction
    {
        [JsonProperty("msteams")]
        public FetchActionType TaskAction { get; set; } = new FetchActionType();

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

}