namespace WallpaperForm
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            wallpaperDownloader = new System.Windows.Forms.Timer(components);
            timer2 = new System.Windows.Forms.Timer(components);
            LogTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 1);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 1;
            label1.Text = "运行日志";
            // 
            // wallpaperDownloader
            // 
            wallpaperDownloader.Interval = 60000;
            wallpaperDownloader.Tick += DownLoadBingWallpaper;
            // 
            // LogTextBox
            // 
            LogTextBox.Location = new Point(0, 21);
            LogTextBox.Name = "LogTextBox";
            LogTextBox.Size = new Size(411, 397);
            LogTextBox.TabIndex = 2;
            LogTextBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 419);
            Controls.Add(LogTextBox);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Bing壁纸";
            Deactivate += Form1_Deactivate;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();


        }
        // Fix for CS0123: Ensure the method signature matches the EventHandler delegate
        // Change the method signature of DownloadWallpaperAsync to match the EventHandler delegate

        #endregion
        private Label label1;
        private System.Windows.Forms.Timer wallpaperDownloader;
        private System.Windows.Forms.Timer timer2;
        private RichTextBox LogTextBox;
    }
}
