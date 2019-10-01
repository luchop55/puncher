using System;
using System.Net.Sockets;
using System.Windows;

namespace CoilWinderCCTT.Classes
{
    public class FXEnet
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
        int[] intDReg;
        String dRegStr = "";
        String subHeader = "";

        public bool Connected { get {return client.Connected;} }

        public FXEnet(string ipAddress, int portNum)
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

        public Int32 SignedDoubleIntRead(int Dreg)
        {
            lock (locker)
            {
                Int32 intDReg = 0;

                if (!client.Connected)

                {
                    return intDReg;
                }

                txCommand = "01ff000a4420";

                txCommand += string.Format("{0:X8}", Dreg);        //Head device number

                txCommand += string.Format("{0:X2}", 2);       //Number of device points

                txCommand += "00";                                      //Termination code

                SendReceiveData();

                if (subHeader == "00")
                {                    
                        dRegStr = rxResponse.Substring(8, 4) + rxResponse.Substring(4,4);
                        intDReg = Int32.Parse(dRegStr, System.Globalization.NumberStyles.HexNumber);                    
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

        public int[] SignedIntRead(int startDReg, int dataLenght)
        {

            lock (locker)
            {
                intDReg = new int[dataLenght];

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
                        intDReg[j] = Int32.Parse(dRegStr, System.Globalization.NumberStyles.HexNumber);
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

        public double DataFloatRead(int startDReg)
        {
            lock(locker){
                double floatValue = 0.0;

                if (!client.Connected)
                {
                    return floatValue;
                }

                int[] DValues = SignedIntRead(startDReg, 2);         //leer 2 registros que componen el float
                string binary1 = Convert.ToString(DValues[0], 2).PadLeft(16, '0');   //convertirlos a strings binarios 
                string binary2 = Convert.ToString(DValues[1], 2).PadLeft(16, '0');   //convertirlos a strings binarios 
                binary1 = binary2.Substring(9, 7) + binary1;                                 //copiar los ultimos 7 bits de binary2 a binary1
                binary2 = binary2.Remove(9);

                bool positiveSign = (binary2.Substring(0, 1) == "1") ? false : true;      //Si el primer bit de la izquierda es 1 entonces el signo es positivo

                binary2 = binary2.Remove(0, 1);                                     //remueve el bit de signo para quedar solamente con los bits del exponente

                double num1 = 1;

                for (int i = 0; i <= 22; i++)                                       //conversion de la palabra compuesta por los bits A0 - A22 a int
                {
                    if (binary1.Substring(i, 1) == "1")
                    {
                        num1 += Math.Pow(2, (-i - 1));
                    }
                }

                double num2 = Convert.ToInt32(binary2, 2);                          //se convierte a double el byte compuesto por los bits E0 - E7

                floatValue = num1 * Math.Pow(2, num2 - 127);

                if (!positiveSign) floatValue *= -1;

                return floatValue;
            }
            
}       

        public void DataFloatWrite(int Dreg, double floatNumber)
        {
            lock (locker)
            {
                if (!client.Connected)
                {
                    return;
                }

                if (floatNumber == 0)                   //Si floatNumber == 0 entonces envia dos ceros a los registros del PLC
                {
                    int[] ceros = new int[2];
                    ceros[0] = 0;
                    ceros[1] = 0;
                    SignedIntWrite(Dreg, ceros);
                    return;

                }


                double fltNum = Math.Abs(floatNumber);  //quitar el signo fltNum
                int x = 0;                              //x representa el exponente total (2 elevado a exp - 127)
                int exp = 0;                            //exp representa el byte formado por los bits E0 -- E7     
                string num1floatBinary = "";            //esta variable representa la palabra formada por los bits desde A0 - A22

                if (fltNum >= 2.0)                      //si fltNum es mayor que 2.0 es necesario reducirla hasta que se encuentre
                {                                       //entre un valor entre 1.0 y 1.9999...
                    while (fltNum > 2.0)
                    {
                        fltNum = fltNum / 2;            //si flt se reduce X aumenta
                        x++;
                    }
                }
                else if (fltNum < 1.0)                  //si fltNum es menor que 1.0 hay que amplificarlo...
                {
                    while (fltNum < 1.0)
                    {
                        fltNum = fltNum * 2;            //si fltNum se reduce x disminuye
                        x--;
                    }
                }
                else if (fltNum >= 1.0 & fltNum < 2.0)  //fltNum está dentro del rango que se necesita x = 0
                {
                    x = 0;
                }
                else
                {
                    x = 0;
                    fltNum = 0;
                }

                fltNum = fltNum - 1;                    // se descuenta 1 a fltNum, ya que el la conversion empieza con 2 elevado a 0

                for (int i = 23; i >= 1; i--)           // aqui se produce la conversión de fltNum a binario
                {
                    double potencia = Math.Pow(2, i - 24);

                    if (potencia <= fltNum)
                    {
                        fltNum = fltNum - potencia;
                        num1floatBinary += "1";
                    }
                    else
                    {
                        num1floatBinary += "0";
                    }
                }

                exp = x + 127;                          // como x es iguala exp - 127, se despeja exp
                string expBinary = Convert.ToString(exp, 2).PadLeft(8, '0');    // y se convierte a binario tambien

                expBinary += num1floatBinary.Substring(0, 7);

                if (floatNumber > 0)
                {
                    expBinary = "0" + expBinary;
                }
                else
                {
                    expBinary = "1" + expBinary;
                }

                num1floatBinary = num1floatBinary.Remove(0, 8);

                int[] floatIntData = new int[2];                                //este es el array en donde se guardaran los datos a enviar al PLC

                floatIntData[0] = Convert.ToInt32(num1floatBinary, 2);         //conversion de binario string a Int
                floatIntData[1] = Convert.ToInt32(expBinary, 2);               //conversion de binario string a Int

                SignedIntWrite(Dreg, floatIntData);                             //se envian los datos al PLC
            }                
        }

        public void SignedDoubleIntWrite(int dReg, Int32 dataValue)
        {
            string dataValuefull = string.Format("{0:X8}", dataValue);
            string dataValue1 = dataValuefull.Substring(0,4);
            string dataValue2 = dataValuefull.Substring(4,4);

            int[] dataValues = new int[2];
            dataValues[0] = Int32.Parse(dataValue2, System.Globalization.NumberStyles.HexNumber);
            dataValues[1] = Int32.Parse(dataValue1, System.Globalization.NumberStyles.HexNumber);

            SignedIntWrite(dReg, dataValues);
        }


        public void SignedIntWrite(int startDReg, int[] dataValues)
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

                foreach (int value in dataValues)
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
