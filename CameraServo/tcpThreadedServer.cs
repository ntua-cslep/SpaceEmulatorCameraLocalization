using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using CameraServo.Common;
using CameraServo;
using System.Diagnostics;


namespace CameraServo.TCPServerTest
{
    class tcpThreadedServer
    {
        #region Attributes

        private TcpListener tcpListener;
        private Thread listenThread;
        private bool bStarted;
        private int iPort;
        private List<Thread> clientThreads = new List<Thread>();
        private List<TcpClient> clientlist = new List<TcpClient>();
        NetworkStream clientStream = null;
        private byte[] ImgData = new byte[1600 * 1200];

        public Int32 exposure;
        public Int32 clock;
        
        #endregion

        #region Constructor/Destructor

        public tcpThreadedServer(int _iPort)
        {
            iPort = _iPort;
            bStarted = false;
        }

        ~tcpThreadedServer()
        {
            if (bStarted)
            {
                this.tcpListener.Stop();
                this.listenThread.Abort();
                bStarted = false;
            }
        }

        #endregion

        #region Methods

        public void Start()
        {
            if (!bStarted)
            {
                this.tcpListener = new TcpListener(IPAddress.Any, iPort);
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.Start();
                bStarted = true;
                CameraServo.Program.form1.AppendLine("Server listening on port " + iPort.ToString() + " ...\n");
            }
        }

        public void Stop()
        {
            if (bStarted)
            {
                foreach (TcpClient thcl in clientlist)
                {
                    if(thcl.Connected)
                        thcl.Close();
                }
                clientlist.Clear();
                foreach (Thread thr in clientThreads)
                    thr.Abort();
                    clientThreads.Clear();
                this.tcpListener.Stop();
                this.listenThread.Abort();
                
                bStarted = false;
                //CameraSimulator.Program.form1.AppendLine("Server stoppped.\n");
            }
        }

        public PhysicalAddress GetDestinationMacAddress(System.Net.IPAddress address, System.Net.IPAddress sourceAddress)
        {

            byte[] macAddrBytes = GetDestinationMacAddressBytes(address, sourceAddress);

            PhysicalAddress macAddress = new PhysicalAddress(macAddrBytes);

            return macAddress;

        }

        private static Int32 IpAddressAsInt32(System.Net.IPAddress address)
        {

            byte[] ipAddrBytes = address.GetAddressBytes();

            Int32 addrInt = BitConverter.ToInt32(ipAddrBytes, 0);

            return addrInt;

        }

