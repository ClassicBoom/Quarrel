﻿// Quarrel © 2022

using Discord.API.Models.Json.Settings;

namespace Quarrel.Client.Models.Settings
{
    internal class GuildSettings
    {
        public GuildSettings(JsonGuildSettings settings, out ChannelSettings[] channelSettings)
        {
            GuildId = settings.GuildId;
            SuppressEveryone = settings.SuppressEveryone;
            SuppressRoles = settings.SuppressRoles;
            Muted = settings.Muted;
            MobilePush = settings.MobilePush;

            channelSettings = new ChannelSettings[settings.ChannelOverrides.Length];
            for (int i = 0; i < channelSettings.Length; i++)
            {
                channelSettings[i] = new ChannelSettings(settings.ChannelOverrides[i]);
            }
        }

        public ulong? GuildId { get; }

        public bool SuppressEveryone { get; }

        public bool SuppressRoles { get; }

        public bool Muted { get; }

        public bool MobilePush { get; }
    }
}
