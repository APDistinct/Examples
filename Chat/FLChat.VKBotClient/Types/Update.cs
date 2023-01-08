using Newtonsoft.Json;

namespace FLChat.VKBotClient.Types
{
    public class Update
    {
        /// <summary>
        /// The update's unique identifier. Update identifiers start from a certain positive number and increase sequentially.
        /// This ID becomes especially handy if you're using Webhooks, since it allows you to ignore repeated updates or to
        /// restore the correct update sequence, should they get out of order.
        /// </summary>
        [JsonProperty("update_id", Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// Optional. New incoming message of any kind — text, photo, sticker, etc.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Message { get; set; }

        /// <summary>
        /// Gets the update type.
        /// </summary>
        /// <value>
        /// The update type.
        /// </value>
        public UpdateType Type
        {

            get
            {
                if (Message != null) return UpdateType.Message;
                //if (InlineQuery != null) return UpdateType.InlineQuery;
                //if (ChosenInlineResult != null) return UpdateType.ChosenInlineResult;
                //if (CallbackQuery != null) return UpdateType.CallbackQuery;
                //if (EditedMessage != null) return UpdateType.EditedMessage;
                //if (ChannelPost != null) return UpdateType.ChannelPost;
                //if (EditedChannelPost != null) return UpdateType.EditedChannelPost;
                //if (ShippingQuery != null) return UpdateType.ShippingQuery;
                //if (PreCheckoutQuery != null) return UpdateType.PreCheckoutQuery;

                return UpdateType.Unknown;
            }
        }

        /// <summary>
        /// Optional. New incoming inline query
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineQuery InlineQuery { get; set; }

        /// <summary>
        /// Optional. New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message EditedMessage { get; set; }
    }
}
