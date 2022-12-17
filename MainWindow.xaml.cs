using AEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StageEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public class StageData
    {
        public string MapName;
        public string FileName;
        public Int16 Unk1;
        public Int16 Unk2;
        public Int32 Force;
        public Int32 Hurricane;
        public Int32 Electricity;
        public Int32 Wind;
        public Int32 Protection;
        public Int32 Ignorance;
        public Int32 Thor;
        public Int32 BlackForce;
        public Int32 Mirror;
        public Int32 JokerForce;
        public byte[] Unk5;

        public Int32 H1Min;
        public Int32 H2Min;
        public Int32 H3Min;
        public Int32 H4Min;
        public Int32 H5Min;
        public Int32 H6Min;
        public Int32 H7Min;
        public Int32 H8Min;
        public Int32 H1Max;
        public Int32 H2Max;
        public Int32 H3Max;
        public Int32 H4Max;
        public Int32 H5Max;
        public Int32 H6Max;
        public Int32 H7Max;
        public Int32 H8Max;

        public Int32 F1Min;
        public Int32 F2Min;
        public Int32 F3Min;
        public Int32 F4Min;
        public Int32 F5Min;
        public Int32 F6Min;
        public Int32 F7Min;
        public Int32 F8Min;
        public Int32 F1Max;
        public Int32 F2Max;
        public Int32 F3Max;
        public Int32 F4Max;
        public Int32 F5Max;
        public Int32 F6Max;
        public Int32 F7Max;
        public Int32 F8Max;

        public Int32 E1Min;
        public Int32 E2Min;
        public Int32 E3Min;
        public Int32 E4Min;
        public Int32 E5Min;
        public Int32 E6Min;
        public Int32 E7Min;
        public Int32 E8Min;
        public Int32 E1Max;
        public Int32 E2Max;
        public Int32 E3Max;
        public Int32 E4Max;
        public Int32 E5Max;
        public Int32 E6Max;
        public Int32 E7Max;
        public Int32 E8Max;

        public Int32 X1Min;
        public Int32 X2Min;
        public Int32 X3Min;
        public Int32 X4Min;
        public Int32 X5Min;
        public Int32 X6Min;
        public Int32 X7Min;
        public Int32 X8Min;
        public Int32 X1Max;
        public Int32 X2Max;
        public Int32 X3Max;
        public Int32 X4Max;
        public Int32 X5Max;
        public Int32 X6Max;
        public Int32 X7Max;
        public Int32 X8Max;

        public Int32 Y1Min;
        public Int32 Y2Min;
        public Int32 Y3Min;
        public Int32 Y4Min;
        public Int32 Y5Min;
        public Int32 Y6Min;
        public Int32 Y7Min;
        public Int32 Y8Min;
        public Int32 Y1Max;
        public Int32 Y2Max;
        public Int32 Y3Max;
        public Int32 Y4Max;
        public Int32 Y5Max;
        public Int32 Y6Max;
        public Int32 Y7Max;
        public Int32 Y8Max;

    }

    public partial class MainWindow : Window
    {
        List<StageData> Temp = new List<StageData>();

        MemoryStream FileStream = null;

        byte[] CRC = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        int CurrentIndex = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("stage.dat"))
            {
                byte[] Stage = File.ReadAllBytes("stage.dat");

                StreamDataReader SR = new StreamDataReader(Stage);
                CRC = SR.ReadBytes(4);
                byte[] StageDecrypted = GBCrypto.Decompress(SR.ReadBytes(SR.Length - 4), 39936);

                FileStream = new MemoryStream(StageDecrypted);

                //File.WriteAllBytes("StageDecrypted.dat", StageDecrypted);
                StreamDataReader StageReader = new StreamDataReader(StageDecrypted);

                while (StageReader.CurrentOffset != StageReader.Length)
                {

                    StageData T = new StageData();
                    T.MapName = StageReader.ReadPStringFixed(128);
                    T.FileName = StageReader.ReadPStringFixed(128);
                    T.Unk1 = StageReader.ReadInt16();
                    T.Unk2 = StageReader.ReadInt16();

                    T.Force = StageReader.ReadInt32();
                    T.Hurricane = StageReader.ReadInt32();
                    T.Electricity = StageReader.ReadInt32();
                    T.Wind = StageReader.ReadInt32();
                    T.Protection = StageReader.ReadInt32();
                    T.Ignorance = StageReader.ReadInt32();
                    T.Thor = StageReader.ReadInt32();
                    T.BlackForce = StageReader.ReadInt32();
                    T.Mirror = StageReader.ReadInt32();
                    T.JokerForce = StageReader.ReadInt32();
                    T.Unk5 = StageReader.ReadBytes(4);
                    //Hurricane
                    T.H1Min = StageReader.ReadInt32();
                    T.H2Min = StageReader.ReadInt32();
                    T.H3Min = StageReader.ReadInt32();
                    T.H4Min = StageReader.ReadInt32();
                    T.H5Min = StageReader.ReadInt32();
                    T.H6Min = StageReader.ReadInt32();
                    T.H7Min = StageReader.ReadInt32();
                    T.H8Min = StageReader.ReadInt32();

                    T.H1Max = StageReader.ReadInt32();
                    T.H2Max = StageReader.ReadInt32();
                    T.H3Max = StageReader.ReadInt32();
                    T.H4Max = StageReader.ReadInt32();
                    T.H5Max = StageReader.ReadInt32();
                    T.H6Max = StageReader.ReadInt32();
                    T.H7Max = StageReader.ReadInt32();
                    T.H8Max = StageReader.ReadInt32();

                    //Force
                    T.F1Min = StageReader.ReadInt32();
                    T.F2Min = StageReader.ReadInt32();
                    T.F3Min = StageReader.ReadInt32();
                    T.F4Min = StageReader.ReadInt32();
                    T.F5Min = StageReader.ReadInt32();
                    T.F6Min = StageReader.ReadInt32();
                    T.F7Min = StageReader.ReadInt32();
                    T.F8Min = StageReader.ReadInt32();

                    T.F1Max = StageReader.ReadInt32();
                    T.F2Max = StageReader.ReadInt32();
                    T.F3Max = StageReader.ReadInt32();
                    T.F4Max = StageReader.ReadInt32();
                    T.F5Max = StageReader.ReadInt32();
                    T.F6Max = StageReader.ReadInt32();
                    T.F7Max = StageReader.ReadInt32();
                    T.F8Max = StageReader.ReadInt32();

                    //Electricity
                    T.E1Min = StageReader.ReadInt32();
                    T.E2Min = StageReader.ReadInt32();
                    T.E3Min = StageReader.ReadInt32();
                    T.E4Min = StageReader.ReadInt32();
                    T.E5Min = StageReader.ReadInt32();
                    T.E6Min = StageReader.ReadInt32();
                    T.E7Min = StageReader.ReadInt32();
                    T.E8Min = StageReader.ReadInt32();

                    T.E1Max = StageReader.ReadInt32();
                    T.E2Max = StageReader.ReadInt32();
                    T.E3Max = StageReader.ReadInt32();
                    T.E4Max = StageReader.ReadInt32();
                    T.E5Max = StageReader.ReadInt32();
                    T.E6Max = StageReader.ReadInt32();
                    T.E7Max = StageReader.ReadInt32();
                    T.E8Max = StageReader.ReadInt32();

                    //Unk1

                    T.X1Min = StageReader.ReadInt32();
                    T.X2Min = StageReader.ReadInt32();
                    T.X3Min = StageReader.ReadInt32();
                    T.X4Min = StageReader.ReadInt32();
                    T.X5Min = StageReader.ReadInt32();
                    T.X6Min = StageReader.ReadInt32();
                    T.X7Min = StageReader.ReadInt32();
                    T.X8Min = StageReader.ReadInt32();

                    T.X1Max = StageReader.ReadInt32();
                    T.X2Max = StageReader.ReadInt32();
                    T.X3Max = StageReader.ReadInt32();
                    T.X4Max = StageReader.ReadInt32();
                    T.X5Max = StageReader.ReadInt32();
                    T.X6Max = StageReader.ReadInt32();
                    T.X7Max = StageReader.ReadInt32();
                    T.X8Max = StageReader.ReadInt32();

                    //Unk2

                    T.Y1Min = StageReader.ReadInt32();
                    T.Y2Min = StageReader.ReadInt32();
                    T.Y3Min = StageReader.ReadInt32();
                    T.Y4Min = StageReader.ReadInt32();
                    T.Y5Min = StageReader.ReadInt32();
                    T.Y6Min = StageReader.ReadInt32();
                    T.Y7Min = StageReader.ReadInt32();
                    T.Y8Min = StageReader.ReadInt32();

                    T.Y1Max = StageReader.ReadInt32();
                    T.Y2Max = StageReader.ReadInt32();
                    T.Y3Max = StageReader.ReadInt32();
                    T.Y4Max = StageReader.ReadInt32();
                    T.Y5Max = StageReader.ReadInt32();
                    T.Y6Max = StageReader.ReadInt32();
                    T.Y7Max = StageReader.ReadInt32();
                    T.Y8Max = StageReader.ReadInt32();



                    Temp.Add(T);

                    if (String.IsNullOrEmpty(T.MapName))
                    {
                        break;
                    }

                }

                ShowData(Temp[0]);
            }
            else
            {
                LBError.Visibility = Visibility.Visible;
            }
        }

        void ShowData(StageData Data)
        {
            LBLINDEX.Content = "Index: " + CurrentIndex;
            TXTTITLE.Text = Data.MapName;
            TXTFILENAME.Text = Data.FileName;
            TXTHEX.Text = BitConverter.ToString(Data.Unk5).Replace("-", "").ToUpper();
            TForce.Text = Data.Force.ToString();
            THurricane.Text = Data.Hurricane.ToString();
            TElectricity.Text = Data.Electricity.ToString();
            TWind.Text = Data.Wind.ToString();
            TProtection.Text = Data.Protection.ToString();
            TIgnorance.Text = Data.Ignorance.ToString();
            TThor.Text = Data.Thor.ToString();
            TBlackForce.Text = Data.BlackForce.ToString();
            TMirror.Text = Data.Mirror.ToString();
            TJokerForce.Text = Data.JokerForce.ToString();

            HMIN1.Text = Data.H1Min.ToString();
            HMIN2.Text = Data.H2Min.ToString();
            HMIN3.Text = Data.H3Min.ToString();
            HMIN4.Text = Data.H4Min.ToString();
            HMIN5.Text = Data.H5Min.ToString();
            HMIN6.Text = Data.H6Min.ToString();
            HMIN7.Text = Data.H7Min.ToString();
            HMIN8.Text = Data.H8Min.ToString();
            HMAX1.Text = Data.H1Max.ToString();
            HMAX2.Text = Data.H2Max.ToString();
            HMAX3.Text = Data.H3Max.ToString();
            HMAX4.Text = Data.H4Max.ToString();
            HMAX5.Text = Data.H5Max.ToString();
            HMAX6.Text = Data.H6Max.ToString();
            HMAX7.Text = Data.H7Max.ToString();
            HMAX8.Text = Data.H8Max.ToString();

            FMIN1.Text = Data.F1Min.ToString();
            FMIN2.Text = Data.F2Min.ToString();
            FMIN3.Text = Data.F3Min.ToString();
            FMIN4.Text = Data.F4Min.ToString();
            FMIN5.Text = Data.F5Min.ToString();
            FMIN6.Text = Data.F6Min.ToString();
            FMIN7.Text = Data.F7Min.ToString();
            FMIN8.Text = Data.F8Min.ToString();
            FMAX1.Text = Data.F1Max.ToString();
            FMAX2.Text = Data.F2Max.ToString();
            FMAX3.Text = Data.F3Max.ToString();
            FMAX4.Text = Data.F4Max.ToString();
            FMAX5.Text = Data.F5Max.ToString();
            FMAX6.Text = Data.F6Max.ToString();
            FMAX7.Text = Data.F7Max.ToString();
            FMAX8.Text = Data.F8Max.ToString();

            EMIN1.Text = Data.E1Min.ToString();
            EMIN2.Text = Data.E2Min.ToString();
            EMIN3.Text = Data.E3Min.ToString();
            EMIN4.Text = Data.E4Min.ToString();
            EMIN5.Text = Data.E5Min.ToString();
            EMIN6.Text = Data.E6Min.ToString();
            EMIN7.Text = Data.E7Min.ToString();
            EMIN8.Text = Data.E8Min.ToString();
            EMAX1.Text = Data.E1Max.ToString();
            EMAX2.Text = Data.E2Max.ToString();
            EMAX3.Text = Data.E3Max.ToString();
            EMAX4.Text = Data.E4Max.ToString();
            EMAX5.Text = Data.E5Max.ToString();
            EMAX6.Text = Data.E6Max.ToString();
            EMAX7.Text = Data.E7Max.ToString();
            EMAX8.Text = Data.E8Max.ToString();

            XMIN1.Text = Data.X1Min.ToString();
            XMIN2.Text = Data.X2Min.ToString();
            XMIN3.Text = Data.X3Min.ToString();
            XMIN4.Text = Data.X4Min.ToString();
            XMIN5.Text = Data.X5Min.ToString();
            XMIN6.Text = Data.X6Min.ToString();
            XMIN7.Text = Data.X7Min.ToString();
            XMIN8.Text = Data.X8Min.ToString();
            XMAX1.Text = Data.X1Max.ToString();
            XMAX2.Text = Data.X2Max.ToString();
            XMAX3.Text = Data.X3Max.ToString();
            XMAX4.Text = Data.X4Max.ToString();
            XMAX5.Text = Data.X5Max.ToString();
            XMAX6.Text = Data.X6Max.ToString();
            XMAX7.Text = Data.X7Max.ToString();
            XMAX8.Text = Data.X8Max.ToString();

            YMIN1.Text = Data.Y1Min.ToString();
            YMIN2.Text = Data.Y2Min.ToString();
            YMIN3.Text = Data.Y3Min.ToString();
            YMIN4.Text = Data.Y4Min.ToString();
            YMIN5.Text = Data.Y5Min.ToString();
            YMIN6.Text = Data.Y6Min.ToString();
            YMIN7.Text = Data.Y7Min.ToString();
            YMIN8.Text = Data.Y8Min.ToString();
            YMAX1.Text = Data.Y1Max.ToString();
            YMAX2.Text = Data.Y2Max.ToString();
            YMAX3.Text = Data.Y3Max.ToString();
            YMAX4.Text = Data.Y4Max.ToString();
            YMAX5.Text = Data.Y5Max.ToString();
            YMAX6.Text = Data.Y6Max.ToString();
            YMAX7.Text = Data.Y7Max.ToString();
            YMAX8.Text = Data.Y8Max.ToString();

            if (Data.Unk2 == 0)
            {
                CUNK2_1.IsChecked = true;
                CUNK2_2.IsChecked = false;
            }
            else
            {
                CUNK2_2.IsChecked = true;
                CUNK2_1.IsChecked = false;
            }

            if (Data.Unk1 == 0)
            {
                CUNK1_1.IsChecked = true;
               
            }
            else if(Data.Unk1 == 1)
            {
                CUNK1_2.IsChecked = true;
            }
            else if (Data.Unk1 == 2)
            {
                CUNK1_3.IsChecked = true;
            }

            TXTDEBUGC.Content = $"UNK1:{Data.Unk1.ToString("x2")} UNK2:{Data.Unk2.ToString("x2")}";

        }

        void SaveRow()
        {
            if ((bool)CUNK2_1.IsChecked)
            {
                Temp[CurrentIndex].Unk2 = 0;
            }
            else if ((bool)CUNK2_2.IsChecked)
            {
                Temp[CurrentIndex].Unk2 = 1;
            }

            if ((bool)CUNK1_1.IsChecked)
            {
                Temp[CurrentIndex].Unk1 = 0;
            }
            else if ((bool)CUNK1_2.IsChecked)
            {
                Temp[CurrentIndex].Unk1 = 1;
            }
            else if ((bool)CUNK1_3.IsChecked)
            {
                Temp[CurrentIndex].Unk1 = 2;
            }

            Temp[CurrentIndex].MapName = TXTTITLE.Text;
            Temp[CurrentIndex].FileName = TXTFILENAME.Text;
            Temp[CurrentIndex].Force = Int32.Parse(TForce.Text);
            Temp[CurrentIndex].Hurricane = Int32.Parse(THurricane.Text);
            Temp[CurrentIndex].Electricity = Int32.Parse(TElectricity.Text);
            Temp[CurrentIndex].Wind = Int32.Parse(TWind.Text);
            Temp[CurrentIndex].Protection = Int32.Parse(TProtection.Text);
            Temp[CurrentIndex].Ignorance = Int32.Parse(TIgnorance.Text);
            Temp[CurrentIndex].Thor = Int32.Parse(TThor.Text);
            Temp[CurrentIndex].BlackForce = Int32.Parse(TBlackForce.Text);
            Temp[CurrentIndex].Mirror = Int32.Parse(TMirror.Text);
            Temp[CurrentIndex].JokerForce = Int32.Parse(TJokerForce.Text);

            Temp[CurrentIndex].H1Min = Int32.Parse(HMIN1.Text);
            Temp[CurrentIndex].H2Min = Int32.Parse(HMIN2.Text);
            Temp[CurrentIndex].H3Min = Int32.Parse(HMIN3.Text);
            Temp[CurrentIndex].H4Min = Int32.Parse(HMIN4.Text);
            Temp[CurrentIndex].H5Min = Int32.Parse(HMIN5.Text);
            Temp[CurrentIndex].H6Min = Int32.Parse(HMIN6.Text);
            Temp[CurrentIndex].H7Min = Int32.Parse(HMIN7.Text);
            Temp[CurrentIndex].H8Min = Int32.Parse(HMIN8.Text);

            Temp[CurrentIndex].H1Max = Int32.Parse(HMAX1.Text);
            Temp[CurrentIndex].H2Max = Int32.Parse(HMAX2.Text);
            Temp[CurrentIndex].H3Max = Int32.Parse(HMAX3.Text);
            Temp[CurrentIndex].H4Max = Int32.Parse(HMAX4.Text);
            Temp[CurrentIndex].H5Max = Int32.Parse(HMAX5.Text);
            Temp[CurrentIndex].H6Max = Int32.Parse(HMAX6.Text);
            Temp[CurrentIndex].H7Max = Int32.Parse(HMAX7.Text);
            Temp[CurrentIndex].H8Max = Int32.Parse(HMAX8.Text);

            Temp[CurrentIndex].F1Min = Int32.Parse(FMIN1.Text);
            Temp[CurrentIndex].F2Min = Int32.Parse(FMIN2.Text);
            Temp[CurrentIndex].F3Min = Int32.Parse(FMIN3.Text);
            Temp[CurrentIndex].F4Min = Int32.Parse(FMIN4.Text);
            Temp[CurrentIndex].F5Min = Int32.Parse(FMIN5.Text);
            Temp[CurrentIndex].F6Min = Int32.Parse(FMIN6.Text);
            Temp[CurrentIndex].F7Min = Int32.Parse(FMIN7.Text);
            Temp[CurrentIndex].F8Min = Int32.Parse(FMIN8.Text);

            Temp[CurrentIndex].F1Max = Int32.Parse(FMAX1.Text);
            Temp[CurrentIndex].F2Max = Int32.Parse(FMAX2.Text);
            Temp[CurrentIndex].F3Max = Int32.Parse(FMAX3.Text);
            Temp[CurrentIndex].F4Max = Int32.Parse(FMAX4.Text);
            Temp[CurrentIndex].F5Max = Int32.Parse(FMAX5.Text);
            Temp[CurrentIndex].F6Max = Int32.Parse(FMAX6.Text);
            Temp[CurrentIndex].F7Max = Int32.Parse(FMAX7.Text);
            Temp[CurrentIndex].F8Max = Int32.Parse(FMAX8.Text);

            Temp[CurrentIndex].E1Min = Int32.Parse(EMIN1.Text);
            Temp[CurrentIndex].E2Min = Int32.Parse(EMIN2.Text);
            Temp[CurrentIndex].E3Min = Int32.Parse(EMIN3.Text);
            Temp[CurrentIndex].E4Min = Int32.Parse(EMIN4.Text);
            Temp[CurrentIndex].E5Min = Int32.Parse(EMIN5.Text);
            Temp[CurrentIndex].E6Min = Int32.Parse(EMIN6.Text);
            Temp[CurrentIndex].E7Min = Int32.Parse(EMIN7.Text);
            Temp[CurrentIndex].E8Min = Int32.Parse(EMIN8.Text);

            Temp[CurrentIndex].E1Max = Int32.Parse(EMAX1.Text);
            Temp[CurrentIndex].E2Max = Int32.Parse(EMAX2.Text);
            Temp[CurrentIndex].E3Max = Int32.Parse(EMAX3.Text);
            Temp[CurrentIndex].E4Max = Int32.Parse(EMAX4.Text);
            Temp[CurrentIndex].E5Max = Int32.Parse(EMAX5.Text);
            Temp[CurrentIndex].E6Max = Int32.Parse(EMAX6.Text);
            Temp[CurrentIndex].E7Max = Int32.Parse(EMAX7.Text);
            Temp[CurrentIndex].E8Max = Int32.Parse(EMAX8.Text);

            Temp[CurrentIndex].X1Min = Int32.Parse(XMIN1.Text);
            Temp[CurrentIndex].X2Min = Int32.Parse(XMIN2.Text);
            Temp[CurrentIndex].X3Min = Int32.Parse(XMIN3.Text);
            Temp[CurrentIndex].X4Min = Int32.Parse(XMIN4.Text);
            Temp[CurrentIndex].X5Min = Int32.Parse(XMIN5.Text);
            Temp[CurrentIndex].X6Min = Int32.Parse(XMIN6.Text);
            Temp[CurrentIndex].X7Min = Int32.Parse(XMIN7.Text);
            Temp[CurrentIndex].X8Min = Int32.Parse(XMIN8.Text);

            Temp[CurrentIndex].X1Max = Int32.Parse(XMAX1.Text);
            Temp[CurrentIndex].X2Max = Int32.Parse(XMAX2.Text);
            Temp[CurrentIndex].X3Max = Int32.Parse(XMAX3.Text);
            Temp[CurrentIndex].X4Max = Int32.Parse(XMAX4.Text);
            Temp[CurrentIndex].X5Max = Int32.Parse(XMAX5.Text);
            Temp[CurrentIndex].X6Max = Int32.Parse(XMAX6.Text);
            Temp[CurrentIndex].X7Max = Int32.Parse(XMAX7.Text);
            Temp[CurrentIndex].X8Max = Int32.Parse(XMAX8.Text);

            Temp[CurrentIndex].Y1Min = Int32.Parse(YMIN1.Text);
            Temp[CurrentIndex].Y2Min = Int32.Parse(YMIN2.Text);
            Temp[CurrentIndex].Y3Min = Int32.Parse(YMIN3.Text);
            Temp[CurrentIndex].Y4Min = Int32.Parse(YMIN4.Text);
            Temp[CurrentIndex].Y5Min = Int32.Parse(YMIN5.Text);
            Temp[CurrentIndex].Y6Min = Int32.Parse(YMIN6.Text);
            Temp[CurrentIndex].Y7Min = Int32.Parse(YMIN7.Text);
            Temp[CurrentIndex].Y8Min = Int32.Parse(YMIN8.Text);

            Temp[CurrentIndex].Y1Max = Int32.Parse(YMAX1.Text);
            Temp[CurrentIndex].Y2Max = Int32.Parse(YMAX2.Text);
            Temp[CurrentIndex].Y3Max = Int32.Parse(YMAX3.Text);
            Temp[CurrentIndex].Y4Max = Int32.Parse(YMAX4.Text);
            Temp[CurrentIndex].Y5Max = Int32.Parse(YMAX5.Text);
            Temp[CurrentIndex].Y6Max = Int32.Parse(YMAX6.Text);
            Temp[CurrentIndex].Y7Max = Int32.Parse(YMAX7.Text);
            Temp[CurrentIndex].Y8Max = Int32.Parse(YMAX8.Text);





        }

        private void BTNBACK_Click(object sender, RoutedEventArgs e)
        {
            SaveRow();
            if (CurrentIndex > -1)
            {
                if (CurrentIndex - 1 > -1)
                {
                    CurrentIndex--;
                    ShowData(Temp[CurrentIndex]);
                }
            }
        }

        private void BTNNEXT_Click(object sender, RoutedEventArgs e)
        {
            SaveRow();
            if (CurrentIndex + 1 < Temp.Count )
            {
                CurrentIndex++;
                ShowData(Temp[CurrentIndex]);
            }
        }

        private void BTNADDNEW_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex++;
            Temp.Insert(CurrentIndex, new StageData() { MapName = ".", FileName = ".", Unk5 = new byte[4] });
            ShowData(Temp[CurrentIndex]);
        }

        private void BTNSAVE_Click(object sender, RoutedEventArgs e)
        {
            BinaryWriter SW = new BinaryWriter(FileStream);
            SW.BaseStream.Position = 0;


            for (int i = 0; i < Temp.Count; i++)
            {
                


              
                if (Temp[i].MapName.Length == 0)
                {
                    SW.Write(new byte[128]);
                }
                else
                {
                    SW.Write(Encoding.ASCII.GetBytes(Temp[i].MapName));
                    SW.Write((byte)0);
                    SW.BaseStream.Position += 128 - (Temp[i].MapName.Length + 1);
                }

                if (Temp[i].FileName.Length == 0)
                {
                    SW.Write(new byte[128]);
                }
                else
                {
                    SW.Write(Encoding.ASCII.GetBytes(Temp[i].FileName));
                    SW.Write((byte)0);
                    SW.BaseStream.Position += 128 - (Temp[i].FileName.Length + 1);
                }



                SW.Write(Temp[i].Unk1);
                SW.Write(Temp[i].Unk2);

                SW.Write(Temp[i].Force);
                SW.Write(Temp[i].Hurricane);
                SW.Write(Temp[i].Electricity);
                SW.Write(Temp[i].Wind);
                SW.Write(Temp[i].Protection);
                SW.Write(Temp[i].Ignorance);
                SW.Write(Temp[i].Thor);
                SW.Write(Temp[i].BlackForce);
                SW.Write(Temp[i].Mirror);
                SW.Write(Temp[i].JokerForce);
                SW.Write(Temp[i].Unk5);

                SW.Write(Temp[i].H1Min);
                SW.Write(Temp[i].H2Min);
                SW.Write(Temp[i].H3Min);
                SW.Write(Temp[i].H4Min);
                SW.Write(Temp[i].H5Min);
                SW.Write(Temp[i].H6Min);
                SW.Write(Temp[i].H7Min);
                SW.Write(Temp[i].H8Min);
                SW.Write(Temp[i].H1Max);
                SW.Write(Temp[i].H2Max);
                SW.Write(Temp[i].H3Max);
                SW.Write(Temp[i].H4Max);
                SW.Write(Temp[i].H5Max);
                SW.Write(Temp[i].H6Max);
                SW.Write(Temp[i].H7Max);
                SW.Write(Temp[i].H8Max);

                SW.Write(Temp[i].F1Min);
                SW.Write(Temp[i].F2Min);
                SW.Write(Temp[i].F3Min);
                SW.Write(Temp[i].F4Min);
                SW.Write(Temp[i].F5Min);
                SW.Write(Temp[i].F6Min);
                SW.Write(Temp[i].F7Min);
                SW.Write(Temp[i].F8Min);
                SW.Write(Temp[i].F1Max);
                SW.Write(Temp[i].F2Max);
                SW.Write(Temp[i].F3Max);
                SW.Write(Temp[i].F4Max);
                SW.Write(Temp[i].F5Max);
                SW.Write(Temp[i].F6Max);
                SW.Write(Temp[i].F7Max);
                SW.Write(Temp[i].F8Max);

                SW.Write(Temp[i].E1Min);
                SW.Write(Temp[i].E2Min);
                SW.Write(Temp[i].E3Min);
                SW.Write(Temp[i].E4Min);
                SW.Write(Temp[i].E5Min);
                SW.Write(Temp[i].E6Min);
                SW.Write(Temp[i].E7Min);
                SW.Write(Temp[i].E8Min);
                SW.Write(Temp[i].E1Max);
                SW.Write(Temp[i].E2Max);
                SW.Write(Temp[i].E3Max);
                SW.Write(Temp[i].E4Max);
                SW.Write(Temp[i].E5Max);
                SW.Write(Temp[i].E6Max);
                SW.Write(Temp[i].E7Max);
                SW.Write(Temp[i].E8Max);

                SW.Write(Temp[i].X1Min);
                SW.Write(Temp[i].X2Min);
                SW.Write(Temp[i].X3Min);
                SW.Write(Temp[i].X4Min);
                SW.Write(Temp[i].X5Min);
                SW.Write(Temp[i].X6Min);
                SW.Write(Temp[i].X7Min);
                SW.Write(Temp[i].X8Min);
                SW.Write(Temp[i].X1Max);
                SW.Write(Temp[i].X2Max);
                SW.Write(Temp[i].X3Max);
                SW.Write(Temp[i].X4Max);
                SW.Write(Temp[i].X5Max);
                SW.Write(Temp[i].X6Max);
                SW.Write(Temp[i].X7Max);
                SW.Write(Temp[i].X8Max);

                SW.Write(Temp[i].Y1Min);
                SW.Write(Temp[i].Y2Min);
                SW.Write(Temp[i].Y3Min);
                SW.Write(Temp[i].Y4Min);
                SW.Write(Temp[i].Y5Min);
                SW.Write(Temp[i].Y6Min);
                SW.Write(Temp[i].Y7Min);
                SW.Write(Temp[i].Y8Min);
                SW.Write(Temp[i].Y1Max);
                SW.Write(Temp[i].Y2Max);
                SW.Write(Temp[i].Y3Max);
                SW.Write(Temp[i].Y4Max);
                SW.Write(Temp[i].Y5Max);
                SW.Write(Temp[i].Y6Max);
                SW.Write(Temp[i].Y7Max);
                SW.Write(Temp[i].Y8Max);
            }
           
            byte[] Encrypted = GBCrypto.Compress(FileStream.ToArray());
            StreamDataWrite Ouput = new StreamDataWrite();
            Ouput.WriteBytes(CRC);
            Ouput.WriteBytes(Encrypted);
            File.WriteAllBytes("stage.dat", Ouput.ToByteArray());
        }
    }
}
