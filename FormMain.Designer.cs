namespace PluginManagerObs
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonObsPath = new Button();
            buttonPluginsPath = new Button();
            labelObsPath = new Label();
            labelPluginsPath = new Label();
            folderBrowserDialog = new FolderBrowserDialog();
            listViewPlugins = new ListView();
            columnHeaderName = new ColumnHeader();
            columnHeaderStatus = new ColumnHeader();
            columnHeaderDate = new ColumnHeader();
            textBoxSearch = new TextBox();
            buttonSearch = new Button();
            buttonReload = new Button();
            buttonAdd = new Button();
            buttonRemove = new Button();
            labelSign = new Label();
            panelDragnDrop = new Panel();
            labelDrop = new Label();
            timerOBSCheck = new System.Windows.Forms.Timer(components);
            labelWarnings = new Label();
            buttonMarkNotInstalled = new Button();
            labelOBSWarning = new Label();
            panelDragnDrop.SuspendLayout();
            SuspendLayout();
            // 
            // buttonObsPath
            // 
            buttonObsPath.Location = new Point(12, 323);
            buttonObsPath.Name = "buttonObsPath";
            buttonObsPath.Size = new Size(95, 23);
            buttonObsPath.TabIndex = 0;
            buttonObsPath.Text = "OBS Path";
            buttonObsPath.UseVisualStyleBackColor = true;
            buttonObsPath.Click += buttonObsPath_Click;
            // 
            // buttonPluginsPath
            // 
            buttonPluginsPath.Location = new Point(12, 352);
            buttonPluginsPath.Name = "buttonPluginsPath";
            buttonPluginsPath.Size = new Size(95, 23);
            buttonPluginsPath.TabIndex = 1;
            buttonPluginsPath.Text = "Plugins Path";
            buttonPluginsPath.UseVisualStyleBackColor = true;
            buttonPluginsPath.Click += buttonPluginsPath_Click;
            // 
            // labelObsPath
            // 
            labelObsPath.AutoSize = true;
            labelObsPath.Location = new Point(113, 327);
            labelObsPath.Name = "labelObsPath";
            labelObsPath.Size = new Size(73, 15);
            labelObsPath.TabIndex = 2;
            labelObsPath.Text = "Path Not Set";
            // 
            // labelPluginsPath
            // 
            labelPluginsPath.AutoSize = true;
            labelPluginsPath.Location = new Point(113, 356);
            labelPluginsPath.Name = "labelPluginsPath";
            labelPluginsPath.Size = new Size(73, 15);
            labelPluginsPath.TabIndex = 3;
            labelPluginsPath.Text = "Path Not Set";
            // 
            // listViewPlugins
            // 
            listViewPlugins.BackColor = SystemColors.Control;
            listViewPlugins.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderStatus, columnHeaderDate });
            listViewPlugins.FullRowSelect = true;
            listViewPlugins.Location = new Point(12, 41);
            listViewPlugins.MultiSelect = false;
            listViewPlugins.Name = "listViewPlugins";
            listViewPlugins.Size = new Size(609, 276);
            listViewPlugins.TabIndex = 4;
            listViewPlugins.UseCompatibleStateImageBehavior = false;
            listViewPlugins.View = View.Details;
            listViewPlugins.SelectedIndexChanged += listViewPlugins_SelectedIndexChanged;
            // 
            // columnHeaderName
            // 
            columnHeaderName.Text = "Name";
            columnHeaderName.Width = 320;
            // 
            // columnHeaderStatus
            // 
            columnHeaderStatus.Text = "Status";
            columnHeaderStatus.Width = 140;
            // 
            // columnHeaderDate
            // 
            columnHeaderDate.Text = "Date";
            columnHeaderDate.Width = 140;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(12, 12);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(447, 23);
            textBoxSearch.TabIndex = 5;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(465, 12);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(75, 23);
            buttonSearch.TabIndex = 6;
            buttonSearch.Text = "Search";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // buttonReload
            // 
            buttonReload.Location = new Point(546, 12);
            buttonReload.Name = "buttonReload";
            buttonReload.Size = new Size(75, 23);
            buttonReload.TabIndex = 7;
            buttonReload.Text = "Reload";
            buttonReload.UseVisualStyleBackColor = true;
            buttonReload.Click += buttonReload_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.Enabled = false;
            buttonAdd.Location = new Point(627, 41);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(186, 23);
            buttonAdd.TabIndex = 8;
            buttonAdd.Text = "Add Plugin";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRemove
            // 
            buttonRemove.Enabled = false;
            buttonRemove.Location = new Point(627, 70);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(186, 23);
            buttonRemove.TabIndex = 9;
            buttonRemove.Text = "Remove Plugin";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // labelSign
            // 
            labelSign.AutoSize = true;
            labelSign.Font = new Font("Courier New", 9F, FontStyle.Italic, GraphicsUnit.Point);
            labelSign.Location = new Point(668, 365);
            labelSign.Name = "labelSign";
            labelSign.Size = new Size(126, 16);
            labelSign.TabIndex = 10;
            labelSign.Text = "AshuraStrike 2023";
            // 
            // panelDragnDrop
            // 
            panelDragnDrop.AllowDrop = true;
            panelDragnDrop.BackColor = SystemColors.Control;
            panelDragnDrop.Controls.Add(labelDrop);
            panelDragnDrop.Location = new Point(12, 41);
            panelDragnDrop.Name = "panelDragnDrop";
            panelDragnDrop.Size = new Size(609, 276);
            panelDragnDrop.TabIndex = 11;
            panelDragnDrop.Visible = false;
            panelDragnDrop.DragDrop += panelDragnDrop_DragDrop;
            panelDragnDrop.DragEnter += panelDragnDrop_DragEnter;
            panelDragnDrop.DragLeave += panelDragnDrop_DragLeave;
            // 
            // labelDrop
            // 
            labelDrop.AutoSize = true;
            labelDrop.Font = new Font("Consolas", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
            labelDrop.Location = new Point(130, 109);
            labelDrop.Name = "labelDrop";
            labelDrop.Size = new Size(339, 43);
            labelDrop.TabIndex = 0;
            labelDrop.Text = "Drop files here!";
            labelDrop.Visible = false;
            // 
            // timerOBSCheck
            // 
            timerOBSCheck.Enabled = true;
            timerOBSCheck.Interval = 2000;
            timerOBSCheck.Tick += timerOBSCheck_Tick;
            // 
            // labelWarnings
            // 
            labelWarnings.AutoSize = true;
            labelWarnings.Location = new Point(627, 125);
            labelWarnings.MaximumSize = new Size(187, 0);
            labelWarnings.Name = "labelWarnings";
            labelWarnings.Size = new Size(174, 120);
            labelWarnings.TabIndex = 12;
            labelWarnings.Text = "Not all files from the Zip are present in the OBS directory, or they do not match the existing files.\r\nLikely a different version of this plugin was installed after installation by the Plugin Manager";
            labelWarnings.Visible = false;
            // 
            // buttonMarkNotInstalled
            // 
            buttonMarkNotInstalled.Enabled = false;
            buttonMarkNotInstalled.Location = new Point(627, 99);
            buttonMarkNotInstalled.Name = "buttonMarkNotInstalled";
            buttonMarkNotInstalled.Size = new Size(186, 23);
            buttonMarkNotInstalled.TabIndex = 9;
            buttonMarkNotInstalled.Text = "Clear Manager Installation Flag";
            buttonMarkNotInstalled.UseVisualStyleBackColor = true;
            buttonMarkNotInstalled.Click += buttonMarkNotInstalled_Click;
            // 
            // labelOBSWarning
            // 
            labelOBSWarning.AutoSize = true;
            labelOBSWarning.Location = new Point(627, 250);
            labelOBSWarning.MaximumSize = new Size(187, 0);
            labelOBSWarning.Name = "labelOBSWarning";
            labelOBSWarning.Size = new Size(174, 75);
            labelOBSWarning.TabIndex = 13;
            labelOBSWarning.Text = "Warning:\r\nAn elevated OBS installation is currently running. Unable to check whether it is the selected OBS path.";
            labelOBSWarning.Visible = false;
            // 
            // FormMain
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(825, 388);
            Controls.Add(labelOBSWarning);
            Controls.Add(labelWarnings);
            Controls.Add(labelSign);
            Controls.Add(buttonMarkNotInstalled);
            Controls.Add(buttonRemove);
            Controls.Add(buttonAdd);
            Controls.Add(buttonReload);
            Controls.Add(buttonSearch);
            Controls.Add(textBoxSearch);
            Controls.Add(panelDragnDrop);
            Controls.Add(listViewPlugins);
            Controls.Add(labelPluginsPath);
            Controls.Add(labelObsPath);
            Controls.Add(buttonPluginsPath);
            Controls.Add(buttonObsPath);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormMain";
            Text = "Plugin Manager for OBS";
            Load += FormMain_Load;
            DragDrop += FormMain_DragDrop;
            DragOver += FormMain_DragOver;
            DragLeave += FormMain_DragLeave;
            panelDragnDrop.ResumeLayout(false);
            panelDragnDrop.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonObsPath;
        private Button buttonPluginsPath;
        private Label labelObsPath;
        private Label labelPluginsPath;
        private FolderBrowserDialog folderBrowserDialog;
        private ListView listViewPlugins;
        private ColumnHeader columnHeaderName;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private Button buttonReload;
        private Button buttonAdd;
        private ColumnHeader columnHeaderStatus;
        private Button buttonRemove;
        private Label labelSign;
        private ColumnHeader columnHeaderDate;
        private Panel panelDragnDrop;
        private Label labelDrop;
        private System.Windows.Forms.Timer timerOBSCheck;
        private Label labelWarnings;
        private Button buttonMarkNotInstalled;
        private Label labelOBSWarning;
    }
}