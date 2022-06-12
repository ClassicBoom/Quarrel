﻿// Quarrel © 2022

using Discord.API.Sockets;
using Discord.API.Voice.Models.Handshake;
using System;
using System.Threading.Tasks;

namespace Discord.API.Voice
{
    internal partial class VoiceConnection
    {
        protected override async Task SendHeartbeatAsync()
        {
            try
            {
                var frame = new VoiceSocketFrame<int>()
                {
                    Operation = VoiceOperation.Heartbeat,
                    Payload = (int)Math.Round(DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds),
                };

                await SendMessageAsync(frame);
            }
            catch
            {
            }
        }

        private bool OnHello(VoiceSocketFrame<VoiceHello> frame)
        {
            _ = SetupVoiceConnection(frame.Payload.HeartbeatInterval);
            return true;
        }

        private bool OnReady(VoiceSocketFrame<VoiceReady> frame)
        {
            var ready = frame.Payload;
            _ssrc = ready.SSRC;

            return FireEvent(frame, Ready);
        }

        private async Task SetupVoiceConnection(int interval)
        {
            switch (ConnectionStatus)
            {
                case ConnectionStatus.Reconnecting:
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Resuming:
                    ConnectionStatus = ConnectionStatus.Connected;
                    break;
                default:
                    ConnectionStatus = ConnectionStatus.Error;
                    return;
            }

            double jitter = new Random().NextDouble();
            await Task.Delay((int)(interval * jitter));
            _ = BeginHeartbeatAsync(interval);
        }

        private async Task IdentifySelfToVoiceConnection()
        {
            var identity = new VoiceIdentity()
            {
                ServerId = _voiceConfig.GuildId ?? _voiceConfig.ChannelId,
                SessionId = _state.SessionId,
                Token = _voiceConfig.Token,
                UserId = _state.UserId,
                Video = false,
            };

            var payload = new VoiceSocketFrame<VoiceIdentity>
            {
                Event = VoiceEvent.IDENTIFY,
                Operation = VoiceOperation.Identify,
                Payload = identity,
            };

            await SendMessageAsync(payload);
        }
    }
}
