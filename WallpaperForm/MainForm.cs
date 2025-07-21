using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperForm
{
    public partial class MainForm : Form
    {
        private static string todoFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\todo.txt";

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private HttpClient client;
        private string url = "https://bing.img.run/rand_uhd.php";

        NotifyIcon icon;

        public MainForm()
        {
            InitializeComponent();
            
            Window_PreLoad();
        }

        private void Window_PreLoad()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            client = new HttpClient(handler);

            wallpaperDownloader.Enabled = true;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("加载完成");
            LogTextBox.AppendText("加载完成\n");
            // 创建托盘图标
            icon = new NotifyIcon
            {
                Icon = new Icon("bing.ico"),
                Visible = true,
                Text = "壁纸服务"
            };
            // 添加右键菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("退出", null, (s, args) => Application.Exit());
            contextMenu.Items.Add("显示窗口", null, (s,args) => { this.Show(); this.WindowState = FormWindowState.Normal; });
            icon.ContextMenuStrip = contextMenu;
            // 设置托盘图标的双击事件
            icon.DoubleClick += (s, args) => { this.Show(); this.WindowState = FormWindowState.Normal; };

            this.ShowInTaskbar = false;
            this.icon.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); 
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void DownLoadBingWallpaper(object sender, EventArgs e)
        {
            //Console.WriteLine("开始下载壁纸");
            LogTextBox.AppendText("开始下载壁纸\n");
            DownloadWallpaperAsync();
        }

        private async Task DownloadWallpaperAsync()
        {
            try
            {
                //Console.WriteLine($"正在从{url}下载图片...");
                LogTextBox.AppendText($"正在从{url}下载图片...\n");
                string fileName = "Wallpaper.jpeg";
                fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                byte[] imageBytes = await client.GetByteArrayAsync(url);
                File.WriteAllBytes(fileName, imageBytes);
                //Console.WriteLine("壁纸保存完毕");
                LogTextBox.AppendText("壁纸保存完毕\n");
                RefreshWallpaper();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"下载图片出错: {ex.Message}");
                LogTextBox.AppendText($"Error: {ex.Message}\n");
                //MessageBox.Show($"下载图片出错: {ex.Message}", "错误");
            }
        }

        private void RefreshWallpaper()
        {
            try
            {
                //Console.WriteLine("开始刷新");
                LogTextBox.AppendText("开始刷新\n");
                //_ = SystemParametersInfo(20, 0, "Wallpaper.jpeg", 1);

                //copy the file to the TranscodedWallpaper path
                string transcodedWallpaperPath = @"C:\Users\shliu\AppData\Roaming\Microsoft\Windows\Themes\TranscodedWallpaper";
                if (File.Exists(transcodedWallpaperPath))
                {
                    File.Delete(transcodedWallpaperPath);
                }
                File.Copy(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Wallpaper.jpeg"), transcodedWallpaperPath);
                // Set the desktop wallpaper using P/Invoke
                SystemParametersInfo(20, 0, transcodedWallpaperPath, 3);

                /*var process = Process.Start("cmd.exe", "ASSOC .tmp=tmpfile");
                Thread.Sleep(1000);
                process.Close();*/

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // 要执行的程序
                    RedirectStandardInput = true, // 重定向输出（可选）
                    UseShellExecute = false, // 禁用 Shell 执行
                    CreateNoWindow = true, // 不创建窗口
                    WindowStyle = ProcessWindowStyle.Hidden // 隐藏窗口
                };
                using (Process process = Process.Start(startInfo))
                {
                    process.StandardInput.WriteLine("ASSOC .tmp=tmpfile");
                    process.StandardInput.WriteLine("exit");
                    process.StandardInput.AutoFlush = true; // 确保输入被刷新

                    process.WaitForExit(); // 等待命令执行完成
                    process.Close();
                }

                //Console.WriteLine("搞定");
                LogTextBox.AppendText("搞定\n");
            }
            catch(Exception err)
            {
                LogTextBox.AppendText($"Error: {err.Message}\n");
            }
            
        }
    }
}
