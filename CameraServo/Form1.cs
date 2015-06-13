using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mvIMPACT_NET;
using mvIMPACT_NET.acquire;
using IliasPatsiaourasImageProccessing;
using CameraServo.Common;
using CameraServo.TCPServerTest;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace CameraServo
{
    
    public partial class Form1 : Form
    {
        string settingsFilename;

        string LogFilename;
        static System.IO.FileStream logFileStream;

        DeviceManager devMgr;
        int devIdx;
        Device dev;
        Window win;
        tcpThreadedServer mytcpserver;

        //UDP attributes
        IPEndPoint UDPgroupEP;
        static IPEndPoint UDPreceiveEndPoint;
        static UdpClient udpClient;

        


        public Form1()
        {
            InitializeComponent();

            try
            {
                devMgr = new DeviceManager();
                //fill the drop down menu with all supported camera devices
                if (devMgr.deviceCount() == 0)
                {
                    debug.AppendText("No MATRIX VISION device found!\n");
                }
                else
                {
                    for (uint i = 0; i < devMgr.deviceCount(); i++)
                    {
                        Device tempDev = devMgr.getDevice(i);
                        if (tempDev != null)
                        {
                            //Console.WriteLine("[{0}]: {1}({2}, family: {3})", i, tempDev.serial.read(), tempDev.product.read(), tempDev.family.read());
                            comboBoxDevice.Items.Add(tempDev.product.read());
                        }
                        tempDev.Dispose();
                    }
                }

            }
            catch (ImpactException e)
            {
                MessageBox.Show(e.errorString);
            }

        }

        void Form1_Load(object sender, EventArgs e)
        {           
            winWidth.Value = Properties.Settings.Default.winWidth;
        }
  
        void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Properties.Settings.Default.winWidth = (int)winWidth.Value;
            Properties.Settings.Default.Save();

            //throw new System.NotImplementedException();
            debug.AppendText("Closing Communication\n");
            try { UDPDisconnect(25000); }
            catch { }
            
            try {mytcpserver.Stop();}
            catch { }

        }

        delegate void AppendLineCallback(string text);

        public void AppendLine(string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.debug.InvokeRequired)
            {
                AppendLineCallback d = new AppendLineCallback(AppendLine);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.debug.AppendText(text);
            }
        }

        private void buttonAddRbt_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text != "" && textBoxSign.Text != "" && textBoxPort.Text != "")
            {
                string name = textBoxName.Text;
                double sign = double.Parse(textBoxSign.Text);
                int port = int.Parse(textBoxPort.Text);
                //add robot to the list
                RobotCheckList.Items.Add(new ImageProccessing.Robot(name, sign, port));

                //reset the input textboxes
                textBoxName.Clear();
                textBoxSign.Clear();
                textBoxPort.Clear();
            }
            else
            {
                MessageBox.Show("All fields must be filled");
            }
        }

        private void buttonRemoveRbt_Click(object sender, EventArgs e)
        {
            //if is something selected
            if (RobotCheckList.SelectedIndex != -1)
            {
                //remove this robot from checklist
                RobotCheckList.Items.RemoveAt(RobotCheckList.SelectedIndex);//always this last cause the following change the index num
            }
        }

        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //select device from drop down menu
            devIdx = comboBoxDevice.SelectedIndex;
            dev = devMgr.getDevice((uint)devIdx);
            debug.AppendText(String.Format("Device: {0} selected\n", dev.serial.read()));
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "Start tracking")
            {
                //do START
                //if a device is selected
                if (dev != null)
                {
                    buttonStart.Text = "Stop tracking";
                    buttonStart.BackColor = Color.Red;

                    stat.Clear();
                    win = new Window(new mvIMPACT_NET.Image());
                    win.title = "Press Stop before closing this window";
                    if (fit.Checked)//fit to page selection check
                        win.zoomMode = ZoomingMode.zmFitAspect;
                    //start image proccessing thread
                    imageProcWorker.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Please select Device");
                }
            }
            else
            {
                //do STOP
                buttonStart.Text = "Start tracking";
                buttonStart.BackColor = Color.LimeGreen;

                imageProcWorker.CancelAsync();
                if (checkBoxLogging.Checked)
                {
                    logFileStream.Close();
                }
            }

        }

        private void imageProcWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //debug.Invoke(new Action(() => debug.Clear()));
            debug.Invoke(new Action(() => debug.AppendText("Opening device...\n")));
            try
            {
                dev.open();
            }
            catch (ImpactAcquireException ex)
            {
                // this e.g. might happen if the same device is already opened in another process...
                MessageBox.Show("An error occurred while opening the device " 
                    + dev.serial.read() + "(error code: " + ex.Message + ").");
            }
            

            // establish access to the statistic properties
            Statistics statistics = new Statistics(ref dev);
            // create an interface to the device found
            FunctionInterface fi = new FunctionInterface(ref dev);

            try
            {
                fi.loadSetting(settingsFilename, TStorageFlag.sfFile);
            }
            catch
            {
                MessageBox.Show("Could not load camera settings");
                fi.loadSettingFromDefault();
            }
            
            //can overload with the specific request object. if not just use one of the available
            fi.imageRequestSingle();//fps: 6.95 capture time (s): 0.11075
            fi.imageRequestSingle();//fps: 17.05 capture time (s): 0.111
            //fi.imageRequestSingle();//fps: 17.05 capture time (s): 0.167
            //fi.imageRequestSingle();//fps: 17.05 capture time (s): 0.168

            // run thread loop
            Request request;
            const int timeout_ms = 2000;
            int requestNr = -1;
            int lastRequestNr = -1;
            int cnt = 0;

            //if (RobotList.Items.Count == 0) //needs implementation

            //crating a vector named 'input' that contains the selected robots to feed them to the image proccessing constructor
            ImageProccessing.Robot[] input = new ImageProccessing.Robot[RobotCheckList.CheckedIndices.Count];
            RobotCheckList.CheckedItems.CopyTo(input, 0);

            //initiate imageproccessing
            ImageProccessing imProc = new ImageProccessing(2, input);// This method also load the lookup tables
            imProc.l_dzone = (int)leftDzone.Value;
            imProc.r_dzone = (int)rightDzone.Value;
            imProc.u_dzone = (int)upDzone.Value;
            imProc.d_dzone = (int)lowDzone.Value;
            imProc.threshold = (byte)threshold.Value;
            imProc.tolerance = (double)tolerance.Value;

            int winheight = (int)winHeight.Value;
            int winwidth = (int)winWidth.Value;

            //the LOOP 
            while (!imageProcWorker.CancellationPending)
            {
                // wait for results from the default capture queue blocking method
                requestNr = fi.imageRequestWaitFor(timeout_ms);

                if (fi.isRequestNrValid(requestNr))
                {
                    request = fi.getRequest(requestNr);
                    if (fi.isRequestOK(ref request))
                    {
                        ++cnt;
                        // here we can display some statistical information every 50th image
                        if (cnt % 20 == 0)
                        {
                            stat.Invoke(new Action(() => stat.Text ="\nfps: " + statistics.framesPerSecond.readS() + ", error count: " +
                                statistics.errorCount.readS() + ", capture time: " + statistics.captureTime_s.readS() + "\n"));
                        }
                        // This call is fast, as it uses the request memory to create the IMPACT image. However
                        // the IMPACT image will become invalid as soon as the request buffer is unlocked via
                        // fi.imageRequestUnlock( requestNr );
                        mvIMPACT_NET.Image image = request.getIMPACTImage(TImpactBufferFlag.ibfUseRequestMemory);
                        // This call would be slower as it copies the image data. This image remains valid even when the
                        // request has been unlocked again...
                        // Image image = pRequest.getIMPACTImage();

                        /*********************************************************************************************
                                                        IMAGE PROCCESSING (region)
                         * ******************************************************************************************/

                        #region ImageProccessing
                        //history based (smart) scan
                        if (imProc.fullScanRequired == false)
                        {
                            //update only assigned leds
                            foreach (ImageProccessing.LED ledi in imProc.ledFound.Where(led => led.assigned==true))
                            {
                                imProc.updateLed(image, ledi, winwidth, winheight);
                            }

                            /*
                            //upadate all founded leds
                            for (int i=0; i < imProc.ledFound.Count; i++)
                            {
                                imProc.updateLed(image, imProc.ledFound[i], winwidth, winheight);
                                //if a led lost but fullScanRequired_flag didnt raised this means that is not assigned to any robot,
                                //so we remove it from the list

                                //if is not uptodate
                                if (!imProc.ledFound[i].isUptodate)
                                {
                                    imProc.ledFound.RemoveAt(i);
                                    i--;
                                }
                            }*/

                            imProc.calcLedsRealCoords();
                            //update all robots
                            foreach (ImageProccessing.Robot rob in imProc.rbt)
                            {
                                imProc.updateRobot(rob, statistics.framesPerSecond.read());
                            }                                                       
                            
                        }
                        //full scan                                       
                        else
                        {
                            imProc.scanForLeds(image); //create a list findLed.ledFound with all leds camera can see
                            if (imProc.ledFound.Count >= imProc.ledNum) // chech that there is at least as many led required
                            {
                                imProc.fullScanRequired = false;
                                imProc.calcLedsRealCoords();
                                imProc.ScanForRobots();
                                foreach (ImageProccessing.Robot rob in imProc.rbt)
                                {
                                    imProc.updateRobot(rob, statistics.framesPerSecond.read());
                                }
                                debug.Invoke(new Action(() => debug.AppendText("Full scan found enough leds\n")));
                            }
                            else
                            {
                                debug.Invoke(new Action(() => debug.AppendText(String.Format("full scan can't find enough leds.{0} leds missing\n", imProc.ledNum - imProc.ledFound.Count))));  
                            }
                            if (imProc.fullScanRequired==false)
                                debug.Invoke(new Action(() => debug.AppendText(String.Format("Started smart scan with {0}x{1} window\n", winwidth, winheight))));
                        }
                        #endregion
                        

                        //if someone close the win window without "stop tracking" fisrt the followings crash and run catch
                        try
                        {
                            if (imProc.rbtNum > 0)//if there are robot selected
                            {
                                for (int i = 0; i < imProc.rbtNum; i++) //for the selected robot
                                {
                                    if (imProc.rbt[i].Exist) //if image proccessing has found it draw a circle around it
                                    {
                                        var pt = new mvIMPACT_NET.Point((int)imProc.rbt[i].bigLed.x, (int)imProc.rbt[i].bigLed.y);
                                        image.drawEllipse(pt, 90, 90, (int)(imProc.rbt[i].sign*1.5));
                                    }
                                    //else not needed, every image is new
                                }   
                                UDPBroadcastRobots(imProc.rbt, statistics.captureTime_s.read());

                                if (checkBoxLogging.Checked)
                                {
                                    LogWrite(String.Format("{0}\t{1}\t{2}\t{3}\n", imProc.rbt[0].x, imProc.rbt[0].y, imProc.rbt[0].theta, imProc.rbt[0].calculatedSign));
                                }
                            }

                            win.buffer = image;
                            win.update();                            
                        }
                        catch
                        {
                            //do STOP
                            buttonStart.Invoke(new Action(() => buttonStart.Text = "Start tracking"));
                            buttonStart.Invoke(new Action(() => buttonStart.BackColor = Color.LimeGreen));

                            imageProcWorker.CancelAsync();
                            if (checkBoxLogging.Checked)
                            {
                                logFileStream.Close();
                            }
                        }
                        //image.Dispose();
                    }
                    else
                    {
                        debug.Invoke(new Action(() => debug.AppendText(String.Format("Error: " + request.requestResult.readS() + "\n"))));             
                    }
                    if (fi.isRequestNrValid(lastRequestNr))
                    {
                        // this image has been displayed thus the buffer is no longer needed...
                        fi.imageRequestUnlock(lastRequestNr);
                    }
                    lastRequestNr = requestNr;
                    // send a new image request into the capture queue
                    fi.imageRequestSingle();
                }
                else
                {
                    // this should not happen in this sample, but may happen if you wait for a request without
                    // sending one to the driver before or if a trigger is missing while the device has been 
                    // set up for triggerd acquisition. Please refer to the documentation for reasons for
                    // possible errors if you ever reach this code
                    debug.Invoke(new Action(() => debug.AppendText(String.Format("Acquisition error: {0}.\n", requestNr))));        
                }
                //debug.Invoke(new Action(() => debug.Clear()));
            }//End of the LOOP

            /*
            //prevent display of already freed memory
            win.buffer = new mvIMPACT_NET.Image();
            win.update();
            */

            win.Dispose();
            
            // free the last potential locked request
            if (fi.isRequestNrValid(requestNr))
            {
                fi.imageRequestUnlock(requestNr);
            }
            // clear the request queue
            fi.imageRequestReset(0, 0);
            // extract and unlock all requests that are now returned as 'aborted'
            while ((requestNr = fi.imageRequestWaitFor(0)) >= 0)
            {
                fi.imageRequestUnlock(requestNr);
            }
            dev.close();
        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {

        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TROLLLLLLL");

        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = ".\\";//default path the path of the .exe
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                settingsFilename = openFileDialog1.FileName;
                textBoxSettings.Text = settingsFilename;
            }
        }

        private void buttonBrowseLogfile_Click(object sender, EventArgs e)
        {
            openLogFile.InitialDirectory = ".\\";//default path the path of the .exe
            if (openLogFile.ShowDialog() == DialogResult.OK)
            {
                LogFilename = openLogFile.FileName;
                textBoxLogFile.Text = LogFilename;
            }
            if (System.IO.File.Exists(LogFilename))
            {
                System.IO.File.Delete(LogFilename);
            }  
        }

        private void LogWrite(string str)
        {
            Byte[] data = new UTF8Encoding(true).GetBytes(str);
            // Add some information to the file.
            try
            {
                logFileStream.Write(data, 0, data.Length);
            }
            catch
            {
                AppendLine("log file problem");
            }
        }

        private void checkBoxLogging_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLogging.Checked)//just checked
            {
                if (!System.IO.File.Exists(LogFilename))
                {
                    logFileStream = System.IO.File.Create(LogFilename);
                }                
            }
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            mytcpserver = new tcpThreadedServer(12001);
            mytcpserver.Start();

            UDPConnect(25000);
        }

        delegate void Remote_setup(int exposure,int clock);

        public void remote_settings(int exposure,int clock)
        {
            if (this.debug.InvokeRequired)
            {
                Remote_setup a = new Remote_setup(remote_settings);
                this.Invoke(a, new object[] {exposure, clock});
            }
            else
            {
                if (exposure <= 500)
                {
                    settingsFilename = ".\\Overlapping_overclocked_500exp.xml";
                }
                else if (exposure >= 5000)
                {
                    settingsFilename = ".\\Overlapping_overclocked_5000exp.xml";
                }
                else if (exposure >= 3000)
                {
                    settingsFilename = ".\\Overlapping_overclocked_3000exp.xml";
                }

                textBoxSettings.Text = settingsFilename;      
            }                  
        }

