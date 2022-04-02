﻿// Adam Dernis © 2022

using Discord.API.Models.Json.Emojis;
using System.Text.Json.Serialization;

namespace Discord.API.Gateways.Models.Messages
{
    internal class MessageReactionUpdated
    {
        [JsonPropertyName("user_id"), JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public ulong UserId { get; set; }

        [JsonPropertyName("channel_id"), JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public ulong ChannelId { get; set; }

        [JsonPropertyName("message_id"), JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public ulong MessageId { get; set; }

        [JsonPropertyName("guild_id"), JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public ulong? GuildId { get; set; }

        [JsonPropertyName("emoji")]
        public JsonEmoji Emoji { get; set; }
    }
}
