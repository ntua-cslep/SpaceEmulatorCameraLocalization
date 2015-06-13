using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraServo.Common;

namespace CameraServo
{
    class CameraMessage
    {
        #region Attributes

        private byte[] payload;
        private messageType type;
        private UInt16 uniqueID;
        private byte errorID;
        private byte commandID;
        private byte feedbackID;
        private Int32[] valuesInt32;
        private byte [] valuesByte;
        private byte[] ImgRxData;
        
        #endregion

        #region Methods
        
        public CameraMessage(byte[] _stream)
        {
            if (_stream.Length >=5 &&_stream[0] == Globals.SEPARATOR && _stream[_stream.Length-1] == Globals.SEPARATOR)
            {
                if (_stream[3] >= 0x01 && _stream[3] <= 0x16)
                {
                    type = messageType.COMMAND;
                    commandID = _stream[3];
                }
                if (_stream[3] >= 0x20 && _stream[3] <= 0x32)
                {
                    type = messageType.FEEDBACK;
                    feedbackID = _stream[3];
                }
                if (_stream[3] == 0x43)
                    type = messageType.ACK;
                if (_stream[3] == 0x44)
                    type = messageType.ERROR;

                payload = new byte[_stream.Length - 2];
                Array.Copy(_stream, 1, payload, 0, _stream.Length - 2);
            }
            else
                type = messageType.INVALID;

            this.Decode();
        }

        private void Decode()
        {
            try
            {
                switch (type)
                {
                    case messageType.COMMAND:
                        uniqueID = BitConverter.ToUInt16(payload, 0);
                        if (commandID == 0x02)
                        {
                            valuesInt32 = new Int32[2];
                            for (int i = 0; i < 2; i++)
                                valuesInt32[i] = BitConverter.ToInt32(payload, 3 + i * 4);
                        }
                        break;
                    case messageType.INVALID:
                    default:
                        uniqueID = 0;
                        payload = null;
                        valuesByte = null;
                        valuesInt32 = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.type = messageType.INVALID;
                uniqueID = 0;
                payload = null;
            }
        }

        public messageType GetMessageType()
        {
            return this.type;
        }

        public UInt16 GetUniqueID()
        {
            return this.uniqueID;
        }

        public byte GetCommandID()
        {
            return this.commandID;
        }

        public byte[] GetByteValues()
        {
            return valuesByte;
        }

        public byte[] GetImgData()
        {
            return ImgRxData;
        }

        public byte GetFeedBackID()
        {
            return feedbackID;
        }

        public byte GetErrorID()
        {
            return errorID;
        }

        public Int32[] GetInt32Values()
        {
            return valuesInt32;
        }

        public byte[] GetPayload()
        {
            return payload;
        }
        
        #endregion
    }
}
