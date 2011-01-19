using System;

namespace Inferis.Apis.Campfire
{
    public class CampfireMessage
    {
        //       {"room_id":271712,"created_at":"2010/10/05 08:51:10 +0000","body":"Tegen den honger?","id":264006344,"user_id":654400,"type":"TextMessage"}

        public long Id { get; set; }
        public long RoomId { get; set; }
        public long UserId { get; set; }
        public CampfireMessageType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Body { get; set; }
    }

    public enum CampfireMessageType
    {
        TimestampMessage,
        TextMessage, // (regular chat message),
        PasteMessage, // (pre-formatted message, rendered in a fixed-width font),
        SoundMessage, // (plays a sound as determined by the message, which can be either “rimshot”, “crickets”, or “trombone”),
        TweetMessage // (a Twitter status URL to be fetched and inserted into the chat)
    }
}