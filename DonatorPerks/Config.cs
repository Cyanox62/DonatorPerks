using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace DonatorPerks
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Players who's UserId (user@steam) is in here will be allowed to teamkill at the end of the round.")]
		public List<string> TeamkillUsers { get; set; } = new List<string>();

		[Description("Players who's UserId (user@steam) is in here will be allowed to change their badge text and color.")]
		public List<string> BadgeOverrideUsers { get; set; } = new List<string>();

		[Description("Determines the colors that users are allowed to use in their custom badge color.")]
		public List<string> ValidBadgeColors { get; set; } = new List<string>()
		{
			"pink",
			"red",
			"default",
			"brown",
			"silver",
			"light_green",
			"crimson",
			"cyan",
			"aqua",
			"deep_pink",
			"tomato",
			"yellow",
			"magenta",
			"blue_green",
			"orange",
			"lime",
			"green",
			"emerald",
			"carmine",
			"nickel",
			"mint",
			"army_green",
			"pumpkin"
		};
	}
}
