using PluginManagerObs.Classes;
using PluginManagerObs.Classes.ThemeManager;
using PluginManagerObs.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace PluginManagerObs
{
    public partial class FormMain : Form
    {
        ControllerPlugins controllerPlugins = new();

        enum OBSRunningStateType
        {
            NotRunning,
            ExactRunning,
            AdminRunning
        }
        OBSRunningStateType OBSRunningState = OBSRunningStateType.NotRunning;

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonObsPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath + '\\';
                if (controllerPlugins.validateObsPath(selectedPath))
                {
                    Debug.WriteLine($"OBS found");
                    controllerPlugins.setObsPath(selectedPath);
                }
                else
                {
                    Debug.WriteLine("Not OBS");
                    DialogResult result2 = MessageBox.Show("Want to use it anyways?", "Not OBS directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        controllerPlugins.setObsPath(selectedPath);
                    }
                }
                if (controllerPlugins.pluginsPath != string.Empty)
                {
                    buttonReload.PerformClick();
                }
                labelObsPath.Text = controllerPlugins.getObsPath();
                controllerPlugins.savePaths();
            }
        }

        private void buttonPluginsPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                controllerPlugins.pluginsPath = folderBrowserDialog.SelectedPath + '\\';
                labelPluginsPath.Text = controllerPlugins.pluginsPath;
                buttonReload.PerformClick();
                controllerPlugins.savePaths();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (controllerPlugins.loadPaths())
            {
                labelObsPath.Text = controllerPlugins.getObsPath();
                labelPluginsPath.Text = controllerPlugins.pluginsPath;
                buttonReload.PerformClick();
                UpdatePluginActionButtonState();
            }
            ThemeManager.SetTheme(ThemeManager.CurrentTheme, this);
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = string.Empty;
            listViewPlugins.Items.Clear();
            bool refreshed = controllerPlugins.populatePluginLists();
            if (refreshed)
            {
                PopulateListViewPlugins();
            }
            else
            {
                MessageBox.Show("Set/Change the Plugins path", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            listViewPlugins.Items.Clear();
            controllerPlugins.filterPlugins(textBoxSearch.Text);
            PopulateListViewPlugins();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (controllerPlugins.getObsPath() == string.Empty)
            {
                return;
            }
            if (listViewPlugins.SelectedItems.Count == 0)
            {
                return;
            }
            if (AbortActionOverride()) { return; }
            ListViewItem lvi = listViewPlugins.SelectedItems[0];
            Debug.WriteLine($"Plugin to add: {lvi.Text}");
            if (!controllerPlugins.addPlugins(lvi.Text))
            {
                MessageBox.Show($"Failed to add {lvi.Text}", "Unable to add", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            listViewPlugins.Items.Clear();
            // Update installation state
            controllerPlugins.populatePluginLists();
            PopulateListViewPlugins();
        }

        private bool AbortActionOverride()
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                return false;
            }
            else
            {
                ListViewItem lvi = listViewPlugins.SelectedItems[0];
                foreach (Plugin p in controllerPlugins.listPlugins)
                {
                    if (p.Name != lvi.Text)
                    {
                        continue;
                    }
                    switch (p.Installed)
                    {
                        case PluginInstallationType.FILES_PRESENT:
                        case PluginInstallationType.INSTALLED_MODIFIED:
                            DialogResult dr = MessageBox.Show("This action will overwrite actions done to the selected plugin outside of the Plugin Manager, unless multiple versions are managed.\r\nDo you want to continue?", "Overwrite outside action?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (dr != DialogResult.Yes)
                            {
                                return true;
                            }
                            return false;
                        default:
                            return false;
                    }
                }
                // shouldn't be reached
                return true;
            }
        }

        private void PopulateListViewPlugins()
        {
            foreach (Plugin p in controllerPlugins.listPlugins)
            {
                ListViewItem lvi = new ListViewItem(p.Name);
                lvi.UseItemStyleForSubItems = false;
                string status;
                Color bgColor;
                switch (p.Installed)
                {
                    case PluginInstallationType.NOT_INSTALLED:
                        status = "Not Installed";
                        bgColor = ThemeManager.CurrentTheme.PluginStateColor_NOT_INSTALLED;
                        break;
                    case PluginInstallationType.INSTALLED:
                        status = "Installed";
                        bgColor = ThemeManager.CurrentTheme.PluginStateColor_INSTALLED;
                        break;
                    case PluginInstallationType.MANUALLY_INSTALLED:
                        status = "Manually Installed";
                        bgColor = ThemeManager.CurrentTheme.PluginStateColor_MANUALLY_INSTALLED;
                        break;
                    case PluginInstallationType.FILES_PRESENT:
                        status = "Files present";
                        bgColor = ThemeManager.CurrentTheme.PluginStateColor_FILES_PRESENT;
                        break;
                    case PluginInstallationType.INSTALLED_MODIFIED:
                        status = "Installed - Modified";
                        bgColor = ThemeManager.CurrentTheme.PluginStateColor_INSTALLED_MODIFIED;
                        break;
                    default:
                        status = "Unknown";
                        bgColor = ThemeManager.CurrentTheme.ListViewBackColor;
                        break;
                }

                lvi.SubItems.Add(status);
                lvi.SubItems[1].BackColor = bgColor;
                if (p.dbEntry.InstalledDate > 0)
                {
                    lvi.SubItems.Add(DateTimeOffset.FromUnixTimeMilliseconds(p.dbEntry.InstalledDate).LocalDateTime.ToString());
                }
                listViewPlugins.Items.Add(lvi);
            }
        }

        private void UpdatePluginActionButtonState()
        {
            if (controllerPlugins.getObsPath() == string.Empty || listViewPlugins.SelectedIndices.Count == 0)
            {
                buttonAdd.Enabled = false;
                buttonRemove.Enabled = false;
                buttonMarkNotInstalled.Enabled = false;
                return;
            }
            labelWarnings.Visible = false;
            ListViewItem lvi = listViewPlugins.SelectedItems[0];
            switch (controllerPlugins.getInstallStateOfPlugin(lvi.Text))
            {
                case PluginInstallationType.NOT_INSTALLED:
                    buttonAdd.Enabled = true;
                    buttonRemove.Enabled = false;
                    buttonMarkNotInstalled.Enabled = false;
                    break;
                case PluginInstallationType.INSTALLED:
                    buttonAdd.Enabled = false;
                    buttonRemove.Enabled = true;
                    buttonMarkNotInstalled.Enabled = false;
                    break;
                case PluginInstallationType.MANUALLY_INSTALLED:
                    buttonAdd.Enabled = true;
                    buttonRemove.Enabled = true;
                    buttonMarkNotInstalled.Enabled = false;
                    labelWarnings.Text = "All files from the Zip are already present in the OBS directory, but they were not installed by the Plugin Manager";
                    labelWarnings.Visible = true;
                    break;
                case PluginInstallationType.FILES_PRESENT:
                    buttonAdd.Enabled = true;
                    buttonRemove.Enabled = true;
                    buttonMarkNotInstalled.Enabled = false;
                    labelWarnings.Text = "Not all files from the Zip are present in the OBS directory, or they do not match the existing files.\r\nLikely a different version of this plugin is installed";
                    labelWarnings.Visible = true;
                    break;
                case PluginInstallationType.INSTALLED_MODIFIED:
                    buttonAdd.Enabled = true;
                    buttonRemove.Enabled = true;
                    buttonMarkNotInstalled.Enabled = true;
                    labelWarnings.Text = "Not all files from the Zip are present in the OBS directory, or they do not match the existing files.\r\nLikely a different version of this plugin was installed after installation by the Plugin Manager";
                    labelWarnings.Visible = true;
                    break;
                default:
                    buttonAdd.Enabled = false;
                    buttonRemove.Enabled = false;
                    buttonMarkNotInstalled.Enabled = false;
                    break;
            }
        }

        private void CheckOBSRunningState()
        {
            var prevOBSState = OBSRunningState;
            OBSRunningStateType tempRunning = OBSRunningStateType.NotRunning;
            if (controllerPlugins.getObsPath() != string.Empty)
            {
                List<Process> procs = new List<Process>();
                procs.AddRange(Process.GetProcessesByName("obs64"));
                procs.AddRange(Process.GetProcessesByName("obs")); // 32-bit executable name
                foreach (Process p in procs)
                {
                    try
                    {
                        if (p.MainModule.FileName.StartsWith(controllerPlugins.getObsPath(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            tempRunning = OBSRunningStateType.ExactRunning;
                            break;
                        }
                    }
                    catch (Win32Exception)
                    {
                        tempRunning = OBSRunningStateType.AdminRunning;
                    }
                }
            }
            OBSRunningState = tempRunning;
            if (prevOBSState != OBSRunningState)
            {
                switch (OBSRunningState)
                {
                    case OBSRunningStateType.ExactRunning:
                        labelOBSWarning.Visible = true;
                        labelOBSWarning.Text = "Warning:\r\nThis OBS installation is currently running!";
                        break;
                    case OBSRunningStateType.AdminRunning:
                        labelOBSWarning.Visible = true;
                        labelOBSWarning.Text = "Warning:\r\nAn elevated OBS installation is currently running. Unable to check whether it is the selected OBS path.";
                        break;
                    default:
                        labelOBSWarning.Visible = false;
                        break;
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // Enable multi-Pick
            if (controllerPlugins.getObsPath() == string.Empty)
            {
                return;
            }
            if (listViewPlugins.SelectedItems.Count == 0)
            {
                return;
            }
            if (AbortActionOverride()) { return; }
            ListViewItem lvi = listViewPlugins.SelectedItems[0];
            Debug.WriteLine("Plugin to remove: " + lvi.Text);
            if (!controllerPlugins.uninstallPlugin(lvi.Text))
            {
                MessageBox.Show($"Failed to remove {lvi.Text}", "Unable to remove", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            listViewPlugins.Items.Clear();
            // Update installation state
            controllerPlugins.populatePluginLists();
            PopulateListViewPlugins();
        }

        private void buttonMarkNotInstalled_Click(object sender, EventArgs e)
        {
            // Enable multi-Pick
            if (controllerPlugins.getObsPath() == string.Empty)
            {
                return;
            }
            if (listViewPlugins.SelectedItems.Count == 0)
            {
                return;
            }
            ListViewItem lvi = listViewPlugins.SelectedItems[0];
            Debug.WriteLine(lvi.Text);
            controllerPlugins.markPluginUninstalled(lvi.Text);
            listViewPlugins.Items.Clear();
            PopulateListViewPlugins();
        }

        private void panelDragnDrop_DragDrop(object sender, DragEventArgs e)
        {
            if (controllerPlugins.pluginsPath == string.Empty)
            {
                MessageBox.Show("Set/Change the Plugins path", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        Debug.WriteLine(file);
                        if (!controllerPlugins.copyPluginZip(file))
                        {
                            MessageBox.Show($"Could not copy file {file}", "Could not copy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                //Refresh
                buttonReload.PerformClick();
            }
            panelDragnDrop.Visible = false;
        }

        private void panelDragnDrop_DragEnter(object sender, DragEventArgs e)
        {
            panelDragnDrop.Visible = true;
            e.Effect = DragDropEffects.Copy;
            labelDrop.Visible = true;
            labelDrop.ForeColor = Color.Black;
            labelDrop.Text = "Drop your files to copy!";
            labelDrop.Location = new Point(60, 109);
        }

        private void panelDragnDrop_DragLeave(object sender, EventArgs e)
        {
            labelDrop.ForeColor = Color.DarkRed;
            labelDrop.Text = "Drop to CANCEL";
            labelDrop.Location = new Point(150, 109);
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            panelDragnDrop.Visible = false;
            labelDrop.Visible = false;
        }

        private void FormMain_DragOver(object sender, DragEventArgs e)
        {
            panelDragnDrop.Visible = true;
            labelDrop.Visible = true;
        }

        private void FormMain_DragLeave(object sender, EventArgs e)
        {
            panelDragnDrop.Visible = false;
            labelDrop.Visible = false;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            buttonSearch.PerformClick();
        }

        private void listViewPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePluginActionButtonState();
        }

        private void timerOBSCheck_Tick(object sender, EventArgs e)
        {
            CheckOBSRunningState();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ControlKey)
            {
                buttonAdd.Text = "Force Add Plugin";
                buttonRemove.Text = "Force Remove Plugin";
            }
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                buttonAdd.Text = "Add Plugin";
                buttonRemove.Text = "Remove Plugin";
            }
        }

        private void pictureSwitchTheme_Click(object sender, EventArgs e)
        {
            var newTheme = ThemeManager.CurrentTheme == Themes.Light ? Themes.Dark : Themes.Light;
            ThemeManager.SetTheme(newTheme, this);
            controllerPlugins.savePaths();
        }
    }
}