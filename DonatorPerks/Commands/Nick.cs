using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace DonatorPerks.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Nick : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Set your nick";

		string ICommand.Command { get; } = "nick";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender playerSender)
			{
				Player player = Player.Get(playerSender);
				if (Plugin.singleton.Config.BadgeOverrideUsers.Contains(player.UserId))
				{
					if (arguments.Count > 0)
					{
						string msg = string.Empty;
						if (Plugin.badgeOverrides.ContainsKey(player.UserId))
						{
							foreach (string s in arguments) msg += $"{s} ";
							msg = msg.Trim();
							if (msg.Length > 32) msg = msg.Substring(0, 32);
							player.ReferenceHub.nicknameSync.Network_displayName = msg;
							Plugin.badgeOverrides[player.UserId].nickname = msg;
							response = $"Your nickname has been changed to '{msg}'.";
							return true;
						}
						else
						{
							response = "Failed to grab player info.";
							return false;
						}
					}
					else
					{
						player.ReferenceHub.nicknameSync.Network_displayName = null;
						Plugin.badgeOverrides[player.UserId].nickname = null;
						response = "Your nickname has been cleared.";
					}
					return true;
				}
				else
				{
					response = "You do not have permissions to use this command.";
					return false;
				}
			}
			else
			{
				response = "Only players may use this command";
				return false;
			}
		}
	}
}
