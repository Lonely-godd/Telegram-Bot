using System.Drawing;

namespace BotConfigurator
{
	internal static class Theme
	{
		// ==================== ЦВЕТА ====================
		public static readonly Color SidebarBg = Color.FromArgb(22, 22, 30);
		public static readonly Color SidebarHover = Color.FromArgb(40, 40, 55);
		public static readonly Color SidebarSelected = Color.FromArgb(99, 102, 241);
		public static readonly Color SidebarText = Color.FromArgb(210, 210, 225);
		public static readonly Color SidebarMuted = Color.FromArgb(100, 100, 120);
		public static readonly Color HeaderBg = Color.FromArgb(15, 15, 20);

		public static readonly Color PageBg = Color.FromArgb(246, 247, 250);
		public static readonly Color Surface = Color.White;
		public static readonly Color Border = Color.FromArgb(225, 228, 235);

		public static readonly Color TextPrimary = Color.FromArgb(20, 20, 30);
		public static readonly Color TextSecondary = Color.FromArgb(100, 106, 120);
		public static readonly Color TextMuted = Color.FromArgb(160, 165, 180);

		public static readonly Color Accent = Color.FromArgb(99, 102, 241);
		public static readonly Color AccentDark = Color.FromArgb(75, 78, 210);
		public static readonly Color Danger = Color.FromArgb(220, 53, 69);
		public static readonly Color DangerDark = Color.FromArgb(185, 30, 48);
		public static readonly Color Success = Color.FromArgb(22, 163, 74);
		public static readonly Color SuccessDark = Color.FromArgb(14, 128, 56);

		public static readonly Font FontAppTitle = new("Segoe UI Semibold", 13.5f, FontStyle.Bold);
		public static readonly Font FontSection = new("Segoe UI", 12f, FontStyle.Bold);
		public static readonly Font FontBase = new("Segoe UI", 10.5f);
		public static readonly Font FontSmall = new("Segoe UI", 9.5f);
		public static readonly Font FontCaps = new("Segoe UI", 8.5f, FontStyle.Bold);
		public static readonly Font FontButton = new("Segoe UI", 10f, FontStyle.Bold);
	}
}