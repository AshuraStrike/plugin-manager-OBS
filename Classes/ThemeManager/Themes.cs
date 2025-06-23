using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManagerObs.Classes.ThemeManager
{
    public static class Themes
    {
        public static Theme Light = new Theme
        {
            BackgroundColor = SystemColors.Control,
            ForegroundColor = SystemColors.ControlText,
            ButtonBackColor = SystemColors.Control,
            ButtonForeColor = SystemColors.ControlText,
            TextBoxBackColor = SystemColors.Control,
            TextBoxForeColor = SystemColors.ControlText,
            ListViewBackColor = SystemColors.Control,
            ListViewForeColor = SystemColors.ControlText
        };

        public static Theme Dark = new Theme
        {
            BackgroundColor = SystemColors.ControlDarkDark,
            ForegroundColor = SystemColors.ControlText,
            ButtonBackColor = Color.FromArgb(225,225,225),
            ButtonForeColor = SystemColors.ControlText,
            TextBoxBackColor = SystemColors.Control,
            TextBoxForeColor = SystemColors.ControlText,
            ListViewBackColor = SystemColors.ControlDark,
            ListViewForeColor = SystemColors.ControlText
        };
    }
}
