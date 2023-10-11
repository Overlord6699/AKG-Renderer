using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CGA_1
{
    public partial class Form1 : Form
    {
        private float currentScale = 1.0f;

        private ObjModel _model;
        public Form1()
        {
            InitializeComponent();

            Color backColor = Color.FromArgb(32, 33, 36);
            this.BackColor = backColor;
         
        }    


        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Color penColor = Color.FromArgb(222, 218, 217);
            Pen pen = new Pen(penColor);

            g.ScaleTransform(currentScale, currentScale);

            foreach (var edge in _model.Edges)
            {
                int x1 = (int)(edge.Item1.X * currentScale);
                int y1 = (int)(edge.Item1.Y * currentScale);
                int x2 = (int)(edge.Item2.X * currentScale);
                int y2 = (int)(edge.Item2.Y * currentScale);

                // Реализация алгоритма DDA для рисования линии
                int dx = x2 - x1;
                int dy = y2 - y1;
                int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
                float xIncrement = (float)dx / steps;
                float yIncrement = (float)dy / steps;
                float x = x1, y = y1;

                for (int i = 0; i <= steps; i++)
                {                    
                    g.DrawRectangle(pen, x, y, 1, 1);
                    x += xIncrement;
                    y += yIncrement;
                }
            }
            pen.Dispose();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int delta = trackBar1.Value;//e.Delta;

            float scaleFactor = trackBar1.Value / 10;//0.2f;

            currentScale += scaleFactor;
            /*if (delta > 0)
            {
                currentScale += scaleFactor;
            }
            else if (delta < 0 && currentScale > scaleFactor)
            {
                currentScale -= scaleFactor;
            }*/

            pictureBox.Invalidate();
        }

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedFilePath = openFileDialog.FileName;

                if (File.Exists(selectedFilePath))
                {
                    // Файл существует, можно считать его содержимое
                    using (StreamReader reader = new StreamReader(selectedFilePath))
                    {
                        string fileContent = reader.ReadToEnd();

                        var parser = new ObjectModelParser();
                        _model = parser.ParseObjFile(in selectedFilePath);

                        // Здесь вы можете обработать содержимое файла
                        MessageBox.Show("Файл успешно считан!");
                    }
                }
                else
                {
                    MessageBox.Show("Выбранный файл не существует.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при считывании файла: " + ex.Message);
            }

            
        }
    }
}
