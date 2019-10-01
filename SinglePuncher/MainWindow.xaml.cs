using SinglePuncher.Classes;
using SinglePuncher.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        GCodeObject gCodeObject = new GCodeObject();

        private readonly SynchronizationContext synchronizationContext;
        Classes.SinglePuncher SP;
        Line yMeasure = new Line();
        Line xMeasure = new Line();
        bool mouseClicked = false;
        public static Size SheetSize { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            DisableGCodeButtons();
            SP = new Classes.SinglePuncher();
            UpdateUIAsync();
        }

        private async void UpdateUIAsync()
        {                  
            while (true)
            {
                await SP.UpdateData();
                UpdateUIMethod();
            }            
        }

        private void UpdateUIMethod()
        {

            if (this.gCodeObject.gCodeList.Count > 0 && (GCodeView.SelectedIndex <= gCodeObject.gCodeList.Count) && GCodeView.SelectedIndex >= 0)
            {
                cmdRemove.IsEnabled = true;
            }
            else { cmdRemove.IsEnabled = false; }


            StartPunchButton.IsEnabled = SP.ReferencedAxes && !SP.IsMoving && !SP.Punching && (gCodeObject.gCodeList.Count > 0) && !SP.IsMDIAlive;

            StopPunchButton.IsEnabled = SP.ReferencedAxes && (SP.IsMoving || SP.Punching);

            MDIButton.IsEnabled = SP.ReferencedAxes && !SP.Punching;

            if (SP.Connected)
            {
                if (SP.EmergencyStop) { SP.StopPunch(); }
                if (SP.ReferencedAxes)
                {                    
                    if (SP.Punching || SP.IsMoving) { DisableJogButtons(); } else { EnableJogButtons(); }                    
                }

                if (SP.Punching)
                {
                    cmdReferenceAxes.IsEnabled = false;              
                    GCodeView.SelectedIndex = SP.ProgramIndex;
                }
                else{cmdReferenceAxes.IsEnabled = true;}

                XAxisPosition.Text = string.Format("X Axis Position: {0:0.00} mm.", SP.XAxisPosition);
                YAxisPosition.Text = string.Format("Y Axis Position: {0:0.00} mm.", SP.YAxisPosition);

                if(SP.XAxisPosition <= MainCanvas.Width && SP.XAxisPosition > 0 && SP.YAxisPosition <= MainCanvas.Height && SP.YAxisPosition > 0)
                    Arrow.Margin = new Thickness((SP.XAxisPosition), (SP.YAxisPosition), 0, 0);
            }
        }

        private void EnableGCodeButtons()
        {
            cmdLineAtAngle.IsEnabled = true;
            cmdBoltHoleCircle.IsEnabled = true;
            cmdLinearContour.IsEnabled = true;
            cmdCurveContour.IsEnabled = true;
            cmdGridX.IsEnabled = true;
            cmdGridY.IsEnabled = true;
            cmdArc.IsEnabled = true;
            cmdRecWinP.IsEnabled = true;
            cmdRecWinF.IsEnabled = true;
            cmdSinglePunch.IsEnabled = true;
            cmdRemove.IsEnabled = true;
            cmdDuplicate.IsEnabled = true;
            cmdDuplicateAll.IsEnabled = true;
        }

        private void DisableGCodeButtons()
        {
            cmdLineAtAngle.IsEnabled = false;
            cmdBoltHoleCircle.IsEnabled = false;
            cmdLinearContour.IsEnabled = false;
            cmdCurveContour.IsEnabled = false;
            cmdGridX.IsEnabled = false;
            cmdGridY.IsEnabled = false;
            cmdArc.IsEnabled = false;
            cmdRecWinP.IsEnabled = false;
            cmdRecWinF.IsEnabled = false;
            cmdSinglePunch.IsEnabled = false;
            cmdRemove.IsEnabled = false;
            cmdDuplicate.IsEnabled = false;
            cmdDuplicateAll.IsEnabled = false;
        }

        private void EnableJogButtons()
        {
            cmdJogXForward.IsEnabled = true;
            cmdJogXReverse.IsEnabled = true;
            cmdJogYForward.IsEnabled = true;
            cmdJogYReverse.IsEnabled = true;
        }

        private void DisableJogButtons()
        {
            cmdJogXForward.IsEnabled = false;
            cmdJogXReverse.IsEnabled = false;
            cmdJogYForward.IsEnabled = false;
            cmdJogYReverse.IsEnabled = false;
        }

        private void loadFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            gCodeObject = FileManager.OpenFile(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);
            SheetSize = gCodeObject.sheetSize;
            SheetWidthTextBlock.Text = string.Format("{0:0.00}", gCodeObject.sheetSize.Width);
            SheetHeightTextBlock.Text = string.Format("{0:0.00}", gCodeObject.sheetSize.Height);

            StartPunchButton.IsEnabled = true;
            EnableGCodeButtons();
        }

        private void saveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileManager.SaveFile(gCodeObject);
        }

        private void SetSheetDimensions_Click(object sender, RoutedEventArgs e)
        {
            SetSheetProperties SSP = new SetSheetProperties();

            SSP.ShowDialog();

            if (SSP.sheetSize == null || SSP.sheetSize.Height <= 0 || SSP.sheetSize.Width <= 0)
                return;

            gCodeObject.gCodeList.Clear();

            gCodeObject.sheetSize = SSP.sheetSize;
            SheetSize = SSP.sheetSize;
            MainCanvas.Width = SSP.sheetSize.Width;
            MainCanvas.Height = SSP.sheetSize.Height;
            SheetWidthTextBlock.Text = string.Format("{0:0.00}", gCodeObject.sheetSize.Width);
            SheetHeightTextBlock.Text = string.Format("{0:0.00}", gCodeObject.sheetSize.Height);

            double scx = Math.Abs((canvasColumn.ActualWidth - 3) / MainCanvas.Width);
            double scy = Math.Abs((canvasRow.ActualHeight - 3) / MainCanvas.Height);

            if(scx < scy)
            {
                gCodeObject.canvasScale = scx;
                MainCanvasScale.ScaleX = scx;
                MainCanvasScale.ScaleY = scx;            
            }
            else
            {
                gCodeObject.canvasScale = scy;
                MainCanvasScale.ScaleX = scy;
                MainCanvasScale.ScaleY = scy;
            }

            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);
            EnableGCodeButtons();
            EnableJogButtons();
            
        }        

        private void GcodeButton_Click(object sender, RoutedEventArgs e)
        {
            Button GcodeButton = (Button)sender;
            GcodeManager.AddButtonGcode(GcodeButton.Name, ref gCodeObject.gCodeList);
            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);

            if (gCodeObject.gCodeList.Count > 0 || gCodeObject.gCodeList == null)
                StartPunchButton.IsEnabled = true;

            saveFileMenuItem.IsEnabled = true;

        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            SP.StopPunch();
            gCodeObject.gCodeList.RemoveAt(GCodeView.SelectedIndex);
            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);
            if (gCodeObject.gCodeList.Count > 0)
                GCodeView.SelectedIndex = (GCodeView.Items.Count - 1);
        }       

        private void GCodeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GcodeViewSelectionChanged();
        }

        private void GcodeViewSelectionChanged()
        {
            if (!(GCodeView.Items.Count > 0) || !(gCodeObject.gCodeList.Count > 0))
                return;

            int index = GCodeView.SelectedIndex;

            for (int i = 0; i < gCodeObject.gCodeList.Count; i++)
            {
                gCodeObject.gCodeList.ElementAt(i).selected = (i == index) ? true : false;
            }

            cmdRemove.IsEnabled = true;

            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);

            GCodeView.SelectionChanged -= GCodeView_SelectionChanged;

            GCodeView.SelectedIndex = index;

            GCodeView.SelectionChanged += GCodeView_SelectionChanged;
        }

        private void StartPunchButton_Click(object sender, RoutedEventArgs e)
        {
            if(gCodeObject.gCodeList.Count <= 0 || gCodeObject.gCodeList == null)
            {
                MessageBox.Show("No existen comandos de punzonado");
                return;
            }

            StopPunchButton.IsEnabled = true;

            int index = GCodeView.SelectedIndex;

            if (index < 1)
                index = 0;

            bool disablePunch = PunchDisableCheckBox.IsChecked == true;

            SP.StartPunch(gCodeObject.gCodeList, index, disablePunch);                     
        }

        private void StopPunchButton_Click(object sender, RoutedEventArgs e)
        {
            SP.StopPunch();
        }

        private void cmdReferenceAxes_Click(object sender, RoutedEventArgs e)
        {
            //SP.ReferenceAxes();

            loadFileMenuItem.IsEnabled = true;
            SetSheetDimensions.IsEnabled = true;
            MDIButton.IsEnabled = true;
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var msgResponse = MessageBox.Show("¿Está seguro que desea Salir?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (msgResponse == MessageBoxResult.Yes)
            {
                SP.StopHydraulicPump();
                SP.StopPunch();
                SP.StopMDI();
                SP.Disconnect();
                Application.Current.Shutdown();
                System.Environment.Exit(0);
            }               
        }            

        private void MoveMeasureLines(Point pos)
        {            
            yMeasure.X1 = 0;
            yMeasure.X2 = MainCanvas.Width;
            yMeasure.Y1 = pos.Y;
            yMeasure.Y2 = pos.Y;
            yMeasure.Stroke = Brushes.Green;
            yMeasure.StrokeThickness = 1;

            xMeasure.X1 = pos.X;
            xMeasure.X2 = pos.X;
            xMeasure.Y1 = 0;
            xMeasure.Y2 = MainCanvas.Height;
            xMeasure.Stroke = Brushes.Green;
            xMeasure.StrokeThickness = 1;

            xCoords.Text = string.Format("{0:0.00}", pos.X);
            yCoords.Text = string.Format("{0:0.00}", pos.Y);
        }

        private void MainCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MoveLines(e);
        }

        private void MoveLines(object mouseEventArgs)
        {
            MouseEventArgs e = (MouseEventArgs)mouseEventArgs;            

            double x = e.GetPosition(MainCanvas).X;
            double y = e.GetPosition(MainCanvas).Y;
            
            if(mouseClicked)
                MoveMeasureLines(e.GetPosition(MainCanvas));
            
        }

        private void MainCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseClicked = true;
            MoveLines(e);
        }

        private void MainCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseClicked = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainCanvas.Children.Add(yMeasure);
            yMeasure.Stroke = Brushes.Transparent;

            this.MainCanvas.Children.Add(xMeasure);
            xMeasure.Stroke = Brushes.Transparent;
        }

        private void cmdJogYForward_Click(object sender, RoutedEventArgs e)
        {
            SP.JOGYForward();
        }

        private void cmdJogXReverse_Click(object sender, RoutedEventArgs e)
        {
            if (SP.XAxisPosition < 100)
                return;

            SP.JOGXReverse();
        }

        private void cmdJogXForward_Click(object sender, RoutedEventArgs e)
        {
            SP.JOGXForward();
        }

        private void cmdJogYReverse_Click(object sender, RoutedEventArgs e)
        {

            if (SP.YAxisPosition < 100)
                return;

            SP.JOGYReverse();
        }

        private void GCodeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Codigo de prueba para modificar los parametros de un GCode, cuando está creado en el GCodeView

            //if (!(GCodeView.Items.Count > 0) || !(gCodeObject.gCodeList.Count > 0))
            //    return;

            //int index = GCodeView.SelectedIndex;

            //Gcode modifiyngGcode = gCodeObject.gCodeList[index];
            
            //switch(modifiyngGcode.ToString())
            //{
            //    case "LAAV":
            //        {
            //            gCodeObject.gCodeList[index] = GcodeManager.CreateLineAtAngle();
            //            break;
            //        }
            //    case "asdasdasd":
            //        {
            //            break;
            //        }
            //    case "aassd":
            //        {
            //            break;
            //        }
            // }

        }

        private void MDIButton_Click(object sender, RoutedEventArgs e)
        {            
            if(MDIButton.Background == Brushes.LawnGreen)
            {

                StartPunchButton.IsEnabled = false;
                SP.StopPunch();

                Point TargetPoint = new Point(HelperClass.ShowInputBox("Enter X Axis Position:"), 
                    HelperClass.ShowInputBox("Enter Y Axis Position:"));                               

                MDIButton.Background = Brushes.Red;
                MDIButton.Content = "Stop MDI";

                SP.MDI(TargetPoint);                
            }
            else
            {
                SP.StopMDI();
                MDIButton.Background = Brushes.LawnGreen;
                MDIButton.Content = "MDI";
            }            
        }

        private void DuplicateGcode(double distXDir, int numXDir, double distYDir, int numYDir, int index)
        {
            Gcode gcodeToCopy = CloneClass.CloneObject<Gcode>(gCodeObject.gCodeList[index]);

            double distX = gCodeObject.gCodeList[index].xStart;
            double distY = gCodeObject.gCodeList[index].yStart;

            for (int x = 0; x <= numXDir; x++)
            {
                for (int y = 1; y < numYDir; y++)
                {
                    distY += distYDir;

                    Gcode copyY = CloneClass.CloneObject<Gcode>(gcodeToCopy);

                    copyY.xStart = distX;
                    copyY.yStart = distY;

                    gCodeObject.gCodeList.Add(copyY);
                }

                distYDir *= -1;

                distX += distXDir;

                if (x >= numXDir - 1)
                    break;

                Gcode copyX = CloneClass.CloneObject<Gcode>(gcodeToCopy);

                copyX.xStart = distX;
                copyX.yStart = distY;

                gCodeObject.gCodeList.Add(copyX);
            }

            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);
        }

        private void cmdDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (gCodeObject.gCodeList.Count < 1 || GCodeView.SelectedIndex < 0)
                return;            

            double distXDir = HelperClass.ShowInputBox("Distance between Macros in X Direction");
            int numXDir = Convert.ToInt32(HelperClass.ShowInputBox("Number of Macros in X Direction"));
            double distYDir = HelperClass.ShowInputBox("Distance between Macros in Y Direction");
            int numYDir = Convert.ToInt32(HelperClass.ShowInputBox("Number of Macros in Y Direction"));                

            int index = GCodeView.SelectedIndex;

            DuplicateGcode(distXDir, numXDir, distYDir, numYDir, index);            
        }

        private void cmdDuplicateAll_Click(object sender, RoutedEventArgs e)
        {
            if (gCodeObject.gCodeList.Count < 1)
                return;

            double distXDir = HelperClass.ShowInputBox("Distance between Macros in X Direction");
            int numXDir = Convert.ToInt32(HelperClass.ShowInputBox("Number of Macros in X Direction"));
            double distYDir = HelperClass.ShowInputBox("Distance between Macros in Y Direction");
            int numYDir = Convert.ToInt32(HelperClass.ShowInputBox("Number of Macros in Y Direction"));

            List<Gcode> DuplicatedList = new List<Gcode>();          
             
            foreach(Gcode item in gCodeObject.gCodeList)
            {
                Gcode gcodeToCopy = CloneClass.CloneObject<Gcode>(item);
                DuplicatedList.Add(gcodeToCopy);
                double distX = item.xStart;
                double distY = item.yStart;
                
                double distYDirFixed = distYDir;

                for (int x = 0; x <= numXDir; x++)
                {
                    for (int y = 1; y < numYDir; y++)
                    {
                        distY += distYDirFixed;

                        Gcode copyY = CloneClass.CloneObject<Gcode>(gcodeToCopy);

                        copyY.xStart = distX;
                        copyY.yStart = distY;

                        DuplicatedList.Add(copyY);                        
                    }

                    distYDirFixed *= -1;

                    distX += distXDir;

                    if (x >= numXDir - 1)
                        break;

                    Gcode copyX = CloneClass.CloneObject<Gcode>(gcodeToCopy);

                    copyX.xStart = distX;
                    copyX.yStart = distY;

                    DuplicatedList.Add(copyX);
                }
            }

            gCodeObject.gCodeList = DuplicatedList;          

            gCodeObject.DrawObjects(ref MainCanvas, ref MainCanvasScale, ref GCodeView, ref Arrow, xMeasure, yMeasure);

        }

        private void MainCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            xMeasure.Stroke = Brushes.Transparent;
            yMeasure.Stroke = Brushes.Transparent;           
        }
    }
}
