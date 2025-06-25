namespace PluginManagerObs.Classes.ThemeManager
{
    internal class ThemeManager
    {
        public static Theme CurrentTheme { get; set; } = Themes.Light;

        public static void ApplyTheme(Control control)
        {
            control.BackColor = CurrentTheme.BackgroundColor;
            control.ForeColor = CurrentTheme.ForegroundColor;

            foreach (Control child in control.Controls)
            {
                if (child is Button)
                {
                    child.BackColor = CurrentTheme.ButtonBackColor;
                    child.ForeColor = CurrentTheme.ButtonForeColor;
                }
                else if (child is TextBox)
                {
                    child.BackColor = CurrentTheme.TextBoxBackColor;
                    child.ForeColor = CurrentTheme.TextBoxForeColor;
                }
                else if (child is Panel)
                {
                    child.BackColor = CurrentTheme.ListViewBackColor;
                    child.ForeColor = CurrentTheme.ListViewForeColor;
                }
                else if (child is ListView)
                {
                    child.BackColor = CurrentTheme.ListViewBackColor;
                    child.ForeColor = CurrentTheme.ListViewForeColor;

                    ListView lv = child as ListView;
                    foreach (ListViewItem item in lv.Items)
                    {
                        var subItem = item.SubItems[1];
                        subItem.BackColor = getBackColorByInstallState(subItem.Text);
                    }
                }
                else
                {
                    ApplyTheme(child);
                }
            }
        }

        private static Color getBackColorByInstallState(string text)
        {
            Color color = SystemColors.Control;
            switch (text)
            {
                case "Not Installed":
                    color = ThemeManager.CurrentTheme.PluginStateColor_NOT_INSTALLED;
                    break;
                case "Installed":
                    color = ThemeManager.CurrentTheme.PluginStateColor_INSTALLED;
                    break;
                case "Manually Installed":
                    color = ThemeManager.CurrentTheme.PluginStateColor_MANUALLY_INSTALLED;
                    break;
                case "Files present":
                    color = ThemeManager.CurrentTheme.PluginStateColor_FILES_PRESENT;
                    break;
                case "Installed - Modified":
                    color = ThemeManager.CurrentTheme.PluginStateColor_INSTALLED_MODIFIED;
                    break;
                default:
                    break;
            }
            return color;
        }

        public static void SetTheme(Theme theme, Control root)
        {
            CurrentTheme = theme;
            ApplyTheme(root);
        }
    }
}
