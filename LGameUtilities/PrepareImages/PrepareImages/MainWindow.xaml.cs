using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using LG.Data;
using LG.XmlData;

namespace PrepareImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PictureBox> picBoxes = new List<PictureBox>();
        DataLoader loader;
        public MainWindow()
        {
            InitializeComponent();
            hh();
        }
        class Product
        {

        }
        async private Task hh()
        {
            
            string s = System.IO.Path.GetFullPath("data");
            loader = new DataLoader(s);
            List<Language> langs = await loader.GetLanguages();
            List<Theme> themes = null;
            try
            {
                themes = await loader.GetThemesByLangWithoutChildren(langs[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (Theme theme in themes)
            {
                cbThemes.Items.Add(theme);
            }

        }
        public void ConvertSize(string sourceVideo, string outputVideo, System.Windows.Size newSize, string ffMpegUtil)
        {
            try
            {
                //GetMediaInfo(sourceVideo);

                Process ffMpegProc = new Process();
                ffMpegProc.ErrorDataReceived += ffMpegProc_ErrorDataReceived;
                ffMpegProc.OutputDataReceived += ffMpegProc_OutputDataReceived;
                ffMpegProc.StartInfo.RedirectStandardError = true;
                ffMpegProc.StartInfo.RedirectStandardInput = true;
                ffMpegProc.StartInfo.RedirectStandardOutput = true;
                ffMpegProc.StartInfo.Arguments = " -i \"" + sourceVideo + "\"" + " -s " + newSize.Width + "x" + newSize.Height + " \"" + outputVideo + "\"";
                ffMpegProc.StartInfo.FileName = ffMpegUtil;
                ffMpegProc.StartInfo.CreateNoWindow = true;
                ffMpegProc.StartInfo.UseShellExecute = false;
                ffMpegProc.Start();
                ffMpegProc.BeginErrorReadLine();
                ffMpegProc.BeginOutputReadLine();
                ffMpegProc.WaitForExit(6000000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ffMpegProc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
        }

        private void ffMpegProc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
        }

        private void Browse_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> convertedImages = new List<string>();
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.InitialDirectory = System.IO.Path.GetFullPath("pictures");
                if ((bool)ofd.ShowDialog())
                {
                    if (ofd.FileNames.Count() > 0)
                    {
                        lbFiles.Items.Clear();
                        picBoxes.Clear();
                        spImages.Children.Clear();

                        string destinationFolder = "";
                        if (ofd.FileNames.Count() > 0)
                        {
                            destinationFolder = Path.GetDirectoryName(ofd.FileNames[0]) + "\\" + Path.GetFileNameWithoutExtension(ofd.FileNames[0]);
                            try
                            {
                                Directory.CreateDirectory(destinationFolder);
                            }
                            catch
                            {

                            }
                        }
                        if (!string.IsNullOrEmpty(destinationFolder))
                        {
                            foreach (string fileName in ofd.FileNames)
                            {

                                try
                                {
                                    lbFiles.Items.Add(fileName);
                                    double thumbnailWidth = 170;
                                    double thumbnailHeight = 120;

                                    double newHeight = 120;
                                    double newWidth = 170;
                                    Bitmap image = new Bitmap(fileName);
                                    bool isLandscape = image.Width >= image.Height;
                                    float aspectRation = (float)image.Width / (float)image.Height;

                                    if (isLandscape)
                                    {
                                        double kw = (double)image.Width / thumbnailWidth;
                                        newHeight = (double)image.Height / kw;
                                        if (newHeight > thumbnailHeight)
                                        {
                                            double kh = (double)image.Height / thumbnailHeight;
                                            newHeight = 120;
                                            newWidth = (double)image.Width / kh;
                                        }
                                    }
                                    else
                                    {
                                        double kh = (double)image.Height / thumbnailHeight;
                                        newWidth = (double)image.Width / kh;
                                        if (newWidth > thumbnailWidth)
                                        {
                                            double kw = (double)image.Width / thumbnailWidth;
                                            newWidth = 170;
                                            newHeight = (double)image.Height / kw;
                                        }
                                    }

                                    string convertName = destinationFolder + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".jpg";
                                    if (!File.Exists(convertName))
                                    {

                                        ConvertSize(fileName, convertName, new System.Windows.Size((double)Math.Round(newWidth, 0), (double)Math.Round(newHeight, 0)), "ffmpeg.exe");
                                    }
                                    convertedImages.Add(convertName);


                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        DisplayConvertedImages(convertedImages);
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayConvertedImages(List<string> imageNames)
        {
            StackPanel spRow = null;
            for (int i = 0; i < imageNames.Count; i++)
            {
                if (i % 5 == 0)
                {
                    spRow = new StackPanel();
                    spRow.Margin = new Thickness(5);
                    spRow.Orientation = Orientation.Horizontal;
                    spImages.Children.Add(spRow);
                }

                StackPanel spImage = new StackPanel();
                spRow.Children.Add(spImage);

                PictureBox box = new PictureBox(imageNames[i]);
                box.OnWordAdded += box_OnWordAdded;
                picBoxes.Add(box);
                box.Margin = new Thickness(5);
                spImage.Children.Add(box);


                box.PictureName = Path.GetFileName(imageNames[i]);
                box.PicturePath = imageNames[i];
            }
        }


        byte r;
        byte g;
        byte b;

        private void redSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (redSlider != null && greenSlider != null && blueSlider != null && tbBlue != null)
            {
                r = (byte)redSlider.Value;
                g = (byte)greenSlider.Value;
                b = (byte)blueSlider.Value;

                tbRed.Text = r.ToString();
                tbGreen.Text = g.ToString();
                tbBlue.Text = b.ToString();

                foreach (PictureBox box in this.picBoxes)
                {
                    if (box.chkBox.IsChecked != null)
                    {
                        if ((bool)box.chkBox.IsChecked)
                        {
                            box.BackgroundColor = System.Windows.Media.Color.FromArgb(255, r, g, b);
                        }
                    }
                }
            }
        }

        private void AjustColors_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color borderCol = TranslateteColor(17, 0);
            System.Windows.Media.Color leftCol = TranslateteColor(25, -51);
            System.Windows.Media.Color TopCol = TranslateteColor(51, -51);
            System.Windows.Media.Color TopLeftCol = TranslateteColor(51, 0);

            foreach (PictureBox box in this.picBoxes)
            {
                if (box.chkBox.IsChecked != null)
                {
                    if ((bool)box.chkBox.IsChecked)
                    {
                        box.BorderColor = borderCol;
                        box.LeftColor = leftCol;
                        box.TopColor = TopCol;
                        box.LeftTopColor = TopLeftCol;
                    }
                }
            }

        }

        private System.Windows.Media.Color TranslateteColor(int deltaMin, int deltaMax)
        {
            List<byte> colors = new List<byte>();
            colors.Add(r);
            colors.Add(g);
            colors.Add(b);

            int iMax;
            iMax = r > g ? 0 : 1;
            byte colMax = r > g ? r : g;
            iMax = colMax > b ? iMax : 2;

            colors[iMax] = colors[iMax] < 51 ? (byte)46 : colors[iMax];
            int deltaM;

            if (iMax == 0)
            {
                colors[1] = colors[1] > 204 ? (byte)204 : colors[1];
                colors[2] = colors[2] > 204 ? (byte)204 : colors[2];

                int delta1 = colors[iMax] - colors[1];
                int delta2 = colors[iMax] - colors[2];
                deltaM = (int)Math.Round((double)((delta1 + delta2) / 2 * deltaMax / 136), 0);

                delta1 = (int)Math.Round((double)(delta1 * deltaMin / 136), 0);
                delta2 = (int)Math.Round((double)(delta2 * deltaMin / 136), 0);


                colors[1] = (byte)(colors[1] + delta1);
                colors[2] = (byte)(colors[2] + delta2);

                deltaM = (int)Math.Round((double)((delta1 + delta2) / 2 * deltaMax / 136), 0);
            }
            else if (iMax == 1)
            {
                colors[0] = colors[0] > 204 ? (byte)204 : colors[0];
                colors[2] = colors[2] > 204 ? (byte)204 : colors[2];

                int delta1 = colors[iMax] - colors[0];
                int delta2 = colors[iMax] - colors[2];
                deltaM = (int)Math.Round((double)((delta1 + delta2) / 2 * deltaMax / 136), 0);

                delta1 = (int)Math.Round((double)(delta1 * deltaMin / 136), 0);
                delta2 = (int)Math.Round((double)(delta2 * deltaMin / 136), 0);

                colors[0] = (byte)(colors[0] + delta1);
                colors[2] = (byte)(colors[2] + delta2);


            }
            else
            {
                colors[0] = colors[0] > 204 ? (byte)204 : colors[0];
                colors[1] = colors[1] > 204 ? (byte)204 : colors[1];

                int delta1 = colors[iMax] - colors[0];
                int delta2 = colors[iMax] - colors[1];

                deltaM = (int)Math.Round((double)((delta1 + delta2) / 2 * deltaMax / 136), 0);

                delta1 = (int)Math.Round((double)(delta1 * deltaMin / 136), 0);
                delta2 = (int)Math.Round((double)(delta2 * deltaMin / 136), 0);

                colors[0] = (byte)(colors[0] + delta1);
                colors[1] = (byte)(colors[1] + delta2);


            }


            colors[iMax] = (byte)(colors[iMax] + deltaM);
            return System.Windows.Media.Color.FromArgb(255, colors[0], colors[1], colors[2]);
        }

        async private void btAddTheme_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTheme.Text))
            {
                uint maxInt = 1;
                var ids = from th in cbThemes.Items.Cast<Theme>() select th.ID;
                if (ids.Count() > 0)
                {
                    maxInt = (uint)ids.Max() + 1;
                }

                List<Language> langs = await loader.GetLanguages();
                await loader.AddTheme(new Theme() { ID = (int)maxInt, Name = tbTheme.Text }, langs[0]);
                List<Theme> themes = await loader.GetThemesByLangWithoutChildren(langs[0]);
                cbThemes.Items.Clear();
                foreach (Theme theme in themes)
                {
                    cbThemes.Items.Add(theme);
                }
            }
        }
        async private void box_OnWordAdded(PictureBox pbox)
        {
            if (!string.IsNullOrEmpty(tbWord.Text) && this.cbThemes.SelectedItem != null)
            {
                int wcount = await loader.GetWordsCount();
                int maxInt = wcount + 1;
                Word wrd = new Word()
                {
                    ID = maxInt,
                    Value = tbWord.Text,
                    PictureUrl = tbWord.Text + ".jpg",
                    AudioUrl = tbWord.Text + ".mp3",
                    BackColor = new LG.Common.Color(pbox.BackgroundColor.ToString(System.Globalization.CultureInfo.CurrentCulture)),
                    BorderColor = new LG.Common.Color(pbox.BorderColor.ToString(System.Globalization.CultureInfo.CurrentCulture)),
                    LeftColor = new LG.Common.Color(pbox.LeftColor.ToString(System.Globalization.CultureInfo.CurrentCulture)),
                    TopColor = new LG.Common.Color(pbox.TopColor.ToString(System.Globalization.CultureInfo.CurrentCulture)),
                    LeftTopColor = new LG.Common.Color(pbox.LeftTopColor.ToString(System.Globalization.CultureInfo.CurrentCulture)),
                };
                Theme t = ((Theme)this.cbThemes.SelectedItem);
                loader.AddWord((uint)t.ID, wrd, new Language() { Name = "English" });
                Directory.CreateDirectory(System.IO.Path.GetFullPath("pictures") + "\\" + t.Name.ToLower() + "\\1Final");
                File.Copy(pbox.PicturePath, System.IO.Path.GetFullPath("pictures") + "\\" + t.Name.ToLower() + "\\1Final\\" + tbWord.Text + ".jpg", true);
            }

        }

        async private void btLoadWords_Click_1(object sender, RoutedEventArgs e)
        {
            spImages.Children.Clear();
            Theme th = (Theme)cbThemes.SelectedItem;
            List<Language> ls = await loader.GetLanguages();
            List<Word> words = await loader.GetWordsByTheme(th.ID, ls[0], 0, 26);
            StackPanel spRow = null;
            for (int i = 0; i < words.Count; i++)
            {
                if (i % 5 == 0)
                {
                    spRow = new StackPanel();
                    spRow.Margin = new Thickness(5);
                    spRow.Orientation = Orientation.Horizontal;
                    spImages.Children.Add(spRow);
                }
                StackPanel spImage = new StackPanel();
                spRow.Children.Add(spImage);

                PictureBox box = new PictureBox(System.IO.Path.GetFullPath("pictures") + "\\" + th.Name.ToLower() + "\\1Final\\" + words[i].PictureUrl.ToLower());
                picBoxes.Add(box);
                box.Margin = new Thickness(5);

                LG.Common.Color clr = words[i].LeftColor;
                box.LeftColor = System.Windows.Media.Color.FromArgb(255, clr.R, clr.G, clr.B);

                clr = words[i].LeftTopColor;
                box.LeftTopColor = System.Windows.Media.Color.FromArgb(255, clr.R, clr.G, clr.B);

                clr = words[i].TopColor;
                box.TopColor = System.Windows.Media.Color.FromArgb(255, clr.R, clr.G, clr.B);

                clr = words[i].BackColor;
                box.BackgroundColor = System.Windows.Media.Color.FromArgb(255, clr.R, clr.G, clr.B);

                clr = words[i].BorderColor;
                box.BorderColor = System.Windows.Media.Color.FromArgb(255, clr.R, clr.G, clr.B);

                spImage.Children.Add(box);
            }
        }
    }


}
