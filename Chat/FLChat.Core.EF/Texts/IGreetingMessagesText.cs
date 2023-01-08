using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Texts
{
    public interface IGreetingMessagesText
    {
        /// <summary>
        /// First part of text text for lite link greeting message for known user
        /// </summary>
        string LiteLinkKnownUser { get; }

        /// <summary>
        /// Second part of text for lite link greeting message for users who has owner with flchat
        /// </summary>
        string LiteLinkRouted { get; }

        /// <summary>
        /// Second part of text for lite link greeting message for users who has not owner with flchat
        /// </summary>
        string LiteLinkUnrouted { get; }

        /// <summary>
        /// text for lite link greeting message for unknown user
        /// </summary>
        string LiteLinkUnknownUser { get; }

        /// <summary>
        /// text for greeting message for unknown user who link was rejected or unknown
        /// </summary>
        string LinkRejectedOrUnknown { get; }
    }

    public class GreetingMessagesTextWrapper : IGreetingMessagesText
    {
        private IGreetingMessagesText _texts;

        public GreetingMessagesTextWrapper(IGreetingMessagesText texts = null) {
            _texts = texts ?? new GreetingMessagesText();
        }

        public virtual string LiteLinkKnownUser => _texts.LiteLinkKnownUser;

        public virtual string LiteLinkRouted => _texts.LiteLinkRouted;

        public virtual string LiteLinkUnrouted => _texts.LiteLinkUnrouted;

        public virtual string LiteLinkUnknownUser => _texts.LiteLinkUnknownUser;

        public virtual string LinkRejectedOrUnknown => _texts.LinkRejectedOrUnknown;
    }

    public static class IGreetingMessagesTextExtentions
    {
        public const string AddresseeCnst = "%addressee%";

        public static string LiteLinkRouted_Addressee(this IGreetingMessagesText o) => AddresseeCnst;
    }
}
