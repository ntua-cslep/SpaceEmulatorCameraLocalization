using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraServo.Common
{
    public enum messageType { COMMAND = 0x00, FEEDBACK, ACK, ERROR, INVALID};
    public enum feedbackType { OPTICAL = 0x20, ARM, WHEEL, HAL, IMAGE, CAMERA_CONF, ROBOT_POSITION };
    public enum errorType { UNKNOWN_MESSAGE = 0x01, VALUE_OUT_OF_BOUNDS, INVALID_ARGUMENTS, HARDWARE_FAULT, TERMINAL_REACHED };

    public static class Globals
    {
        public static byte SEPARATOR = 0x7E;
        public static byte ESCAPER = 0x7D;
        /*
        public static void Increment<TKey>(this Dictionary<TKey, UInt16> dictionary)
        {
            for (int i = 0; i < dictionary.Keys.Count; i++)
            {
                dictionary[dictionary.Keys.ElementAt(i)] += 1;
                if (dictionary[dictionary.Keys.ElementAt(i)] >= Properties.Settings.Default.MESSAGETIMEOUT)
                {
                    Space_Robot_Console.Program.form1.AppendRobotText("Message acknowledgement timeout (" + dictionary.Keys.ElementAt(i).ToString() + ")\n");
                    dictionary.Remove(dictionary.Keys.ElementAt(i));
                }
            }
        }
        */

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    public class Framing
    {
        public byte[] EscapeBytes(byte[] _in)
        {
            int num = 0;
            for (int i = 1; i < _in.Length-1; i++)
                if (_in[i] == Globals.ESCAPER || _in[i] == Globals.SEPARATOR)
                    num++;

            byte[] _out = new byte[_in.Length + num];

            for (int i = 1, j = 1; i < _in.Length-1; i++)
            {
                if (_in[i] == Globals.ESCAPER || _in[i] == Globals.SEPARATOR)
                {
                    _out[j++] = Globals.ESCAPER;
                    _out[j++] = (byte)(_in[i] ^ 0x20);
                }
                else
                    _out[j++] = _in[i];
            }
            _out[0] = _in[0];
            _out[_out.Length - 1] = _in[_in.Length - 1];
            return _out;
        }

        public byte[] UnEscapeBytes(byte[] _in)
        {
            int num = 0;
            bool toEscape = false;
            for (int i = 1; i < _in.Length-1; i++)
                if (_in[i] == Globals.ESCAPER)
                    num++;

            byte[] _out = new byte[_in.Length - num];

            _out[0] = _in[0];
            _out[_out.Length - 1] = _in[_in.Length - 1];

            for (int i = 1, j = 1; i < _in.Length-1; i++)
            {
                if (_in[i] == Globals.ESCAPER)
                {
                    toEscape = true;
                }
                else
                {
                    if (toEscape)
                    {
                        _out[j++] = (byte)(_in[i] ^ 0x20);
                        toEscape = false;
                    }
                    else
                        _out[j++] = _in[i];
                }
            }

            return _out;
        }
    }
}
