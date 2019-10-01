using System;
using System.Net.Sockets;
using System.Windows;

namespace SinglePuncher.Classes

{
    public class FXEnet2
    {
        TcpClient client = new TcpClient();
        string _ipAddress;
        int _portNum;
        byte[] buffer;
        byte[] inBuff = new byte[1532];
        String txCommand = "";
        String rxResponse = "";
        String temp = "";
        int j = 0;
        short[] intDReg;
        String dRegStr = "";
        String subHeader = "";

        public bool Connected { get { return client.Connected; } }

        public FXEnet2(string ipAddress, int portNum)
        {
            _ipAddress = ipAddress;
            _portNum = portNum;
        }

        public void conectar()
        {
            try
            {
                client.Connect(_ipAddress, _portNum);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la conexión por el siguiente error " + ex.ToString());
                return;
            }
        }

        public void close()
        {
            if (client.Connected)
            {
                client.GetStream().Close();
                client.Close();
            }
        }

        object locker = new object();
        object locker2 = new object();

        short[] signedDoubleArr = new short[2];
        Int32 signedDouble = 0;
        string signedDoubleStr = "";
        Single dataFltRead = 0;
        byte[] byArrBuffArray;
        short[] intBuffArray = new short[2];
        short[] int32BuffArray = new short[2];
        short[] dataValuesArr = new short[1];
        short[] signedIntArr = new short[1];


        public bool BitXRead(int dataBitX)
        {
            lock (locker)
            {
                bool bitState = false;

                if (!client.Connected)
                {
                    return bitState;
                }

                txCommand = "";
                txCommand += "00";                                      //Batch Read
                txCommand += "ff";                                      //PC Local
                txCommand += "000a";                                    //Monitoring timer
                txCommand += "5820";                                    //Data register M
                txCommand += string.Format("{0:X8}", dataBitX);         //Head device number
                txCommand += string.Format("{0:X2}", 1);                //Number of device points
                txCommand += "00";                                      //Termination code

                SendReceiveData();

                if (subHeader == "00")
                {
                    if (rxResponse.Substring(4, 1) == "1")
                    {
                        bitState = true;
                    }
                    else
                    {
                        bitState = false;
                    }
                }
                else if (subHeader == "5B")
                {
                    temp = "Terminate Code = " + subHeader + "Error code = " + rxResponse.Substring(5, 2);
                    MessageBox.Show(temp);
                }
                else
                {
                    temp = "Terminate Code = " + subHeader;
                    MessageBox.Show(temp);
                }
                return bitState;
            }

        }

        public bool BitMRead(int dataBitM)
        {
            lock (locker)
            {
                bool bitState = false;

                if (!client.Connected)
                {
                    return bitState;
                }

                txCommand = "";
                txCommand += "00";                                      //Batch Read
                txCommand += "ff";                                      //PC Local
                txCommand += "000a";                                    //Monitoring timer
                txCommand += "4d20";                                    //Data register M
                txCommand += string.Format("{0:X8}", dataBitM);         //Head device number
                txCommand += string.Format("{0:X2}", 1);                //Number of device points
                txCommand += "00";                                      //Termination code

                SendReceiveData();


                if (subHeader == "00")
                {
                    if (rxResponse.Substring(4, 1) == "1")
                    {
                        bitState = true;
                    }
                    else
                    {
                        bitState = false;
                    }
                }
                else if (subHeader == "5B")
                {
                    temp = "Terminate Code = " + subHeader + "Error code = " + rxResponse.Substring(5, 2);
                    MessageBox.Show(temp);
                }
                else
                {
                    temp = "Terminate Code = " + subHeader;
                    MessageBox.Show(temp);
                }
                return bitState;
            }
        }

        public void BitMWrite(int dataBitM, bool[] bitValues)
        {
            lock (locker)
            {
                if (!client.Connected)
                {
                    return;
                }

                txCommand = "";
                txCommand += "02";                                          //Batch Read
                txCommand += "ff";                                          //PC Local
                txCommand += "000a";                                        //Monitoring timer
                txCommand += "4d20";                                        //Data register D
                txCommand += string.Format("{0:X8}", dataBitM);            //Head device number
                txCommand += string.Format("{0:X2}", bitValues.Length);    //Number of device points
                txCommand += "00";

                foreach (bool bitState in bitValues)
                {
                    if (bitState == true)
                    {
                        txCommand += "1";
                    }
                    else
                    {
                        txCommand += "0";
                    }
                }

                if (bitValues.Length % 2 != 0)
                {
                    txCommand += "0";
                }

                SendReceiveData();

                if (subHeader == "00")
                {
                    //MessageBox.Show("escritura exitosa");
                }
                else if (subHeader == "5B")
                {
                    temp = "Terminate Code = " + subHeader + "Error code = " + rxResponse.Substring(5, 2);
                    MessageBox.Show(temp);
                }
                else
                {
                    temp = "Terminate Code = " + subHeader;
                    MessageBox.Show(temp);
                }
            }

        }

        public void BitMWrite(int dataBitM, bool bitValue)
        {
            lock (locker)
            {
                bool[] oneBit = new bool[1];

                if (!client.Connected)
                {
                    return;
                }

                if (bitValue)
                {
                    oneBit[0] = true;
                }
                else
                {
                    oneBit[0] = false;
                }
                BitMWrite(dataBitM, oneBit);
            }

        }

        public short SignedIntRead(int DReg)
        {
            signedIntArr = SignedBatchIntRead(DReg, 1);
            return signedIntArr[0];
        }

