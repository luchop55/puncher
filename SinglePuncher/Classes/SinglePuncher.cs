using CoilWinderCCTT.Classes;
using SinglePuncher.Classes;
using SinglePuncher.Classes.Tools;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SinglePuncher.Classes
{
    public class SinglePuncher
    {        
        FXEnet2 myFXEnet2;

        public bool EmergencyStop { get; private set; }
        //public bool Punching{ get{if (punchProcess != null) { return punchProcess.IsAlive; } else { return false; }}}

        public bool Punching { get; private set; }

        public bool IsMoving { get { return (myFXEnet2.BitMRead(Pulse_Count_Y000) || myFXEnet2.BitMRead(Pulse_Count_Y001)); } }

        public bool IsMDIAlive { get; private set; }

        public bool Connected { get; private set; }

        public bool OpenHoodDoor { get; private set; }
        public bool ReferencedAxes { get; set; }
        public bool ToolRecSWOpen { get; private set; }
        public double XAxisPosition { get; private set; }
        public double YAxisPosition { get; private set; }
        public Int32 TargetDirectionX { get; set; }
        public Int32 TargetDirectionY { get; set; }
        private const int Limit_Stroke_Negative_X = 5;
        private const int Limit_Stroke_Negative_Y = 7;
        private const int Forward_JOG_Command_X = 14;
        private const int Reverse_JOG_Command_X = 15;
        private const int Foot_Pedal = 16;
        private const int Forward_JOG_Command_Y = 16;
        private const int Reverse_JOG_Command_Y = 17;
        private const int Start_Hydraulic_Pump = 27;
        private const int Target_Direction_X = 10;
        private const int Target_Direction_Y = 12;
        private const int Punch = 28;
        private const int Punch_Process_Completion = 30;
        private const int Zero_Return_Command_X = 13;
        private const int Zero_return_completion_Y000 = 10;
        private const int Zero_Return_Command_Y = 26;
        private const int Zero_return_completion_Y001 = 11;
        private const int Emergency_Stop = 4;
        private const int Referenced_Axes = 18;
        private const int Pulse_Count_Y000 = 8340;
        private const int Pulse_Count_Y001 = 8350;
        private const int Open_Hood_Door = 6;
        private const int Tool_Rec_SW = 11;
        private const int Claxon_Bit = 31;
        private List<Gcode> gcodeList;
        private Thread punchProcess;
        Thread punchThread;
        private String currentTool = "";

        public int ProgramIndex { get; private set;}
        public bool DisablePunch { get; set; }


        public SinglePuncher()
        {
            string plcIP = ConfigurationManager.AppSettings.Get("PLC.IP");
            string plcPort = ConfigurationManager.AppSettings.Get("PLC.PORT");

            myFXEnet2 = new FXEnet2(plcIP, int.Parse(plcPort));

            //myFXEnet2.conectar();

            StartHydraulicPump();
        }

        public void Disconnect()
        {
            if (punchProcess != null)
            {
                if (punchProcess.IsAlive)
                {
                    punchProcess.Abort();
                    punchProcess.Join();
                    myFXEnet2.close();
                }
            }            
        }

        public Task UpdateData()
        {
            return Task.Run(() =>
            {
                EmergencyStop = !myFXEnet2.BitXRead(Emergency_Stop);
                OpenHoodDoor = !myFXEnet2.BitXRead(Open_Hood_Door);
                ToolRecSWOpen = !myFXEnet2.BitXRead(Tool_Rec_SW);
                XAxisPosition = Convert.ToDouble(myFXEnet2.SignedDoubleIntRead(Pulse_Count_Y000))/ 100;
                YAxisPosition = Convert.ToDouble(myFXEnet2.SignedDoubleIntRead(Pulse_Count_Y001)) / 100;
                Connected = myFXEnet2.Connected;

                if(punchProcess != null) { Punching = punchProcess.IsAlive;}
            });
            
        }

        public void StopPunch()
        {
            if (punchProcess != null)
            {
                if (punchProcess.IsAlive)
                {
                    punchProcess.Abort();
                    punchProcess.Join();
                }
            }
        }

        public void StartPunch(List<Gcode> _gcodeList, int startIndex, bool disablePunch)
        {
            this.DisablePunch = disablePunch;
            gcodeList = _gcodeList;
            punchProcess = new Thread(delegate () { PunchProcess(startIndex); });
            punchProcess.Start();
        }        

        private void PunchProcess(int startIndex)
        {           
            while (true)
            {
                currentTool = "";

                this.TurnOnClaxon();

                for (int i = startIndex; i < gcodeList.Count; i++)
                {
                    ProgramIndex = i;

                    if (currentTool != gcodeList[i].tool.ToString() && currentTool != "")
                    {
                        break;
                    }

                    gcodeList[i].Punch(this);
                    currentTool = gcodeList[i].tool.ToString();
                }

                this.TurnOnClaxon();               
            }
            
        }
        

        public void MDI(Point punchPoint)
        {
            this.DisablePunch = false;

            MoveCommand(punchPoint);
            punchThread = new Thread(delegate () 
            {
                while (true)
                {
                    PunchInPlace();
                }
            });

            this.IsMDIAlive = true;
            punchThread.Start();
        }      

        public void StopMDI()
        {
            if (punchThread != null)
            {
                if (punchThread.IsAlive)
                {
                    myFXEnet2.BitMWrite(Punch, false);
                    punchThread.Abort();
                    punchThread.Join();
                    this.IsMDIAlive = false;
                }                
            }                
        }

        public void JOGXForward()
        {
            myFXEnet2.BitMWrite(Forward_JOG_Command_X, false);
            myFXEnet2.BitMWrite(Forward_JOG_Command_X, true);
        }

        public void JOGXReverse()
        {
            myFXEnet2.BitMWrite(Reverse_JOG_Command_X, false);
            myFXEnet2.BitMWrite(Reverse_JOG_Command_X, true);
        }

        public void JOGYForward()
        {
            myFXEnet2.BitMWrite(Forward_JOG_Command_Y, false);
            myFXEnet2.BitMWrite(Forward_JOG_Command_Y, true);
        }

        public void JOGYReverse()
        {
            myFXEnet2.BitMWrite(Reverse_JOG_Command_Y, false);
            myFXEnet2.BitMWrite(Reverse_JOG_Command_Y, true);
        }

        public void TurnOnClaxon()
        {
            myFXEnet2.BitMWrite(Claxon_Bit, true);
        }

        public void StartHydraulicPump() { myFXEnet2.BitMWrite(Start_Hydraulic_Pump, true); }

        public void StopHydraulicPump() { myFXEnet2.BitMWrite(Start_Hydraulic_Pump, false); }

        public void MoveCommand(Point movePoint)
        {

            if (movePoint.X < 25 || movePoint.X >= 1830 || movePoint.Y >= 762)           //Evita que los ejes choquen contra el punzon o se salgan de rango
                return;

            TargetDirectionX = Convert.ToInt32(movePoint.X * 100);
            TargetDirectionY = Convert.ToInt32(movePoint.Y * 100);

            myFXEnet2.SignedDoubleIntWrite(Target_Direction_X, TargetDirectionX);   //Cargar direccion de destino a eje X
            myFXEnet2.SignedDoubleIntWrite(Target_Direction_Y, TargetDirectionY);   //Cargar direccion de destino a eje Y
            myFXEnet2.BitMWrite(19, true);                                          //Comando para mover ambos ejes a posicion absoluta
                      
            if (DisablePunch || !myFXEnet2.BitMRead(Start_Hydraulic_Pump))  //Si la bomba hidraulica no está funcionando o se deshabilita el punzonado
            {                                                               //la maquina no punzona

                while (!myFXEnet2.BitMRead(Foot_Pedal)) { DoEvents();}      //Esperar a que se pise el pedal
                return;                                                     
            }                

            PunchInPlace();
        }

        public void PunchInPlace()
        {
            myFXEnet2.BitMWrite(Punch, true);                           //Comando para punzonar

            do
            {
                DoEvents();
            } while (!myFXEnet2.BitMRead(Punch_Process_Completion));     //Esperar hasta que el proceso de punzonado termine
        }

        public void ReferenceAxes()
        {
            if (!myFXEnet2.BitXRead(Limit_Stroke_Negative_X))
                JOGXReverse();

            if (!myFXEnet2.BitXRead(Limit_Stroke_Negative_Y))
                JOGYReverse();

            while(!myFXEnet2.BitXRead(Limit_Stroke_Negative_X) || !myFXEnet2.BitXRead(Limit_Stroke_Negative_Y))
            {
                Thread.Sleep(2000);
            }

            myFXEnet2.BitMWrite(Zero_Return_Command_X, false);

            myFXEnet2.BitMWrite(Zero_Return_Command_X, true);
            do
            {
                DoEvents();
            } while (!myFXEnet2.BitMRead(Zero_return_completion_Y000));

            myFXEnet2.SignedDoubleIntWrite(Pulse_Count_Y000, 183000);        //cuando se referencia el eje X se carga 1830mm en la posicion

            myFXEnet2.BitMWrite(Zero_Return_Command_Y, false);

            myFXEnet2.BitMWrite(Zero_Return_Command_Y, true);
            do
            {
                DoEvents();
            } while (!myFXEnet2.BitMRead(Zero_return_completion_Y001));

            myFXEnet2.SignedDoubleIntWrite(Pulse_Count_Y001, 76200);         //cuando se referencia el eje Y se carga 762mm en la posicion

            ReferencedAxes = true;
        }

        private void DoEvents()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
            catch
            {
            }
        }

    }
}
