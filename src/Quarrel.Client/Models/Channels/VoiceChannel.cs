﻿// Quarrel © 2022

using CommunityToolkit.Diagnostics;
using Discord.API.Models.Json.Channels;
using Quarrel.Client.Models.Channels.Abstract;
using Quarrel.Client.Models.Channels.Interfaces;

namespace Quarrel.Client.Models.Channels
{
    /// <summary>
    /// A voice channel in a guild managed by a <see cref="QuarrelClient"/>.
    /// </summary>
    public class VoiceChannel : GuildChannel, IGuildVoiceChannel
    {
        internal VoiceChannel(JsonChannel restChannel, ulong? guildId, QuarrelClient context) :
            base(restChannel, guildId, context)
        {
            Guard.IsNotNull(restChannel.Bitrate, nameof(restChannel.Bitrate));

            Bitrate = restChannel.Bitrate.Value;
            UserLimit = restChannel.UserLimit;
            RTCRegion = restChannel.RTCRegion;
            CategoryId = restChannel.CategoryId;
        }

        /// <inheritdoc/>
        public int Bitrate { get; private set; }

        /// <inheritdoc/>
        public int? UserLimit { get; private set; }

        /// <inheritdoc/>
        public ulong? CategoryId { get; private set; }

        /// <inheritdoc cref="IMessageChannel.MentionCount"/>
        public int? MentionCount { get; internal set; }

        /// <inheritdoc cref="IMessageChannel.LastMessageId"/>
        public ulong? LastMessageId { get; internal set; }

        /// <inheritdoc cref="IMessageChannel.LastReadMessageId"/>
        public ulong? LastReadMessageId { get; internal set; }

        /// <inheritdoc/>
        public bool IsUnread => LastMessageId > LastReadMessageId;

        /// <inheritdoc/>
        public string? RTCRegion { get; private set; }

        int? IMessageChannel.MentionCount
        {
            get => MentionCount;
            set => MentionCount = value;
        }

        ulong? IMessageChannel.LastMessageId
        {
            get => LastMessageId;
            set => LastMessageId = value;
        }

        ulong? IMessageChannel.LastReadMessageId
        {
            get => LastReadMessageId;
            set => LastReadMessageId = value;
        }

        internal override void PrivateUpdateFromJsonChannel(JsonChannel jsonChannel)
        {
            base.PrivateUpdateFromJsonChannel(jsonChannel);
            Bitrate = jsonChannel.Bitrate ?? Bitrate;
            UserLimit = jsonChannel.UserLimit ?? UserLimit;
            CategoryId = jsonChannel.CategoryId ?? CategoryId;
            RTCRegion = jsonChannel.RTCRegion ?? RTCRegion;
        }

        internal override JsonChannel ToJsonChannel()
        {
            JsonChannel restChannel = base.ToJsonChannel();
            restChannel.Bitrate = Bitrate;
            restChannel.UserLimit = UserLimit;
            restChannel.CategoryId = CategoryId;
            restChannel.Position = Position;
            restChannel.GuildId = GuildId;
            restChannel.RTCRegion = RTCRegion;
            return restChannel;
        }
    }
}
