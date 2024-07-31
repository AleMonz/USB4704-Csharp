using Automation.BDaq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System;
using System.Collections.Generic;

namespace USB4704Test
{
    internal class USB4704
    {

        //List
        /// <summary>
        /// Array that contains the Inputs and Outputs states. Here is all the data we need from the device
        /// <para>Digital Outputs Positions: 0-7</para>
        /// <para>Digital Inputs Positions: 8-15</para>
        /// </summary>
        public static int[] DataList = new int[16];

        //Threads
        public static Thread ReadInputs;
        public static Thread WriteOutputs;

        //Flags for the Threads
        /// <summary>
        /// Flag for the ReadingInputs Thread
        /// </summary>
        private static bool InputsThreadState = false;

        /// <summary>
        /// Flag for the WriteOutputs Thread
        /// </summary>
        private static bool OutputsThreadState = false;

        //Functions
        #region Public Functions

        /// <summary>
        /// Starts the Thread that reads the Inputs from the USB device
        /// </summary>
        public static void StartReadingInputs()
        {
            if (!InputsThreadState)
            {
                InputsThreadState = true;
                ReadInputs = new Thread(ReadDigitalInputs);
                ReadInputs.Start();
            }
        }

        /// <summary>
        /// Stops the Thread that reads the Inputs from the USB device
        /// </summary>
        public static void StopReadingInputs()
        {
            if (InputsThreadState)
            {
                InputsThreadState = false;
                ReadInputs.Abort();
            }
        }


        /// <summary>
        /// Starts the Thread that writes the Outputs to the USB device
        /// </summary>
        public static void StartWritingOutputs()
        {
            if (!OutputsThreadState)
            {
                OutputsThreadState = true;
                WriteOutputs = new Thread(WriteDigitalOutputs);
                WriteOutputs.Start();
            }
        }

        /// <summary>
        /// Stops the Thread that writes the Outputs to the USB device
        /// </summary>
        public static void StopWritingOutputs()
        {
            if (OutputsThreadState)
            {
                OutputsThreadState = false;
                WriteOutputs.Abort();
            }
        }

        #endregion Public Functions

        #region Private Functions

        /// <summary>
        /// Reads the 0s and 1s from the USB-4704 Digital Inputs
        /// </summary>
        private static void ReadDigitalInputs()
        {
            try
            {
                InstantDiCtrl instantDiCtrl1 = new InstantDiCtrl();

                //Selects the USB device
                //In case of having more than 1 device, change this part of the code 
                instantDiCtrl1.SelectedDevice = new DeviceInformation(0);

                bool DI0 = true;
                bool DI1 = true;
                bool DI2 = true;
                bool DI3 = true;
                bool DI4 = true;
                bool DI5 = true;
                bool DI6 = true;
                bool DI7 = true;

                byte data0;

                while (InputsThreadState)
                {
                    // Reads the 8 Digital Inputs as a byte 
                    instantDiCtrl1.Read(0, out data0);

                    //Conversion from byte to hex
                    int numdec = Convert.ToInt32(data0);
                    string numhex = numdec.ToString("X");
                    int numhex1 = Convert.ToInt32(numhex, 16);

                    //Conversion from hex to boolean variables
                    DI0 = (numhex1 & 0x01) != 0;
                    DI1 = (numhex1 & 0x02) != 0;
                    DI2 = (numhex1 & 0x04) != 0;
                    DI3 = (numhex1 & 0x08) != 0;
                    DI4 = (numhex1 & 0x10) != 0;
                    DI5 = (numhex1 & 0x20) != 0;
                    DI6 = (numhex1 & 0x40) != 0;
                    DI7 = (numhex1 & 0x80) != 0;

                    //Update the DataList with the Digital Inputs States
                    if (DI0 == true) { DataList[8] = 1; } else { DataList[8] = 0; }
                    if (DI1 == true) { DataList[9] = 1; } else { DataList[9] = 0; }
                    if (DI2 == true) { DataList[10] = 1; } else { DataList[10] = 0; }
                    if (DI3 == true) { DataList[11] = 1; } else { DataList[11] = 0; }
                    if (DI4 == true) { DataList[12] = 1; } else { DataList[12] = 0; }
                    if (DI5 == true) { DataList[13] = 1; } else { DataList[13] = 0; }
                    if (DI6 == true) { DataList[14] = 1; } else { DataList[14] = 0; }
                    if (DI7 == true) { DataList[15] = 1; } else { DataList[15] = 0; }

                }

            }
            catch (Exception e) 
            { 
                Debug.Print(e.Message); 
                StopReadingInputs();
            }

        }


        /// <summary>
        /// Writes 0s or 1s to the USB-4704 Digital Outputs
        /// </summary>
        private static void WriteDigitalOutputs() 
        {
            try
            {
                InstantDoCtrl instantDoCtrl1 = new InstantDoCtrl();

                //Selects the USB device
                //In case of having more than 1 device, change this part of the code 
                instantDoCtrl1.SelectedDevice = new DeviceInformation(0);

                bool DO0 = true;
                bool DO1 = true;
                bool DO2 = true;
                bool DO3 = true;
                bool DO4 = true;
                bool DO5 = true;
                bool DO6 = true;
                bool DO7 = true;


                while (OutputsThreadState)
                {
                    //Reads the DataList values for the DigitalOutputs
                    if (Convert.ToInt32(DataList[0]) == 1) { DO0 = true; } else if (Convert.ToInt32(DataList[0]) == 0) { DO0 = false; }
                    if (Convert.ToInt32(DataList[1]) == 1) { DO1 = true; } else if (Convert.ToInt32(DataList[1]) == 0) { DO1 = false; }
                    if (Convert.ToInt32(DataList[2]) == 1) { DO2 = true; } else if (Convert.ToInt32(DataList[2]) == 0) { DO2 = false; }
                    if (Convert.ToInt32(DataList[3]) == 1) { DO3 = true; } else if (Convert.ToInt32(DataList[3]) == 0) { DO3 = false; }
                    if (Convert.ToInt32(DataList[4]) == 1) { DO4 = true; } else if (Convert.ToInt32(DataList[4]) == 0) { DO4 = false; }
                    if (Convert.ToInt32(DataList[5]) == 1) { DO5 = true; } else if (Convert.ToInt32(DataList[5]) == 0) { DO5 = false; }
                    if (Convert.ToInt32(DataList[6]) == 1) { DO6 = true; } else if (Convert.ToInt32(DataList[6]) == 0) { DO6 = false; }
                    if (Convert.ToInt32(DataList[7]) == 1) { DO7 = true; } else if (Convert.ToInt32(DataList[7]) == 0) { DO7 = false; }

                    byte HexValue = 0;

                    //Get the Hexadecimal value that represents the states of the Digital Outputs that we want to write
                    if (DO0) HexValue |= 0x01;
                    if (DO1) HexValue |= 0x02;
                    if (DO2) HexValue |= 0x04;
                    if (DO3) HexValue |= 0x08;
                    if (DO4) HexValue |= 0x10;
                    if (DO5) HexValue |= 0x20;
                    if (DO6) HexValue |= 0x40;
                    if (DO7) HexValue |= 0x80;

                    //Update the state of the Device Digital Outputs
                    instantDoCtrl1.Write(0, HexValue);

                }
            }
            catch (Exception e) 
            { 
                Debug.Print(e.Message); 
                StopWritingOutputs();
            }

        }

        #endregion Private Functions

    }
}
