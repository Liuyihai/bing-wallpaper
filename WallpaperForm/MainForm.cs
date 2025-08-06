using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperForm
{
    public partial class MainForm : Form
    {
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
            //Console.WriteLine("�������");
            LogTextBox.AppendText("�������\n");
            // ��������ͼ��
            icon = new NotifyIcon
            {
                Icon = new Icon("bing.ico"),
                Visible = true,
                Text = "��ֽ����"
            };
            // ����Ҽ��˵�
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("��ʾ����", null, (s, args) => { this.Show(); this.WindowState = FormWindowState.Normal; });
            contextMenu.Items.Add("�˳�", null, (s, args) => Application.Exit());
            icon.ContextMenuStrip = contextMenu;
            // ��������ͼ���˫���¼�
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
            //Console.WriteLine("��ʼ���ر�ֽ");
            LogTextBox.AppendText("��ʼ���ر�ֽ\n");
            DownloadWallpaperAsync();
        }

        private async Task DownloadWallpaperAsync()
        {
            try
            {
                //Console.WriteLine($"���ڴ�{url}����ͼƬ...");
                LogTextBox.AppendText($"���ڴ�{url}����ͼƬ...\n");
                string fileName = "Wallpaper.jpeg";
                fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                byte[] imageBytes = await client.GetByteArrayAsync(url);
                File.WriteAllBytes(fileName, imageBytes);
                //Console.WriteLine("��ֽ�������");
                LogTextBox.AppendText("��ֽ�������\n");
                RefreshWallpaper();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"����ͼƬ����: {ex.Message}");
                LogTextBox.AppendText($"Error: {ex.Message}\n");
                //MessageBox.Show($"����ͼƬ����: {ex.Message}", "����");
            }
        }

        private void RefreshWallpaper()
        {
            try
            {
                //Console.WriteLine("��ʼˢ��");
                LogTextBox.AppendText("��ʼˢ��\n");
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
                    FileName = "cmd.exe", // Ҫִ�еĳ���
                    RedirectStandardInput = true, // �ض����������ѡ��
                    UseShellExecute = false, // ���� Shell ִ��
                    CreateNoWindow = true, // ����������
                    WindowStyle = ProcessWindowStyle.Hidden // ���ش���
                };
                using (Process process = Process.Start(startInfo))
                {
                    process.StandardInput.WriteLine("ASSOC .tmp=tmpfile");
                    process.StandardInput.WriteLine("exit");
                    process.StandardInput.AutoFlush = true; // ȷ�����뱻ˢ��

                    process.WaitForExit(); // �ȴ�����ִ�����
                    process.Close();
                }

                //Console.WriteLine("�㶨");
                LogTextBox.AppendText("�㶨\n");
            }
            catch (Exception err)
            {
                LogTextBox.AppendText($"Error: {err.Message}\n");
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb_addr.Text = "https://bing.img.run/rand_uhd.php";
            tb_cycle.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //check the link in tb_addr,if the link is not empty, then check if the link is valid
            if (!string.IsNullOrEmpty(tb_addr.Text))
            {
                try
                {
                    Uri uri = new Uri(tb_addr.Text);
                    url = uri.ToString();
                    LogTextBox.AppendText($"�����Ѹ���: {url}\n");
                }
                catch (UriFormatException)
                {
                    LogTextBox.AppendText("��Ч�����Ӹ�ʽ���������롣\n");
                }
            }
            else
            {
                LogTextBox.AppendText("���Ӳ���Ϊ�գ���������Ч�����ӡ�\n");
            }

            //check the cycle in tb_cycle,if the cycle is not empty, then check if the cycle is valid
            if (!string.IsNullOrEmpty(tb_cycle.Text))
            {
                if (double.TryParse(tb_cycle.Text, out double cycle) && cycle > 0)
                {
                    wallpaperDownloader.Interval = (int)cycle * 60 * 1000; // Convert minutes to milliseconds
                    LogTextBox.AppendText($"��������������Ϊ: {cycle} ��\n");
                }
                else
                {
                    LogTextBox.AppendText("��Ч�����ڣ�������һ����������\n");
                }
            }
            else
            {
                LogTextBox.AppendText("���ڲ���Ϊ�գ���������Ч�����ڡ�\n");
            }

        }
    }
}
