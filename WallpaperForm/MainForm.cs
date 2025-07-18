using SkiaSharp;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperForm
{
    public partial class MainForm : Form
    {
        private static string todo = "���������㣡���ú�ˬ����";
        private static string fileHash = string.Empty;
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
            todoReader.Enabled = true;

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
            contextMenu.Items.Add("�˳�", null, (s, args) => Application.Exit());
            contextMenu.Items.Add("��ʾ����", null, (s,args) => { this.Show(); this.WindowState = FormWindowState.Normal; });
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
            DownLoadWallpaper(sender, e);
        }

        private async Task DownLoadWallpaper(object sender, EventArgs e)
        {
            await DownloadWallpaperAsync();

            UpdateWallpaper(todo, false, 40);

        }


        private void TextOnWallpaper(object sender, EventArgs e)
        {
            if (File.Exists(todoFilePath))
            {
                try
                {
                    // ��ȡ�ļ����ݲ������ϣ
                    byte[] fileBytes = File.ReadAllBytes(todoFilePath);
                    string currentHash;
                    using (var sha256 = System.Security.Cryptography.SHA256.Create())
                    {
                        byte[] hashBytes = sha256.ComputeHash(fileBytes);
                        currentHash = BitConverter.ToString(hashBytes).Replace("-", "");
                    }

                    // �жϹ�ϣ�Ƿ�仯
                    if (currentHash != fileHash)
                    {
                        fileHash = currentHash;
                        todo = File.ReadAllText(todoFilePath, Encoding.UTF8);
                        //Console.WriteLine("todo�ļ��Ѹ��£������Ѷ�ȡ��");
                        LogTextBox.AppendText($"todo�ļ��Ѹ��£������Ѷ�ȡ��{todo}\n");
                        UpdateWallpaper(todo, false, 40);
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"��ȡtodo�ļ�ʱ����: {ex.Message}");
                    MessageBox.Show($"��ȡtodo�ļ�ʱ����: {ex.Message}", "����");
                }
            }
            else
            {
                //Console.WriteLine("todo������");
                LogTextBox.AppendText("todo�ļ������ڣ��������洴��һ����Ϊtodo.txt���ļ���\n");
            }

        }

        private void UpdateWallpaper(string text, bool isCenter, int fontSize)
        {
            //Console.WriteLine("���±�ֽ...");
            LogTextBox.AppendText("���±�ֽ...\n");
            using (var image = SKBitmap.Decode("Wallpaper.jpeg"))
            {
                //read the txt file "C:\Users\shliu\Desktop\todo.txt" and get the longest line
                string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var len = lines.ToArray().Max(i => i.Length);
                var pointX = image.Width - len * (fontSize + 5);

                using (var surface = SKSurface.Create(new SKImageInfo(image.Width, image.Height)))
                {
                    //Console.WriteLine("��ʼ������");
                    LogTextBox.AppendText("��ʼ������\n");
                    var canvas = surface.Canvas;
                    canvas.Clear(SKColors.Transparent);
                    canvas.DrawBitmap(image, 0, 0);
                    int i = 1;
                    canvas.DrawText(SKTextBlob.Create("��������", new SKFont { Size = fontSize, Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Normal) }), pointX, fontSize * 1.5f * (i + 1),
                        new SKPaint
                        {
                            Color = SKColors.Blue,
                            IsAntialias = true,
                            TextSize = fontSize,
                            IsStroke = false,
                            TextAlign = isCenter ? SKTextAlign.Center : SKTextAlign.Left,
                            Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Normal)
                        });
                    foreach (var line in lines)
                    {
                        canvas.DrawText(SKTextBlob.Create(line, new SKFont { Size = fontSize, Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Normal) }), pointX, fontSize * 1.5f * (i + 2),
                        new SKPaint
                        {
                            Color = SKColors.Blue,
                            IsAntialias = true,
                            TextSize = fontSize,
                            IsStroke = false,
                            TextAlign = isCenter ? SKTextAlign.Center : SKTextAlign.Left,
                            Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Normal)
                        });
                        i++;
                    }
                    using (var imageResult = surface.Snapshot())
                    {
                        using (var data = imageResult.Encode(SKEncodedImageFormat.Jpeg, 100))
                        {
                            using (var stream = File.OpenWrite("Wallpaper.jpeg"))
                            {
                                data.SaveTo(stream);
                            }
                        }
                    }
                }
                //Console.WriteLine("�ҷ����ұ����ˣ�");
                LogTextBox.AppendText("�ҷ����ұ����ˣ�\n");
            }

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
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"����ͼƬ����: {ex.Message}");
                MessageBox.Show($"����ͼƬ����: {ex.Message}", "����");
            }
        }
    }
}
