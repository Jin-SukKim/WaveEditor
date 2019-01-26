using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace project
{
    public partial class Form1 : Form
    {
        private Fourier foureir;
        int sNum;
        float[] L;
        float[] R;
        float[] cL;
        bool wav = false;
        bool zoom, wavG = false;
        bool delete = false;
        double[] amplitudes;
        double maxAmplitude;
        double minX, maxX;
        private string path;
        public Form1()
        {
            InitializeComponent();
           
        }
        private void StartGraph()
        {
            zoom = false;
            //chart1.ChartAreas.Clear();
            chart1.Series[0].Points.Clear();
            
            // add ChartArea
            //chart1.ChartAreas[0].BackColor = Color.Black;

            // Setting X & Y for graph
            //chart1.ChartAreas[0].BackColor = Color.Black;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = double.NaN;
            //chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas[0].AxisY.Minimum = double.NaN;
            chart1.ChartAreas[0].AxisY.Maximum = double.NaN;
            //chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //chart1.ChartAreas[0].CursorX.SelectionStart = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = zoom;
            //chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            //chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            // Series add(Sin)
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Color = Color.LightGreen;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].IsVisibleInLegend = false;
        }

        private void fGraph()
        {
            foureir = new Fourier(L, R, sNum);
            //chart1.ChartAreas.Clear();
            chart2.Series[0].Points.Clear();

            // add ChartArea
            //chart1.ChartAreas[0].BackColor = Color.Black;

            // Setting X & Y for graph
            //chart1.ChartAreas[0].BackColor = Color.Black;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Maximum = double.NaN;
            //chart1.ChartAreas[0].AxisX.Interval = 1;
            
            chart2.ChartAreas[0].AxisY.Minimum = double.NaN;
            chart2.ChartAreas[0].AxisY.Maximum = double.NaN;
            //chart1.ChartAreas[0].AxisY.Interval = 1;

            //chart1.ChartAreas[0].CursorX.SelectionStart = true;
            //chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            //chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = zoom;
            //chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            //chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart2.Series[0].Label = "Frequency";
            // Series add(Sin)
            //chart2.Series[0].ChartType = SeriesChartType.Bar;
            ////chart2.Series[0].Color = Color.Green;
            //chart2.Series[0].BorderWidth = 2;
            //chart2.Series[0].IsVisibleInLegend = false;
        }
        
        

    
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void sGraph()
        {
            
            for (int x = 0; x < L.Length; x += 1)
            {
                chart1.Series[0].Points.AddXY(x, L[x]);
            }

            amplitudes = foureir.amplitudes();

            for (int x = 0; x < amplitudes.Length; x += 1)
            {
                chart2.Series[0].Points.AddXY(x, amplitudes[x]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createGraph();
        }
        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //    FolderBrowserDialog dialog = new FolderBrowserDialog();
            //    dialog.ShowDialog();
            //    string select_path = dialog.SelectedPath;
            wavG = false;
            OpenFileDialog oFileDlg = new OpenFileDialog();
            oFileDlg.Filter = "Wave Files(*.wav)|*.wav|All Files(*.*)|*.*";
            oFileDlg.Title = "Selec Files";
            if (oFileDlg.ShowDialog() == DialogResult.OK)
            {
                String strFullPathFile = oFileDlg.FileName;
                path = strFullPathFile;
                if (readWav(strFullPathFile, out L, out R))
                {
                    wav = true;
                }
            }
        }

        private void createGraph()
        {
            if (wav)
            {
                wavG = true;
                StartGraph();
                fGraph();
                sGraph();

            }
        }

        private bool readWav(string filename, out float[] L, out float[] R)
        {
            L = R = null;
            //float [] left = new float[1];

            //float [] right;
            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(fs);

                    // chunk 0
                    int chunkID = reader.ReadInt32();
                    int fileSize = reader.ReadInt32();
                    int riffType = reader.ReadInt32();


                    // chunk 1
                    int fmtID = reader.ReadInt32();
                    int fmtSize = reader.ReadInt32(); // bytes for this chunk
                    int fmtCode = reader.ReadInt16();
                    int channels = reader.ReadInt16();
                    int sampleRate = reader.ReadInt32();
                    int byteRate = reader.ReadInt32();
                    int fmtBlockAlign = reader.ReadInt16();
                    int bitDepth = reader.ReadInt16();

                    if (fmtSize == 18)
                    {
                        // Read any extra values
                        int fmtExtraSize = reader.ReadInt16();
                        reader.ReadBytes(fmtExtraSize);
                    }

                    // chunk 2
                    int dataID = reader.ReadInt32();
                    int bytes = reader.ReadInt32();

                    // DATA!
                    byte[] byteArray = reader.ReadBytes(bytes);

                    int bytesForSamp = bitDepth / 8;
                    int samps = bytes / bytesForSamp;

                    sNum = samps;
                    float[] asFloat = null;
                    switch (bitDepth)
                    {
                        case 64:
                            double[]
                            asDouble = new double[samps];
                            Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
                            asFloat = Array.ConvertAll(asDouble, e => (float)e);
                            break;
                        case 32:
                            asFloat = new float[samps];
                            Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
                            break;
                        case 16:
                            Int16[]
                            asInt16 = new Int16[samps];
                            Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
                            asFloat = Array.ConvertAll(asInt16, e => e / (float)Int16.MaxValue);
                            break;
                        default:
                            return false;
                    }

                    switch (channels)
                    {
                        case 1:
                            L = asFloat;
                            R = null;
                            return true;
                        case 2:
                            L = new float[samps];
                            R = new float[samps];
                            for (int i = 0, s = 0; i < samps; i++)
                            {
                                L[i] = asFloat[s++];
                                R[i] = asFloat[s++];
                            }
                            return true;
                        default:
                            return false;
                    }
                }
            }
            catch
            {
                return false;
                //left = new float[ 1 ]{ 0f };
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = zoom;
            minX = -1;
            maxX = 0;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = false;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = zoom;
            minX = -1;
            maxX = 0;
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            double mDownX = -1;
            minX = -1;
            maxX = 0;
            Axis ax = chart1.ChartAreas[0].AxisX;
            mDownX = ax.PixelPositionToValue(e.X);
            minX = mDownX;
            maxX = mDownX;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if(minX == -1)
            {
                return;
            }   
            // If the mouse isn't on the plotting area, a datapoint, or gridline then exit
            HitTestResult htr = chart1.HitTest(e.X, e.Y);
            if (htr.ChartElementType != ChartElementType.PlottingArea && htr.ChartElementType != ChartElementType.DataPoint && htr.ChartElementType != ChartElementType.Gridlines)
                return;

            Axis ax = chart1.ChartAreas[0].AxisX;
            double mMoveX = ax.PixelPositionToValue(e.X);
            if (mMoveX < minX)
            {
                minX = mMoveX;
            }
            if (mMoveX > maxX)
            {
                maxX = mMoveX;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);

            float[] newL = new float[sNum];
            float[] newR = null;
            //int j = 0;
            //if (mDownX > mMoveX)
            //{
            if(minX == -1)
            {
                return;
            }
            if(minX < 1)
            {
                newL[0] = 0;
            }
            for (int i = 1; i < L.Length; i += 1)
            {
                if ((i >= minX) && (i < maxX))
                {
                    newL[i] = 0;
                    //++j;
                } else
                {
                    newL[i] = L[i];
                }
            }
            if (newL != null)
            {
                chart1.Series[0].Points.Clear();
                chart2.Series[0].Points.Clear();
                for (int i = 0; i < newL.Length; i++)
                {
                    L[i] = newL[i];
                }
                chart1.ChartAreas[0].AxisX.Maximum = L.Length;
                chart1.Update();
                chart2.ChartAreas[0].AxisX.Maximum = L.Length;
                chart2.Update();
                createGraph();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            cL = L;
            L = foureir.getInverse();
            chart1.Update();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(wavG)
            {
                try
                {
                    SoundPlayer sdPlayer = new SoundPlayer();
                    sdPlayer.SoundLocation = path;
                    sdPlayer.LoadAsync();
                    sdPlayer.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error loading sound");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        
    }

}


