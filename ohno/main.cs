using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace ohno
{
    public partial class main : Form
    {

        private static bool isPopUP = true;
        private static bool isDVDThingy = false;
        private static bool isScreenshot = true;
        private static bool isScreenshotInverted = true;

        private static int ScreenshotWidth = Screen.PrimaryScreen.Bounds.Width;
        private static int ScreenshotHeight = Screen.PrimaryScreen.Bounds.Height;
        private static int ScreenshotDist = 50;

        public main()
        {
            InitializeComponent();
        }

        Random rnd = new Random();
        float vel = 0.005f; // fraction of screen width per step
        float velX = 0;
        float velY = 0;
        int cornerCount = 0; // how often have we hit the corner

        int r = 255, g = 0, b = 0;

        private void main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BackColor = Color.Green;
            this.TransparencyKey = Color.Green;

            velX = velY = Math.Max(vel * Width, 1);
            label1.Location = new Point(rnd.Next(0, Width - label1.Width), rnd.Next(0, Height - label1.Height));

            postimer.Start();
            poptimer.Start();
            screenshottimer.Start();

            pictureBox.Location = new Point(ScreenshotDist / 2, ScreenshotDist / 2);
            pictureBox.Size = new Size(ScreenshotWidth - ScreenshotDist, ScreenshotHeight - ScreenshotDist);
        }

        private void poptimer_Tick(object sender, EventArgs e)
        {
            if (isPopUP)
            {
                Form PopUp = new popup();
                PopUp.Show();
                PopUp.Location = label1.Location;
                PopUp.BackColor = label1.ForeColor;
                PopUp.Text = RandomString(rnd.Next(1,50));
            }
        }

        private static Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        private void screenshottimer_Tick(object sender, EventArgs e)
        {
            if (isScreenshot)
            {
                Bitmap captureBitmap = new Bitmap(ScreenshotWidth, ScreenshotHeight, PixelFormat.Format32bppArgb);
                Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

                if (isScreenshotInverted)
                {
                    for (int y = 0; (y <= (captureBitmap.Height - 1)); y++)
                    {
                        for (int x = 0; (x <= (captureBitmap.Width - 1)); x++)
                        {
                            Color inv = captureBitmap.GetPixel(x, y);
                            inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                            captureBitmap.SetPixel(x, y, inv);
                        }
                    }
                }
                pictureBox.Image = captureBitmap;

            }
        }

        private void postimer_Tick(object sender, EventArgs e)
        {
            var pos = label1.Location;
            pos.X += (int)velX;
            pos.Y += (int)velY;

            int count = 0;
            if (pos.X < 0)
            {
                velX = Math.Abs(velX);
                count++;
            }
            if (pos.Y < 0)
            {
                velY = Math.Abs(velY);
                count++;
            }
            if (pos.X > Width - label1.Width)
            {
                velX = -Math.Abs(velX);
                count++;
            }
            if (pos.Y > Height - label1.Height)
            {
                velY = -Math.Abs(velY);
                count++;
            }

            if (r > 0 && b == 0)
            {
                r--;
                g++;
            }
            if (g > 0 && r == 0)
            {
                g--;
                b++;
            }
            if (b > 0 && g == 0)
            {
                b--;
                r++;
            }

            if (count == 2)
            {
                this.cornerCount++;
            }

            if (isDVDThingy)
            {
                label1.Text = cornerCount.ToString();
            } else
            {
                label1.Text = "               ";
            }

            label1.Location = pos;
            label1.ForeColor = Color.FromArgb(255, r, g, b);
            this.TopMost = false;
        }
    }
}