        public byte[] GetDestinationMacAddressBytes(System.Net.IPAddress address, System.Net.IPAddress sourceAddress)
        {

            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {

                throw new ArgumentException("Only supports IPv4 Addresses.");

            }

            Int32 addrInt = IpAddressAsInt32(address);

            Int32 srcAddrInt = IpAddressAsInt32(sourceAddress);

            //

            const int MacAddressLength = 6;// 48bits

            byte[] macAddress = new byte[MacAddressLength];

            Int32 macAddrLen = macAddress.Length;

            Int32 ret = NativeMethods.SendArp(addrInt, srcAddrInt, macAddress, ref macAddrLen);

            if (ret != 0)
            {

                throw new System.ComponentModel.Win32Exception(ret);

            }

            return macAddress;

        }
        public void SendMessage(byte[] msg, int mlen)
        {
            clientStream.Write(msg, 0, mlen);
        }
        private void HandleClientComm(object client)
        {
            //double tRate;
            //DateTime onDcnx,onCnx;
            //TimeSpan tDiff;

            //onCnx = DateTime.Now;
            TcpClient tcpClient = (TcpClient)client;
            clientStream = tcpClient.GetStream();

            clientlist.Add(tcpClient);

            //clientStream.Write(Encoding.ASCII.GetBytes(testphrase), 0, testphrase.Length);

            byte[] message = new byte[4096];
            int bytesRead;
            long totalbytes = 0;

            while (bStarted)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);

                }
                catch
                {
                    //a socket error has occured
                    break;
                }
                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                if (bytesRead > 0 && bStarted)
                {
                    totalbytes += bytesRead;
                    
                    // Message Parsing Here
                    Framing frm = new Framing();
                    byte[] _newmsg = frm.UnEscapeBytes(message.SubArray(0,bytesRead));
                    CameraMessage cmr_msg = new CameraMessage(_newmsg);

                    switch (cmr_msg.GetMessageType())
                    {
                        case messageType.COMMAND:
                            switch (cmr_msg.GetCommandID())
                            {
                                case 0x02 ://camera settings recieved
                                    byte[] ackmsg = new byte[cmr_msg.GetPayload().Length - 8 + 2]; // Disregard int32 values
                                    ackmsg[0] = ackmsg[ackmsg.Length - 1] = Globals.SEPARATOR;
                                    Array.Copy(cmr_msg.GetPayload(), 0, ackmsg, 1, cmr_msg.GetPayload().Length - 8);
                                    ackmsg[3] = 0x43;
                                    Framing frm2 = new Framing();
                                    byte[] _newmsg2 = frm.EscapeBytes(ackmsg);
                                    clientStream.Write(_newmsg2, 0, _newmsg2.Length);

                                    Int32[] int32vals = cmr_msg.GetInt32Values();

                                    exposure = int32vals[0];
                                    clock = int32vals[1];

                                    Program.form1.remote_settings(exposure, clock);

                                    break;

                                case 0x03://camera settings requested
                                    byte[] ackmsg2 = new byte[cmr_msg.GetPayload().Length + 2]; // Disregard int32 values
                                    ackmsg2[0] = ackmsg2[ackmsg2.Length - 1] = Globals.SEPARATOR;
                                    Array.Copy(cmr_msg.GetPayload(), 0, ackmsg2, 1, cmr_msg.GetPayload().Length);
                                    ackmsg2[3] = 0x43;
                                    Framing frm3 = new Framing();
                                    byte[] _newmsg3 = frm.EscapeBytes(ackmsg2);
                                    clientStream.Write(_newmsg3, 0, _newmsg3.Length);
                                    //response

                                   

                                    break;
                            }
                            break;
                    }
                }

            }
            tcpClient.Close();
        }

        private void ListenForClients()
        {
            PhysicalAddress macAddress = null;
            try
            {
                this.tcpListener.Start();

                while (true)
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    client.ReceiveBufferSize = 4096;
                    client.SendBufferSize = 4096;
                    client.ReceiveTimeout = 240000;

                    IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    try
                    {
                        macAddress = GetDestinationMacAddress(remoteIpEndPoint.Address, System.Net.IPAddress.Any);
                    }
                    catch (Exception ex)
                    {
                    }
                    //CameraSimulator.Program.form1.AppendLine("Client " + client.Client.RemoteEndPoint.ToString() + " - " + macAddress.ToString() + " connected - " + DateTime.Now.ToString() + "\n");

                    //create a thread to handle communication 
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThreads.Add(clientThread);
                    clientThread.Start(client);
                }
            }
            catch (Exception e)
            {

            }
        }

        #endregion
    }
    static class NativeMethods
    {

        /// <summary>

        /// Sends an ARP request to obtain the physical address that corresponds

        /// to the specified destination IP address.

        /// </summary>

        /// -

        /// <param name="destIpAddress">Destination IP address, in the form of

        /// a <see cref="T:System.Int32"/>. The ARP request attempts to obtain

        /// the physical address that corresponds to this IP address.

        /// </param>

        /// <param name="srcIpAddress">IP address of the sender, in the form of

        /// a <see cref="T:System.Int32"/>. This parameter is optional. The caller

        /// may specify zero for the parameter.

        /// </param>

        /// <param name="macAddress">

        /// </param>

        /// <param name="macAddressLength">On input, specifies the maximum buffer

        /// size the user has set aside at pMacAddr to receive the MAC address,

        /// in bytes. On output, specifies the number of bytes written to

        /// pMacAddr.</param>

        /// -

        /// <returns>If the function succeeds, the return value is NO_ERROR.

        /// If the function fails, use FormatMessage to obtain the message string

        /// for the returned error.

        /// </returns>

        [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]

        internal extern static Int32 SendArp(Int32 destIpAddress, Int32 srcIpAddress,

        byte[] macAddress, ref Int32 macAddressLength);

    }
}
