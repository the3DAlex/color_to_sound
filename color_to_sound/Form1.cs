using RandomAudioGenerator;

namespace color_to_sound
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int getHue(int red, int green, int blue)
        {
            float min = Math.Min(Math.Min(red, green), blue);
            float max = Math.Max(Math.Max(red, green), blue);

            if (min == max)
            {
                return 0;
            }

            double hue = 0f;
            if (max == red)
            {
                hue = (green - blue) / (max - min);

            }
            else if (max == green)
            {
                hue = 2f + (blue - red) / (max - min);

            }
            else
            {
                hue = 4f + (red - green) / (max - min);
            }

            hue = hue * 60;
            if (hue < 0) hue = hue + 360;

            return (int)Math.Round(hue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float quant = 0.001f;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(ofd.FileName);

                List<byte[]> data = new List<byte[]>();

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        var c = bmp.GetPixel(x, y);
                        int h = getHue(c.R, c.G, c.B) * 55;
                        data.Add(wav.CreateSinWave(44100, h, quant));
                    }
                }
                bmp.Dispose();  

                //-- create wav from hue
                var blockSize = data[0].Length;
                byte[] finalData = new byte[data.Count * blockSize];
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < blockSize; j++)
                    {
                        finalData[i * blockSize + j] = data[i][j];
                    }
                }
              
                
                using (FileStream fs = new FileStream("MySound2.wav", FileMode.Create))
                {
                    wav.WriteHeader(fs, finalData.Length, 1, 44100);
                    fs.Write(finalData, 0, finalData.Length);
                    fs.Close();
                }
                
            }
            
        }
    }
}