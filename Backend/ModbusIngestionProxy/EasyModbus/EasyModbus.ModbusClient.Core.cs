/*
Copyright (c) 2018-2020 Rossmann-Engineering
Permission is hereby granted, free of charge, 
to any person obtaining a copy of this software
and associated documentation files (the "Software"),
to deal in the Software without restriction, 
including without limitation the rights to use, 
copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit 
persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission 
notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO.Ports;
using System.Net;

namespace EasyModbus
{

    public partial class ModbusClient
    { 


        #region Class Variables
        private bool debug = false;
        private TcpClient tcpClient;
        private bool udpFlag = false;
        private string ipAddress = "127.0.0.1";
        private int port = 502;
        private int connectTimeout = 1000;
        private bool connected;
        public bool IsConnected => connected;

        private byte[] protocolIdentifier = new byte[2];
        private byte[] crc = new byte[2];
        private byte[] length = new byte[2];
        private byte unitIdentifier = 0x01;
        private byte functionCode;
        private byte[] startingAddress = new byte[2];
        private byte[] quantity = new byte[2];

        public byte[] sendData;
        public byte[] receiveData;
        private byte[] readBuffer = new byte[256];
        private int portOut;

        public int NumberOfRetries { get; set; } = 3;

        private uint transactionIdentifierInternal = 0;
        private byte[] transactionIdentifier = new byte[2];

        public delegate void ConnectedChangedHandler(object sender);
        public event ConnectedChangedHandler ConnectedChanged;

        public delegate void ReceiveDataChangedHandler(object sender);
        public event ReceiveDataChangedHandler ReceiveDataChanged;

        public delegate void SendDataChangedHandler(object sender);
        public event SendDataChangedHandler SendDataChanged;

        NetworkStream stream;
        #endregion


        /// <summary>
        /// Establish connection to Master device in case of Modbus TCP.
        /// </summary>
        public void Connect(string ipAddress, int port)
        {
            if (!udpFlag)
            {
                if (debug) StoreLogData.Instance.Store("Open TCP-Socket, IP-Address: " + ipAddress + ", Port: " + port, System.DateTime.Now);
                tcpClient = new TcpClient();
                var result = tcpClient.BeginConnect(ipAddress, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(connectTimeout);
                if (!success)
                {
                    throw new EasyModbus.Exceptions.ConnectionException("connection timed out");
                }
                tcpClient.EndConnect(result);

                //tcpClient = new TcpClient(ipAddress, port);
                stream = tcpClient.GetStream();
                stream.ReadTimeout = connectTimeout;
                connected = true;
            }
            else
            {
                tcpClient = new TcpClient();
                connected = true;
            }

            if (ConnectedChanged != null)
                ConnectedChanged(this);
        }


        /// <summary>
        /// Close connection to Master Device.
        /// </summary>
        public void Disconnect()
        {
            if (debug) StoreLogData.Instance.Store("Disconnect", System.DateTime.Now);
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
            connected = false;
            if (ConnectedChanged != null)
                ConnectedChanged(this);

        }



        /// <summary>
        /// Read Input Registers from Master device (FC4).
        /// </summary>
        /// <param name="startingAddress">First input register to be read</param>
        /// <param name="quantity">Number of input registers to be read</param>
        /// <returns>Int Array which contains the input registers</returns>
        public int[] ReadInputRegisters(int startingAddress, int quantity)
        {

            if (debug) StoreLogData.Instance.Store("FC4 (Read Input Registers from Master device), StartingAddress: " + startingAddress + ", Quantity: " + quantity, System.DateTime.Now);
            transactionIdentifierInternal++;
            if (tcpClient == null & !udpFlag)
            {
                if (debug) StoreLogData.Instance.Store("ConnectionException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ConnectionException("connection error");
            }
            if (startingAddress > 65535 | quantity > 125)
            {
                if (debug) StoreLogData.Instance.Store("ArgumentException Throwed", System.DateTime.Now);
                throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 125");
            }
            int[] response;
            this.transactionIdentifier = BitConverter.GetBytes((uint)transactionIdentifierInternal);
            this.protocolIdentifier = BitConverter.GetBytes((int)0x0000);
            this.length = BitConverter.GetBytes((int)0x0006);
            this.functionCode = 0x04;
            this.startingAddress = BitConverter.GetBytes(startingAddress);
            this.quantity = BitConverter.GetBytes(quantity);
            Byte[] data = new byte[]{   this.transactionIdentifier[1],
                            this.transactionIdentifier[0],
                            this.protocolIdentifier[1],
                            this.protocolIdentifier[0],
                            this.length[1],
                            this.length[0],
                            this.unitIdentifier,
                            this.functionCode,
                            this.startingAddress[1],
                            this.startingAddress[0],
                            this.quantity[1],
                            this.quantity[0],
                            this.crc[0],
                            this.crc[1]
            };
            crc = BitConverter.GetBytes(calculateCRC(data, 6, 6));
            data[12] = crc[0];
            data[13] = crc[1];
            if (tcpClient.Client.Connected | udpFlag)
            {
                if (udpFlag)
                {
                    UdpClient udpClient = new UdpClient();
                    IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(ipAddress), port);
                    udpClient.Send(data, data.Length - 2, endPoint);
                    portOut = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
                    udpClient.Client.ReceiveTimeout = 5000;
                    endPoint = new IPEndPoint(System.Net.IPAddress.Parse(ipAddress), portOut);
                    data = udpClient.Receive(ref endPoint);
                }
                else
                {
                    stream.Write(data, 0, data.Length - 2);
                    if (debug)
                    {
                        byte[] debugData = new byte[data.Length - 2];
                        Array.Copy(data, 0, debugData, 0, data.Length - 2);
                        if (debug) StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(debugData), System.DateTime.Now);
                    }
                    if (SendDataChanged != null)
                    {
                        sendData = new byte[data.Length - 2];
                        Array.Copy(data, 0, sendData, 0, data.Length - 2);
                        SendDataChanged(this);
                    }
                    data = new Byte[2100];
                    int NumberOfBytes = stream.Read(data, 0, data.Length);
                    if (ReceiveDataChanged != null)
                    {
                        receiveData = new byte[NumberOfBytes];
                        Array.Copy(data, 0, receiveData, 0, NumberOfBytes);
                        if (debug) StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(receiveData), System.DateTime.Now);
                        ReceiveDataChanged(this);
                    }

                }
            }
            if (data[7] == 0x84 & data[8] == 0x01)
            {
                if (debug) StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.FunctionCodeNotSupportedException("Function code not supported by master");
            }
            if (data[7] == 0x84 & data[8] == 0x02)
            {
                if (debug) StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
            }
            if (data[7] == 0x84 & data[8] == 0x03)
            {
                if (debug) StoreLogData.Instance.Store("QuantityInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.QuantityInvalidException("quantity invalid");
            }
            if (data[7] == 0x84 & data[8] == 0x04)
            {
                if (debug) StoreLogData.Instance.Store("ModbusException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ModbusException("error reading");
            }
            response = new int[quantity];
            for (int i = 0; i < quantity; i++)
            {
                byte lowByte;
                byte highByte;
                highByte = data[9 + i * 2];
                lowByte = data[9 + i * 2 + 1];

                data[9 + i * 2] = lowByte;
                data[9 + i * 2 + 1] = highByte;

                response[i] = BitConverter.ToInt16(data, (9 + i * 2));
            }
            return (response);
        }

        /// <summary>
        /// Read Discrete Inputs from Server device (FC2).
        /// </summary>
        /// <param name="startingAddress">First discrete input to read</param>
        /// <param name="quantity">Number of discrete Inputs to read</param>
        /// <returns>Boolean Array which contains the discrete Inputs</returns>
        public bool[] ReadDiscreteInputs(int startingAddress, int quantity)
        {
            if (debug) StoreLogData.Instance.Store("FC2 (Read Discrete Inputs from Master device), StartingAddress: " + startingAddress + ", Quantity: " + quantity, System.DateTime.Now);
            transactionIdentifierInternal++;
            
            if (tcpClient == null & !udpFlag)
            {
                if (debug) StoreLogData.Instance.Store("ConnectionException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ConnectionException("connection error");
            }
            if (startingAddress > 65535 | quantity > 2000)
            {
                if (debug) StoreLogData.Instance.Store("ArgumentException Throwed", System.DateTime.Now);
                throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 2000");
            }

            // Create Request
            ApplicationDataUnit request = new ApplicationDataUnit(2);
            request.QuantityRead = (ushort)quantity;
            request.StartingAddressRead = (ushort)startingAddress;
            request.TransactionIdentifier = (ushort)transactionIdentifierInternal;


            ApplicationDataUnit response = new ApplicationDataUnit(2);
            response.QuantityRead = (ushort)quantity;

            byte[] data = new byte[255];
            if (tcpClient.Client.Connected | udpFlag)
            {
                if (udpFlag)
                {
                    UdpClient udpClient = new UdpClient();
                    System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipAddress), port);
                    udpClient.Send(data, data.Length - 2, endPoint);
                    portOut = ((System.Net.IPEndPoint)udpClient.Client.LocalEndPoint).Port;
                    udpClient.Client.ReceiveTimeout = 5000;
                    endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipAddress), portOut);
                    data = udpClient.Receive(ref endPoint);
                }
                else
                {
                    stream.Write(request.Payload, 0, request.Payload.Length - 2);
                    if (debug)
                    {
                        byte[] debugData = new byte[data.Length - 2];
                        Array.Copy(data, 0, debugData, 0, data.Length - 2);
                        if (debug) StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(debugData), System.DateTime.Now);
                    }
                    if (SendDataChanged != null)
                    {
                        sendData = new byte[data.Length - 2];
                        Array.Copy(data, 0, sendData, 0, data.Length - 2);
                        SendDataChanged(this);
                    }
                    data = new Byte[255];
                    int NumberOfBytes = stream.Read(response.Payload, 0, response.Payload.Length);
                    if (ReceiveDataChanged != null)
                    {
                        receiveData = new byte[NumberOfBytes];
                        Array.Copy(data, 0, receiveData, 0, NumberOfBytes);
                        if (debug) StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(receiveData), System.DateTime.Now);
                        ReceiveDataChanged(this);
                    }
                }
            }
            if (data[7] == 0x82 & data[8] == 0x01)
            {
                if (debug) StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.FunctionCodeNotSupportedException("Function code not supported by master");
            }
            if (data[7] == 0x82 & data[8] == 0x02)
            {
                if (debug) StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
            }
            if (data[7] == 0x82 & data[8] == 0x03)
            {
                if (debug) StoreLogData.Instance.Store("QuantityInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.QuantityInvalidException("quantity invalid");
            }
            if (data[7] == 0x82 & data[8] == 0x04)
            {
                if (debug) StoreLogData.Instance.Store("ModbusException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ModbusException("error reading");
            }

            return response.RegisterDataBool;
        }

        /// <summary>
        /// Read Holding Registers from Master device (FC3).
        /// </summary>
        /// <param name="startingAddress">First holding register to be read</param>
        /// <param name="quantity">Number of holding registers to be read</param>
        /// <returns>Int Array which contains the holding registers</returns>
        public int[] ReadHoldingRegisters(int startingAddress, int quantity)
        {
            if (debug) StoreLogData.Instance.Store("FC3 (Read Holding Registers from Master device), StartingAddress: " + startingAddress + ", Quantity: " + quantity, System.DateTime.Now);
            transactionIdentifierInternal++;
          
            if (tcpClient == null & !udpFlag)
            {
                if (debug) StoreLogData.Instance.Store("ConnectionException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ConnectionException("connection error");
            }
            if (startingAddress > 65535 | quantity > 125)
            {
                if (debug) StoreLogData.Instance.Store("ArgumentException Throwed", System.DateTime.Now);
                throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 125");
            }
            int[] response;
            this.transactionIdentifier = BitConverter.GetBytes((uint)transactionIdentifierInternal);
            this.protocolIdentifier = BitConverter.GetBytes((int)0x0000);
            this.length = BitConverter.GetBytes((int)0x0006);
            this.functionCode = 0x03;
            this.startingAddress = BitConverter.GetBytes(startingAddress);
            this.quantity = BitConverter.GetBytes(quantity);
            Byte[] data = new byte[]{   this.transactionIdentifier[1],
                            this.transactionIdentifier[0],
                            this.protocolIdentifier[1],
                            this.protocolIdentifier[0],
                            this.length[1],
                            this.length[0],
                            this.unitIdentifier,
                            this.functionCode,
                            this.startingAddress[1],
                            this.startingAddress[0],
                            this.quantity[1],
                            this.quantity[0],
                            this.crc[0],
                            this.crc[1]
            };
            crc = BitConverter.GetBytes(calculateCRC(data, 6, 6));
            data[12] = crc[0];
            data[13] = crc[1];
            if (tcpClient.Client.Connected | udpFlag)
            {
                if (udpFlag)
                {
                    UdpClient udpClient = new UdpClient();
                    IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(ipAddress), port);
                    udpClient.Send(data, data.Length - 2, endPoint);
                    portOut = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
                    udpClient.Client.ReceiveTimeout = 5000;
                    endPoint = new IPEndPoint(System.Net.IPAddress.Parse(ipAddress), portOut);
                    data = udpClient.Receive(ref endPoint);
                }
                else
                {
                    stream.Write(data, 0, data.Length - 2);
                    if (debug)
                    {
                        byte[] debugData = new byte[data.Length - 2];
                        Array.Copy(data, 0, debugData, 0, data.Length - 2);
                        if (debug) StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(debugData), System.DateTime.Now);
                    }
                    if (SendDataChanged != null)
                    {
                        sendData = new byte[data.Length - 2];
                        Array.Copy(data, 0, sendData, 0, data.Length - 2);
                        SendDataChanged(this);

                    }
                    data = new Byte[256];
                    int NumberOfBytes = stream.Read(data, 0, data.Length);
                    if (ReceiveDataChanged != null)
                    {
                        receiveData = new byte[NumberOfBytes];
                        Array.Copy(data, 0, receiveData, 0, NumberOfBytes);
                        if (debug) StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(receiveData), System.DateTime.Now);
                        ReceiveDataChanged(this);
                    }
                }
            }
            if (data[7] == 0x83 & data[8] == 0x01)
            {
                if (debug) StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.FunctionCodeNotSupportedException("Function code not supported by master");
            }
            if (data[7] == 0x83 & data[8] == 0x02)
            {
                if (debug) StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
            }
            if (data[7] == 0x83 & data[8] == 0x03)
            {
                if (debug) StoreLogData.Instance.Store("QuantityInvalidException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.QuantityInvalidException("quantity invalid");
            }
            if (data[7] == 0x83 & data[8] == 0x04)
            {
                if (debug) StoreLogData.Instance.Store("ModbusException Throwed", System.DateTime.Now);
                throw new EasyModbus.Exceptions.ModbusException("error reading");
            }
            response = new int[quantity];
            for (int i = 0; i < quantity; i++)
            {
                byte lowByte;
                byte highByte;
                highByte = data[9 + i * 2];
                lowByte = data[9 + i * 2 + 1];

                data[9 + i * 2] = lowByte;
                data[9 + i * 2 + 1] = highByte;

                response[i] = BitConverter.ToInt16(data, (9 + i * 2));
            }
            return (response);
        }



    }
}
