using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Linq;

namespace DonatorPerks.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Badge : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Changes your badge";

		string ICommand.Command { get; } = "badge";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender playerSender)
			{
				Player player = Player.Get(playerSender);
				if (Plugin.singleton.Config.BadgeOverrideUsers.Contains(player.UserId))
				{
					if (Plugin.badgeOverrides.ContainsKey(player.UserId))
					{
						if (arguments.Count > 0)
						{
							string arg = arguments.ElementAt(0).ToLower();
							if (arg == "text")
							{
								if (arguments.Count == 2)
								{
									string badge = string.Empty;
									for (int i = 1; i < arguments.Count; i++) badge += $"{arguments.ElementAt(i)} ";
									badge = badge.Trim();
									if (badge.Length > 32) badge = badge.Substring(0, 32);
									Plugin.badgeOverrides[player.UserId].badge = badge;
									player.GroupName = badge;
									response = $"Your badge text has been updated to '{badge}'.";
									return true;
								}
								else if (arguments.Count == 1)
								{
									Plugin.badgeOverrides[player.UserId].badge = null;
									player.RankName = player.Group.BadgeText;
									response = $"Your badge text has been reset to default.";
									return true;
								}
								else
								{
									response = "Usage: BADGE [TEXT / COLOR] [CONTENT]";
									return false;
								}
							}
							else if (arg == "color")
							{
								if (arguments.Count == 2)
								{
									string color = arguments.ElementAt(1).ToLower();
									if (Plugin.singleton.Config.ValidBadgeColors.Contains(color))
									{
										Plugin.badgeOverrides[player.UserId].color = color;
										player.RankColor = color;
										response = $"Your badge color has been updated to '{color}'.";
										return true;
									}
									else
									{
										response = $"Invalid color!";
										return false;
									}
								}
								else if (arguments.Count == 1)
								{
									Plugin.badgeOverrides[player.UserId].color = null;
									player.RankColor = player.Group.BadgeColor;
									response = $"Your badge text has been reset to default.";
									return true;
								}
								else
								{
									response = "Usage: BADGE [TEXT / COLOR] [CONTENT]";
									return false;
								}
							}
							else
							{
								response = "Usage: BADGE [TEXT / COLOR] [CONTENT]";
								return false;
							}
						}
						else
						{
							response = "Usage: BADGE [TEXT / COLOR] [CONTENT]";
							return false;
						}
					}
					else
					{
						response = "Failed to grab player info.";
						return false;
					}
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
