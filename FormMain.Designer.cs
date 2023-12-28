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
            buttonObsPath = new Button();
            buttonPluginsPath = new Button();
            labelObsPath = new Label();
            labelPluginsPath = new Label();
            folderBrowserDialog = new FolderBrowserDialog();
            listViewPlugins = new ListView();
            columnHeaderName = new ColumnHeader();
            columnHeaderStatus = new ColumnHeader();
            textBoxSearch = new TextBox();
            buttonSearch = new Button();
            buttonReload = new Button();
            buttonAdd = new Button();
            buttonRemove = new Button();
            labelSign = new Label();
            SuspendLayout();
            // 
            // buttonObsPath
            // 
            buttonObsPath.Location = new Point(12, 505);
            buttonObsPath.Name = "buttonObsPath";
            buttonObsPath.Size = new Size(95, 23);
            buttonObsPath.TabIndex = 0;
            buttonObsPath.Text = "Obs Path";
            buttonObsPath.UseVisualStyleBackColor = true;
            buttonObsPath.Click += buttonObsPath_Click;
            // 
            // buttonPluginsPath
            // 
            buttonPluginsPath.Location = new Point(12, 534);
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
            labelObsPath.Location = new Point(113, 509);
            labelObsPath.Name = "labelObsPath";
            labelObsPath.Size = new Size(73, 15);
            labelObsPath.TabIndex = 2;
            labelObsPath.Text = "Path Not Set";
            // 
            // labelPluginsPath
            // 
            labelPluginsPath.AutoSize = true;
            labelPluginsPath.Location = new Point(113, 538);
            labelPluginsPath.Name = "labelPluginsPath";
            labelPluginsPath.Size = new Size(73, 15);
            labelPluginsPath.TabIndex = 3;
            labelPluginsPath.Text = "Path Not Set";
            // 
            // listViewPlugins
            // 
            listViewPlugins.BackColor = SystemColors.ControlDark;
            listViewPlugins.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderStatus });
            listViewPlugins.FullRowSelect = true;
            listViewPlugins.Location = new Point(12, 41);
            listViewPlugins.MultiSelect = false;
            listViewPlugins.Name = "listViewPlugins";
            listViewPlugins.Size = new Size(609, 458);
            listViewPlugins.TabIndex = 4;
            listViewPlugins.UseCompatibleStateImageBehavior = false;
            listViewPlugins.View = View.Details;
            // 
            // columnHeaderName
            // 
            columnHeaderName.Text = "Name";
            columnHeaderName.Width = 320;
            // 
            // columnHeaderStatus
            // 
            columnHeaderStatus.Text = "Status";
            columnHeaderStatus.Width = 80;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(12, 12);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(447, 23);
            textBoxSearch.TabIndex = 5;
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
            buttonAdd.Location = new Point(627, 41);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(75, 23);
            buttonAdd.TabIndex = 8;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(627, 70);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(75, 23);
            buttonRemove.TabIndex = 9;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // labelSign
            // 
            labelSign.AutoSize = true;
            labelSign.Font = new Font("Courier New", 9F, FontStyle.Italic, GraphicsUnit.Point);
            labelSign.Location = new Point(576, 544);
            labelSign.Name = "labelSign";
            labelSign.Size = new Size(126, 16);
            labelSign.TabIndex = 10;
            labelSign.Text = "AshuraStrike 2023";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(714, 569);
            Controls.Add(labelSign);
            Controls.Add(buttonRemove);
            Controls.Add(buttonAdd);
            Controls.Add(buttonReload);
            Controls.Add(buttonSearch);
            Controls.Add(textBoxSearch);
            Controls.Add(listViewPlugins);
            Controls.Add(labelPluginsPath);
            Controls.Add(labelObsPath);
            Controls.Add(buttonPluginsPath);
            Controls.Add(buttonObsPath);
            Name = "FormMain";
            Text = "Plugin Manager for OBS";
            Load += FormMain_Load;
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
    }
}