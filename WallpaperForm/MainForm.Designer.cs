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
            lb_cycle = new Label();
            tb_cycle = new TextBox();
            lb_addr = new Label();
            tb_addr = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 101);
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
            LogTextBox.Location = new Point(0, 121);
            LogTextBox.Name = "LogTextBox";
            LogTextBox.Size = new Size(411, 397);
            LogTextBox.TabIndex = 2;
            LogTextBox.Text = "";
            // 
            // lb_cycle
            // 
            lb_cycle.AutoSize = true;
            lb_cycle.Location = new Point(0, 46);
            lb_cycle.Name = "lb_cycle";
            lb_cycle.Size = new Size(68, 17);
            lb_cycle.TabIndex = 1;
            lb_cycle.Text = "更新周期：";
            // 
            // tb_cycle
            // 
            tb_cycle.Location = new Point(60, 43);
            tb_cycle.Name = "tb_cycle";
            tb_cycle.Size = new Size(339, 23);
            tb_cycle.TabIndex = 3;
            // 
            // lb_addr
            // 
            lb_addr.AutoSize = true;
            lb_addr.Location = new Point(0, 15);
            lb_addr.Name = "lb_addr";
            lb_addr.Size = new Size(68, 17);
            lb_addr.TabIndex = 1;
            lb_addr.Text = "接口地址：";
            // 
            // tb_addr
            // 
            tb_addr.Location = new Point(60, 12);
            tb_addr.Name = "tb_addr";
            tb_addr.Size = new Size(339, 23);
            tb_addr.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(142, 72);
            button1.Name = "button1";
            button1.Size = new Size(75, 29);
            button1.TabIndex = 4;
            button1.Text = "重置参数";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(243, 72);
            button2.Name = "button2";
            button2.Size = new Size(75, 29);
            button2.TabIndex = 4;
            button2.Text = "应用参数";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 519);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(tb_addr);
            Controls.Add(tb_cycle);
            Controls.Add(lb_addr);
            Controls.Add(LogTextBox);
            Controls.Add(lb_cycle);
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
        private Label lb_cycle;
        private TextBox tb_cycle;
        private Label lb_addr;
        private TextBox tb_addr;
        private Button button1;
        private Button button2;
    }
}
