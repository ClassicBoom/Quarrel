#pragma once
#include "WebrtcManager.g.h"

#include <winrt/Windows.Networking.Sockets.h>
#include <winrt/Windows.Storage.Streams.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <call/call.h>
#include <media/engine/webrtcvoiceengine.h>
#include <modules/rtp_rtcp/include/rtp_header_parser.h>
#include <api/audio_codecs/builtin_audio_encoder_factory.h>
#include <api/audio_codecs/builtin_audio_decoder_factory.h>
#include <modules/audio_processing/audio_buffer.h>
#include <common_audio/include/audio_util.h>

#include <iostream>
#include <IAudioDeviceWasapi.h>
#include <sodium/crypto_secretbox.h>

namespace winrt::Webrtc::implementation
{
	struct WebrtcManager : WebrtcManagerT<WebrtcManager>
	{
	public:
		
		WebrtcManager::WebrtcManager();
		void Create();
		
		Windows::Foundation::IAsyncAction ConnectAsync(hstring ip, hstring port, UINT32 ssrc);
		void SendSelectProtocol(UINT32 ssrc);

		void SetKey(array_view<const BYTE> key);
		void SetSpeaking(UINT32 ssrc, int speaking);

		void IpAndPortObtained(event_token const& token) noexcept;
		event_token IpAndPortObtained(Windows::Foundation::TypedEventHandler<hstring, USHORT> const& handler);

		void AudioOutData(event_token const& token) noexcept;
		event_token AudioOutData(Windows::Foundation::EventHandler<Windows::Foundation::Collections::IVector<float>> const& handler);

		void AudioInData(event_token const& token) noexcept;
		event_token AudioInData(Windows::Foundation::EventHandler<Windows::Foundation::Collections::IVector<float>> const& handler);


		void UpdateInBytes(Windows::Foundation::Collections::IVector<float> const& data);
		void UpdateOutBytes(Windows::Foundation::Collections::IVector<float> const& data);

	private:
		event<Windows::Foundation::TypedEventHandler<hstring, USHORT>> m_ipAndPortObtained;
		event<Windows::Foundation::EventHandler<Windows::Foundation::Collections::IVector<float>>> m_audioOutData;
		event<Windows::Foundation::EventHandler<Windows::Foundation::Collections::IVector<float>>> m_audioInData;
		
		void WebrtcManager::CreateVoe();
		void WebrtcManager::CreateCall();
		void WebrtcManager::SetupCall();
		webrtc::AudioSendStream* WebrtcManager::createAudioSendStream(uint32_t ssrc, uint8_t payloadType);
		webrtc::AudioReceiveStream* WebrtcManager::createAudioReceiveStream(uint32_t local_ssrc, uint32_t remote_ssrc, uint8_t payloadType);
		void OnMessageReceived(Windows::Networking::Sockets::DatagramSocket const& sender, Windows::Networking::Sockets::DatagramSocketMessageReceivedEventArgs const& args);
		Windows::Networking::Sockets::DatagramSocket udpSocket{ nullptr };
		Windows::Storage::Streams::DataWriter outputStream{ nullptr };
		unsigned char key[32];
		uint32_t ssrc;

		webrtc::Call* g_call = nullptr;

		std::map<short, webrtc::AudioReceiveStream*> audioReceiveStreams;
		webrtc::AudioSendStream* audioSendStream{ nullptr };

		rtc::scoped_refptr<webrtc::AudioDecoderFactory> g_audioDecoderFactory;
		rtc::scoped_refptr<webrtc::AudioEncoderFactory> g_audioEncoderFactory;

		cricket::WebRtcVoiceEngine* g_engine;

		int g_audioSendChannelId = -1;
		int g_audioReceiveChannelId = -1;
		int g_videoSendChannelId = -1;
		int g_videoReceiveChannelId = -1;

		class AudioLoopbackTransport;
		class VideoLoopbackTransport;
		webrtc::Transport* g_audioSendTransport = nullptr;

		std::unique_ptr<rtc::Thread> workerThread;

	};
}

namespace winrt::Webrtc::factory_implementation
{
	struct WebrtcManager : WebrtcManagerT<WebrtcManager, implementation::WebrtcManager>
	{
	};
}

namespace Webrtc
{
	class StreamTransport : public webrtc::Transport
	{
	public:
		StreamTransport(webrtc::Call* call, winrt::Windows::Storage::Streams::DataWriter const& sendStream);
		virtual bool SendRtp(const uint8_t* packet, size_t length, const webrtc::PacketOptions const& options);
		virtual bool SendRtcp(const uint8_t* packet, size_t length);
	private:
		webrtc::Call* call;
		winrt::Windows::Storage::Streams::DataWriter sendStream;
	};
}