/***********************************UDP************************************/


        private void SendCameraMessage(byte[] _msg)
        {
            Framing frm = new Framing();
            byte[] _newmsg = frm.EscapeBytes(_msg);
            //SendMessageCamera(_newmsg, _newmsg.Length);
        }

        private void UDPConnect(int GroupPort)
        {
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            UDPgroupEP = new IPEndPoint(IPAddress.Broadcast, GroupPort);
            UDPreceiveEndPoint = new IPEndPoint(IPAddress.Any, GroupPort);

            //udpClient.BeginReceive(new AsyncCallback(UDPReceive), UDPreceiveEndPoint);
            debug.Invoke(new Action(() => debug.AppendText(String.Format("UDP server broadcast to {0}\n",GroupPort)))); 
        }

        public static void UDPReceive(IAsyncResult iar)
        {
            byte[] bytes = udpClient.EndReceive(iar, ref UDPreceiveEndPoint);

            // Parse received data

            udpClient.BeginReceive(new AsyncCallback(UDPReceive), UDPreceiveEndPoint);
        }

        private void UDPSend(byte[] _msg)
        {
            udpClient.Send(_msg, _msg.Length, UDPgroupEP);
        }

        private void UDPBroadcastRobotPos(ImageProccessing.Robot[] Rob, double timeDelay)
        {
            List<byte> buffer =new List<byte>();
            buffer.Capacity = 14;

            for (int i = 0; i < 1; i++)
            {
                if (Rob[i].Exist)
                {
                    byte[] x = BitConverter.GetBytes((Int16)(10*Rob[i].x));
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes((Int16)(10*Rob[i].y));
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes((Int16)(10000*Rob[i].theta));
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes((Int16)(10*Rob[i].u));
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes((Int16)(10*Rob[i].v));
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes((Int16)(10000*Rob[i].omega));
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes((Int16)(1000*timeDelay));
                    buffer.AddRange(delay);
                }
                else
                {
                    Int16 nan = 999;
                    
                    byte[] x = BitConverter.GetBytes(nan);
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes(nan);
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes(nan);
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes(nan);
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes(nan);
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes(nan);
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes(timeDelay);
                    buffer.AddRange(delay);
                }
                byte[] _msg = new byte[buffer.Count];
                for (int j = 0; j < buffer.Count; j++)
                {
                    _msg[j] = buffer[j];
                }

                udpClient.Send(_msg, _msg.Length, UDPgroupEP);
            }

            
        }

        private void UDPBroadcastStream(ImageProccessing.Robot[] Rob, double timeDelay)
        {
            List<byte> buffer = new List<byte>();
            buffer.Capacity = (4 * 8 * Rob.Length) + 2;
            //message delimiter
            Int32 msgDelimiter = 0x7E;
            byte[] msgDelim = BitConverter.GetBytes(msgDelimiter);
            buffer.AddRange(msgDelim); 

            for (int i = 0; i < Rob.Length; i++)
            {
                //robot delimiter
                Int32 robDelimiter = 0x7E;
                byte[] robDelim = BitConverter.GetBytes(robDelimiter);
                buffer.AddRange(robDelim); 

                if (Rob[i].Exist)
                {
                    //robot id
                    byte[] id = BitConverter.GetBytes((Rob[i].ID));
                    buffer.AddRange(id); 
                    byte[] x = BitConverter.GetBytes((Int32)(10*Rob[i].x));
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes((Int32)(10*Rob[i].y));
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes((Int32)(1000*Rob[i].theta));
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes((Int32)(10*Rob[i].u));
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes((Int32)(10*Rob[i].v));
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes((Int32)(1000*Rob[i].omega));
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes((Int32)(1000*timeDelay));
                    buffer.AddRange(delay);
                }
                else
                {
                    //robot id
                    byte[] id = BitConverter.GetBytes((Rob[i].ID));
                    buffer.AddRange(id); 
                    //didnt found
                    Int32 nan = -9999;
                    byte[] x = BitConverter.GetBytes(nan);
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes(nan);
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes(nan);
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes(nan);
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes(nan);
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes(nan);
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes(timeDelay);
                    buffer.AddRange(delay);
                }
                //robot delimiter
                buffer.AddRange(robDelim);                
            }
            //message delimiter
            buffer.AddRange(msgDelim);

            byte[] _msg = new byte[buffer.Count];
            for (int j = 0; j < buffer.Count; j++)
            {
                _msg[j] = buffer[j];
            }

            udpClient.Send(_msg, _msg.Length, UDPgroupEP);
        }

        private void UDPBroadcastRobots(ImageProccessing.Robot[] Rob, double timeDelay)
        {           
            for (int i = 0; i < Rob.Length; i++)
            {
                //initialize stream
                List<byte> buffer = new List<byte>();
                buffer.Capacity = 17;

                //message starter delimiter
                byte delimiter = (byte)0x7E;
                buffer.Add(delimiter);

                //robot ID
                byte robID = (byte)Rob[i].ID;
                buffer.Add(robID);

                if (Rob[i].Exist)
                {
                    byte[] x = BitConverter.GetBytes((Int16)(10 * Rob[i].x));
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes((Int16)(10 * Rob[i].y));
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes((Int16)(10000 * Rob[i].theta));
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes((Int16)(10 * Rob[i].u));
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes((Int16)(10 * Rob[i].v));
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes((Int16)(10000 * Rob[i].omega));
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes((Int16)(1000 * timeDelay));
                    buffer.AddRange(delay);
                }
                else
                {
                    Int16 nan = -999;

                    byte[] x = BitConverter.GetBytes(nan);
                    buffer.AddRange(x);
                    byte[] y = BitConverter.GetBytes(nan);
                    buffer.AddRange(y);
                    byte[] theta = BitConverter.GetBytes(nan);
                    buffer.AddRange(theta);
                    byte[] u = BitConverter.GetBytes(nan);
                    buffer.AddRange(u);
                    byte[] v = BitConverter.GetBytes(nan);
                    buffer.AddRange(v);
                    byte[] omega = BitConverter.GetBytes(nan);
                    buffer.AddRange(omega);
                    byte[] delay = BitConverter.GetBytes(timeDelay);
                    buffer.AddRange(delay);
                }

                //message end delimiter
                buffer.Add(delimiter);

                //convert to byte stream
                byte[] _msg = new byte[buffer.Count];
                for (int j = 0; j < buffer.Count; j++)
                {
                    _msg[j] = buffer[j];
                }

                //send byte stream
                udpClient.Send(_msg, _msg.Length, UDPgroupEP);
                Debug.WriteLine("Robot coords sended");
            }
        }

        private void UDPDisconnect(int GroupPort)
        {
            udpClient.Close();
        }
    }
}
