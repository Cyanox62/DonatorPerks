using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Newtonsoft.Json;
using System.IO;

namespace DonatorPerks
{
	class EventHandlers
	{
		internal void OnRoundStart() => Plugin.isRoundEnded = false;
		internal void OnRoundEnd(RoundEndedEventArgs ev) => Plugin.isRoundEnded = true;

		internal void OnRoundRestart()
		{
			File.WriteAllText(Plugin.BadgeOverridesFilePath, JsonConvert.SerializeObject(Plugin.badgeOverrides, Formatting.Indented));
		}

		internal void OnPlayerJoin(VerifiedEventArgs ev)
		{
			if (Plugin.singleton.Config.BadgeOverrideUsers.Contains(ev.Player.UserId))
			{
				if (!Plugin.badgeOverrides.ContainsKey(ev.Player.UserId))
				{
					Plugin.badgeOverrides.Add(ev.Player.UserId, new CustomInfo());
				}

				Timing.CallDelayed(1f, () =>
				{
					CustomInfo info = Plugin.badgeOverrides[ev.Player.UserId];
					if (info.nickname != null) ev.Player.DisplayNickname = info.nickname;
					if (info.badge != null) ev.Player.RankName = info.badge;
					if (info.color != null) ev.Player.RankColor = info.color;
				});
			}
		}
	}
}
