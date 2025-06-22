using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                else if (child is ListView || child is Panel)
                {
                    child.BackColor = CurrentTheme.ListViewBackColor;
                    child.ForeColor = CurrentTheme.ListViewForeColor;
                }
                else
                {
                    ApplyTheme(child);
                }
            }
        }

        public static void SetTheme(Theme theme, Control root)
        {
            CurrentTheme = theme;
            ApplyTheme(root);
        }
    }
}
