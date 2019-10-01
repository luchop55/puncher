using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Views;
using System;
using System.Collections.Generic;

namespace SinglePuncher.Classes
{
    public static class GcodeManager
    {
        public static void AddButtonGcode(String buttonName, ref List<Gcode> gcodeList)
        {
            switch (buttonName)
            {
                case "cmdLineAtAngle":
                    AddLineAtAngle(ref gcodeList);
                    break;
                case "cmdBoltHoleCircle":
                    AddBoltHoleCircle(ref gcodeList);
                    break;
                case "cmdLinearContour":
                    AddLinearContour(ref gcodeList);
                    break;
                case "cmdCurveContour":
                    AddCurveContour(ref gcodeList);
                    break;
                case "cmdGridX":
                    AddGridX(ref gcodeList);
                    break;
                case "cmdGridY":
                    AddGridY(ref gcodeList);
                    break;
                case "cmdArc":
                    AddArc(ref gcodeList);
                    break;
                case "cmdRecWinP":
                    AddRecWinP(ref gcodeList);
                    break;
                case "cmdRecWinF":
                    AddRecWinF(ref gcodeList);
                    break;
                case "cmdSinglePunch":
                    AddSinglePunch(ref gcodeList);
                    break;                
            }
        }

        private static void AddLineAtAngle(ref List<Gcode> gcodeList)
        {
            //LineAtAngleView LAAV = new LineAtAngleView();            
            //LineAtAngle LAA = LAAV.ShowWindow();
            LineAtAngle LAA = CreateLineAtAngle();
            if(LAA != null)
            {
                gcodeList.Add(LAA);

                //gcodeList.Insert(0, LAA);
            }
        }

        public static LineAtAngle CreateLineAtAngle()
        {
            LineAtAngleView LAAV = new LineAtAngleView();
            LineAtAngle LAA = LAAV.ShowWindow();
            return LAA;
        }

        private static void AddBoltHoleCircle(ref List<Gcode> gcodeList)
        {
            BoltHoleCircleView BHCV  = new BoltHoleCircleView();
            BoltHoleCircle BHC = BHCV.ShowWindow();
            if (BHC != null)
            {
                gcodeList.Add(BHC);

                //gcodeList.Insert(0, BHC);
            }
        }

        private static void AddLinearContour(ref List<Gcode> gcodeList)
        {
            LinearContourView LCV = new LinearContourView();
            LinearContour LC = LCV.ShowWindow();
            if (LC != null)
            {
                gcodeList.Add(LC);

                //gcodeList.Insert(0, LC);
            }
        }

        private static void AddCurveContour(ref List<Gcode> gcodeList)
        {
            CurveContourView CCV= new CurveContourView();
            CurveContour CC = CCV.ShowWindow();
            if (CC != null)
            {
                gcodeList.Add(CC);

                //gcodeList.Insert(0, CC);
            }
        }

        private static void AddGridX(ref List<Gcode> gcodeList)
        {
            GridXView GXV = new GridXView();
            GridX GX = GXV.ShowWindow();
            if (GX != null)
            {
                gcodeList.Add(GX);

                //gcodeList.Insert(0, GX);
            }
        }

        private static void AddGridY(ref List<Gcode> gcodeList)
        {
            GridYView GYV = new GridYView();
            GridY GY = GYV.ShowWindow();
            if (GY != null)
            {
                gcodeList.Add(GY);

                //gcodeList.Insert(0, GY);
            }
        }

        private static void AddArc(ref List<Gcode> gcodeList)
        {
            ArcView AV = new ArcView();
            Arc A = AV.ShowWindow();
            if (A != null)
            {
                gcodeList.Add(A);

                //gcodeList.Insert(0, A);
            }
        }

        private static void AddRecWinF(ref List<Gcode> gcodeList)
        {
            RecWinFView RWFV = new RecWinFView();
            RecWinF RWF = RWFV.ShowWindow();
            if (RWF != null)
            {
                gcodeList.Add(RWF);

                //gcodeList.Insert(0, RWF);
            }
        }

        private static void AddRecWinP(ref List<Gcode> gcodeList)
        {
            RecWinPView RWPV = new RecWinPView();
            RecWinP RWP = RWPV.ShowWindow();
            if (RWP != null)
            {
                gcodeList.Add(RWP);

                //gcodeList.Insert(0, RWP);
            }
        }

        private static void AddSinglePunch(ref List<Gcode> gcodeList)
        {
            SinglePunchView SPV = new SinglePunchView();
            SinglePunch SP = SPV.ShowWindow();
            if (SP != null)
            {
                gcodeList.Add(SP);

                //gcodeList.Insert(0, SP);
            }
        }
    }
}
