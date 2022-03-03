using Exiled.API.Features;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DonatorPerks
{
    public class Plugin : Plugin<Config>
    {
		internal static string PluginDirectory = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "DonatorPerks");
		internal static string BadgeOverridesFilePath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "DonatorPerks"), "badgeOverrides.json");

		internal static Dictionary<string, CustomInfo> badgeOverrides = new Dictionary<string, CustomInfo>();

		internal static bool isRoundEnded = false;

		internal static Plugin singleton;
		private EventHandlers ev;
		private Harmony hInstance;

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			hInstance = new Harmony("cyan.donatorperks");
			hInstance.PatchAll();

			if (!Directory.Exists(PluginDirectory)) Directory.CreateDirectory(PluginDirectory);
			if (!File.Exists(BadgeOverridesFilePath)) File.WriteAllText(BadgeOverridesFilePath, JsonConvert.SerializeObject(badgeOverrides, Formatting.Indented));
			badgeOverrides = JsonConvert.DeserializeObject<Dictionary<string, CustomInfo>>(File.ReadAllText(BadgeOverridesFilePath));

			ev = new EventHandlers();

			Exiled.Events.Handlers.Server.RoundEnded += ev.OnRoundEnd;
			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;

			Exiled.Events.Handlers.Player.Verified += ev.OnPlayerJoin;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Server.RoundEnded -= ev.OnRoundEnd;
			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound -= ev.OnRoundRestart;

			Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerJoin;

			ev = null;

			hInstance.UnpatchAll(hInstance.Id);
			hInstance = null;
		}

		public override string Author => "Cyanox";
	}

	class CustomInfo
	{
		public string nickname;
		public string badge;
		public string color;
	}

	[HarmonyPatch(typeof(PlayerStatsSystem.AttackerDamageHandler), nameof(PlayerStatsSystem.AttackerDamageHandler.ProcessDamage))]
	class FriendlyFirePatch
	{
		public static bool Prefix(PlayerStatsSystem.AttackerDamageHandler __instance)
		{
			if (Plugin.isRoundEnded && Plugin.singleton.Config.TeamkillUsers.Contains(__instance.Attacker.Hub.characterClassManager.UserId))
			{
				__instance.ForceFullFriendlyFire = true;
			}
			return true;
		}
	}
}
