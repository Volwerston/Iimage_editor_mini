using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using ExtensionMethods;
using System.IO;

namespace ImageEditor
{
    public partial class Form1 : Form
    {
        readonly Stack<Image> UndoStack = new Stack<Image>(5);
        readonly Stack<Image> RedoStack = new Stack<Image>(5);
        private bool draw;
        private int x, y, lx, ly;
        private Color paintcolor = Color.Black;
        private Bitmap bitmap;
        private Graphics graphics;
        private int brushSize = 3;
        public string filename { get; set; }


        public Form1()
        {
            InitializeComponent();
            changeMenuOptions(false);
            graphics = pictureBoxImage.CreateGraphics();
        }

        public void UndoStackAdd(Image img)
        {
            UndoStack.Push(img);
            undoToolStripMenuItem.Enabled = true;
        }

        public void RedoStackAdd(Image img)
        {
            RedoStack.Push(img);
            rToolStripMenuItem.Enabled = true;
        }

        private void changeMenuOptions(bool value)
        {
            this.saveAsToolStripMenuItem.Enabled = value;
            this.sepiaToolStripMenuItem.Enabled = value;
            this.blurToolStripMenuItem.Enabled = value;
            this.jitterToolStripMenuItem.Enabled = value;
            this.pixallateToolStripMenuItem.Enabled = value;
            this.saveToolStripMenuItem.Enabled = value;
            this.rToolStripMenuItem.Enabled = value;
            this.undoToolStripMenuItem.Enabled = value;
            this.resizeToolStripMenuItem.Enabled = value;
            this.rotateToolStripMenuItem.Enabled = value;
            this.invertColorToolStripMenuItem.Enabled = value;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Documents and Settings\Administrator\My Documents\My Pictures";
            openFileDialog1.Title = "Select an Image";
            openFileDialog1.Filter = "All Files|*.*|Windows Bitmaps|*.bmp|JPEG Files|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                Image img = System.Drawing.Image.FromFile(filename);
                bitmap = new Bitmap(img);
                adjustWindow(bitmap.Width, bitmap.Height);
                pictureBoxImage.Image = bitmap;
                img.Dispose();
                changeMenuOptions(true);
                undoToolStripMenuItem.Enabled = false;
                rToolStripMenuItem.Enabled = false;
                RedoStack.Clear();
                UndoStack.Clear();
                pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
                graphics = Graphics.FromImage(bitmap);
            }
        }

        public void adjustWindow(int width, int height)
        {
            this.Height = height + 2 * menuStrip1.Height;
            pictureBoxImage.Height = height;

            this.Width = width;

            pictureBoxImage.Width = width;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == "")
            {
                SaveFileDialog Save = new SaveFileDialog();
                try
                {
                    //Open a file dialog for saving map documents

                    Save.Title = "Save";
                    Save.Filter = "Please select a file type||Bitmap File (*.bmp)|*.bmp|JPEG file (*.jpg)|*.jpg|PNG file(*.png)|*.png";
                    if (Save.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        filename = Save.FileName;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            // Delete existing file first
            System.IO.File.Delete(filename);
            // then save it
            pictureBoxImage.Image.Save(filename);
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UndoStack.Count != 0)
            {
                RedoStackAdd(pictureBoxImage.Image);
                pictureBoxImage.Image = UndoStack.Pop();
                if (UndoStack.Count == 0)
                {
                    undoToolStripMenuItem.Enabled = false;
                }
                else
                {
                    undoToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RedoStack.Count != 0)
            {
                UndoStackAdd(pictureBoxImage.Image);
                pictureBoxImage.Image = RedoStack.Pop();
                if (RedoStack.Count == 0)
                {
                    rToolStripMenuItem.Enabled = false;
                }
                else
                {
                    rToolStripMenuItem.Enabled = true;
                }
            }
        }



        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog Save = new SaveFileDialog();
            try
            {
                //Open a file dialog for saving map documents

                Save.Title = "Save";
                Save.Filter = "Please select a file type||Bitmap File (*.bmp)|*.bmp|JPEG file (*.jpg)|*.jpg|PNG file(*.png)|*.png";
                if (Save.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            if (filename == "")
            {
                return;
            }
            else if (Save.FileName.Contains(".jpg"))
            {
                pictureBoxImage.Image.Save(Save.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else if (Save.FileName.Contains(".png"))
            {
                pictureBoxImage.Image.Save(Save.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                pictureBoxImage.Image.Save(Save.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            filename = Save.FileName;
        }


        private void filter(Bitmap afterFilteringBitmap)
        {
            UndoStackAdd(pictureBoxImage.Image);
            bitmap = afterFilteringBitmap;
            graphics = Graphics.FromImage(bitmap);  
            RedoStack.Clear();
            pictureBoxImage.Image = bitmap;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(bitmap.ToSepia());
        }

        private void blackAndWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(bitmap.ToBlackAndWhite());
        }

        private void pixallateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(bitmap.ToPixalation());
        }

        private void jitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(bitmap.ToJitter());
        }

        private void invertColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(bitmap.ToInvert());
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gaussian gaussianForm = new Gaussian();
            if (gaussianForm.ShowDialog() == DialogResult.OK)
            {
                filter(bitmap.gaussianBlur(gaussianForm.blur));
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resize resizeForm = new resize();
            if (resizeForm.ShowDialog() == DialogResult.OK)
            {
                adjustWindow(resizeForm.Width, resizeForm.Height);
                filter(bitmap.resize(resizeForm.Width,resizeForm.Height));
            }
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rotate rotateForm = new Rotate();
            if (rotateForm.ShowDialog() == DialogResult.OK)
            {
                UndoStackAdd(pictureBoxImage.Image);
                System.Drawing.Bitmap image = (Bitmap)pictureBoxImage.Image;
                pictureBoxImage.Image = image.rotate(rotateForm.Degrees);
                adjustWindow(image.Width, image.Height);
                RedoStack.Clear();
            }
        }


        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                paintcolor = colorDialog1.Color;

            buttonColor.BackColor = colorDialog1.Color;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            brushSize = (int)numericUpDown1.Value;
        }

        private void pictureBoxImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.FillEllipse(new SolidBrush(paintcolor), e.X - x + x, e.Y - y + y, brushSize, brushSize);
                pictureBoxImage.Image = bitmap;
            }
        }

        private void pictureBoxImage_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            x = e.X;
            y = e.Y;
        }

        private void pictureBoxImage_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            lx = e.X;
            ly = e.Y;
        }
    }
}