        public short[] SignedBatchIntRead(int startDReg, int dataLenght)
        {
            lock (locker)
            {
                intDReg = new short[dataLenght];

                if (!client.Connected)

                {

                    return intDReg;

                }
                //Se le asigna el tamaño a intDReg con cantidad de datos a leer  

                if (dataLenght > 32) return intDReg;                    //Máximo 32 registros para leer                      


                txCommand = "01ff000a4420";

                txCommand += string.Format("{0:X8}", startDReg);        //Head device number

                txCommand += string.Format("{0:X2}", dataLenght);       //Number of device points

                txCommand += "00";                                      //Termination code

                SendReceiveData();

                if (subHeader == "00")
                {
                    temp = "";

                    for (j = 0; j <= dataLenght - 1; j++)
                    {
                        if (dataLenght == intDReg.Length - 1)
                        {
                            break;
                        }
                        dRegStr = rxResponse.Substring((j * 4) + 4, 4);
                        intDReg[j] = Int16.Parse(dRegStr, System.Globalization.NumberStyles.HexNumber);
                    }
                }

                else if (subHeader == "5B")
                {
                    temp = "Terminate Code = " + subHeader + "Error code = " + rxResponse.Substring(5, 2);
                    MessageBox.Show(temp);
                }
                else
                {
                    temp = "Terminate Code = " + subHeader;
                    MessageBox.Show(temp);
                }
                return intDReg;
            }
        }

        public void SignedIntWrite(int startDReg, short dataValue)
        {
            dataValuesArr[0] = dataValue;

            SignedBatchIntWrite(startDReg, dataValuesArr);
        }

        public void SignedBatchIntWrite(int startDReg, short[] dataValues)
        {
            lock (locker)
            {
                int[] data = new int[dataValues.Length];

                if (!client.Connected)
                {
                    return;
                }

                txCommand = "";
                txCommand += "03";                                          //Batch Read
                txCommand += "ff";                                          //PC Local
                txCommand += "000a";                                        //Monitoring timer
                txCommand += "4420";                                        //Data register D
                txCommand += string.Format("{0:X8}", startDReg);            //Head device number
                txCommand += string.Format("{0:X2}", dataValues.Length);    //Number of device points
                txCommand += "00";

                foreach (short value in dataValues)
                {
                    txCommand += (string.Format("{0:X4}", value));
                }

                SendReceiveData();

                if (subHeader == "00")
                {
                    //MessageBox.Show("Escritura exitosa!!");
                }
                else if (subHeader == "5B")
                {
                    temp = "Terminate Code = " + subHeader + "Error code = " + rxResponse.Substring(5, 2);
                    MessageBox.Show(temp);
                }
                else
                {
                    temp = "Terminate Code = " + subHeader;
                    MessageBox.Show(temp);
                }
            }

        }

        public Single DataFloatRead(int startDreg)
        {
            byte[] byarrBuff = new byte[4];

            short[] dataBuff = SignedBatchIntRead(startDreg, 2);

            byte[] byarrTemp = BitConverter.GetBytes(dataBuff[0]);
            byarrBuff[0] = byarrTemp[0];
            byarrBuff[1] = byarrTemp[1];
            byarrTemp = BitConverter.GetBytes(dataBuff[1]);
            byarrBuff[2] = byarrTemp[0];
            byarrBuff[3] = byarrTemp[1];
            dataFltRead = BitConverter.ToSingle(byarrBuff, 0);
            return dataFltRead;
        }

        public void DataFloatWrite(int Dreg, double floatNumber)
        {
            lock (locker)
            {
                if (!client.Connected)
                {
                    return;
                }

                byArrBuffArray = BitConverter.GetBytes(Convert.ToSingle(floatNumber));
                intBuffArray[0] = BitConverter.ToInt16(byArrBuffArray, 0);
                intBuffArray[1] = BitConverter.ToInt16(byArrBuffArray, 2);
                int32BuffArray[0] = Convert.ToInt16(intBuffArray[0]);
                int32BuffArray[1] = Convert.ToInt16(intBuffArray[1]);

                SignedBatchIntWrite(Dreg, int32BuffArray);

            }
        }

        public Int32 SignedDoubleIntRead(int Dreg)
        {
            signedDoubleArr = SignedBatchIntRead(Dreg, 2);

            signedDoubleStr = string.Format("{0:X4}", signedDoubleArr[1]) + string.Format("{0:X4}", signedDoubleArr[0]);

            signedDouble = Int32.Parse(signedDoubleStr, System.Globalization.NumberStyles.HexNumber);

            return signedDouble;
        }

        public void SignedDoubleIntWrite(int dReg, Int32 dataValue)
        {
            string dataValuefull = string.Format("{0:X8}", dataValue);
            string dataValue1 = dataValuefull.Substring(0, 4);
            string dataValue2 = dataValuefull.Substring(4, 4);

            short[] dataValues = new short[2];
            dataValues[0] = short.Parse(dataValue2, System.Globalization.NumberStyles.HexNumber);
            dataValues[1] = short.Parse(dataValue1, System.Globalization.NumberStyles.HexNumber);

            SignedBatchIntWrite(dReg, dataValues);
        }

        private void SendReceiveData()
        {
            lock (locker2)
            {
                buffer = System.Text.Encoding.Default.GetBytes(txCommand.ToCharArray());
                client.GetStream().Write(buffer, 0, buffer.Length);

                while (!client.GetStream().DataAvailable)
                {
                }

                if (client.GetStream().DataAvailable)
                {
                    client.GetStream().Read(inBuff, 0, inBuff.Length);
                    rxResponse = System.Text.Encoding.Default.GetString(inBuff);
                    subHeader = rxResponse.Substring(2, 2);
                }
            }

        }
    }
}
