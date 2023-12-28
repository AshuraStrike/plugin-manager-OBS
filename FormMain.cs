using PluginManagerObs.Classes;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PluginManagerObs
{
    public partial class FormMain : Form
    {
        ControllerPlugins controllerPlugins = new();
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonObsPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // controllerPlugins -> validate OBS folder == Look for OBS bin
                controllerPlugins.obsPath = folderBrowserDialog.SelectedPath + '\\';
                labelObsPath.Text = controllerPlugins.obsPath;
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
                labelObsPath.Text = controllerPlugins.obsPath;
                labelPluginsPath.Text = controllerPlugins.pluginsPath;
                controllerPlugins.populatePlugins();
                PopulateListViewPlugins();
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = string.Empty;
            listViewPlugins.Items.Clear();
            bool refreshed = controllerPlugins.populatePlugins();
            if (refreshed)
            {
                PopulateListViewPlugins();
            }
            else
            {
                MessageBox.Show("Set the Plugins path", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Enable multi-Pick
            if (controllerPlugins.obsPath == string.Empty)
            {
                MessageBox.Show("OBS path not set", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (listViewPlugins.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Pick one plugin", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ListViewItem lvi = listViewPlugins.SelectedItems[0];
                    Debug.WriteLine(lvi.Text);
                    if (controllerPlugins.addPlugins(lvi.Text))
                    {
                        listViewPlugins.Items.Clear();
                        PopulateListViewPlugins();
                    }
                }
            }
        }

        private void PopulateListViewPlugins()
        {
            foreach (Plugin p in controllerPlugins.listPlugins)
            {
                ListViewItem lvi = new ListViewItem(p.Name);
                lvi.UseItemStyleForSubItems = false;

                string status = "Not Installed";
                Color bgColor = Color.Red;
                if (p.IsInstalled)
                {
                    status = "Installed";
                    bgColor = Color.Green;
                }

                lvi.SubItems.Add(status);
                lvi.SubItems[1].BackColor = bgColor;
                listViewPlugins.Items.Add(lvi);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // Enable multi-Pick
            if (controllerPlugins.obsPath == string.Empty)
            {
                MessageBox.Show("OBS path not set", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (listViewPlugins.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Pick one plugin", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ListViewItem lvi = listViewPlugins.SelectedItems[0];
                    Debug.WriteLine(lvi.Text);
                    if (controllerPlugins.uninstallPlugin(lvi.Text))
                    {
                        listViewPlugins.Items.Clear();
                        PopulateListViewPlugins();
                    }
                }
            }
        }
    }
}