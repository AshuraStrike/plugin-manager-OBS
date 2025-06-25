namespace PluginManagerObs.Classes.ThemeManager
{
    public static class Themes
    {
        public static Theme Light = new Theme
        {
            BackgroundColor = SystemColors.ControlLight,
            ForegroundColor = SystemColors.ControlText,
            ButtonBackColor = SystemColors.ControlLight,
            ButtonForeColor = SystemColors.ControlText,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = SystemColors.ControlText,
            ListViewBackColor = SystemColors.ControlLight,
            ListViewForeColor = SystemColors.ControlText,
            PluginStateColor_NOT_INSTALLED = Color.LightBlue,
            PluginStateColor_INSTALLED = Color.Green,
            PluginStateColor_MANUALLY_INSTALLED = Color.GreenYellow,
            PluginStateColor_FILES_PRESENT = Color.Yellow,
            PluginStateColor_INSTALLED_MODIFIED = Color.Crimson
        };

        public static Theme Dark = new Theme
        {
            BackgroundColor = SystemColors.ControlDarkDark,
            ForegroundColor = SystemColors.ControlText,
            ButtonBackColor = SystemColors.ControlLight,
            ButtonForeColor = SystemColors.ControlText,
            TextBoxBackColor = SystemColors.Control,
            TextBoxForeColor = SystemColors.ControlText,
            ListViewBackColor = SystemColors.ControlDark,
            ListViewForeColor = SystemColors.ControlText,
            PluginStateColor_NOT_INSTALLED = Color.RoyalBlue,
            PluginStateColor_INSTALLED = Color.Green,
            PluginStateColor_MANUALLY_INSTALLED = Color.SeaGreen,
            PluginStateColor_FILES_PRESENT = Color.Orange,
            PluginStateColor_INSTALLED_MODIFIED = Color.Firebrick
        };
    }
}
