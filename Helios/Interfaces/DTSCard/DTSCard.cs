//  Copyright 2015 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios.Interfaces.DTSCard
{
    using HidSharp;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DTSCard
    {
        /// <summary>
        /// Serial Number of the card which we will be communicating with.
        /// </summary>
        private string _serialNumber;

        // Buffer for sending output data
        private byte[] _outputBuffer = new byte[5] { 0, 0, 0, 0 ,0 };

        private HidDevice _device;
        private HidStream _stream;

        public DTSCard(string serialNumber)
        {
            _serialNumber = serialNumber;
        }

        #region Device Enumerator

        /// <summary>
        /// Returns a collection of serialnumbers of currenlty attached DTS cards.
        /// </summary>
        public static IList<String> CardSerialNumbers
        {
            get
            {
                HidDeviceLoader deviceLoader = new HidDeviceLoader();
                List<String> cards = new List<String>();
                foreach (HidDevice device in deviceLoader.GetDevices(0x04d8, 0xf64e))
                {
                    cards.Add(device.SerialNumber);
                }
                return cards;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns serial number of this DTS Card.
        /// </summary>
        public string SerialNumber
        {
            get { return _serialNumber; }
        }

        private UInt16 S1
        {
            get
            {
                return (UInt16)((_outputBuffer[1] << 8) + _outputBuffer[2]);
            }
            set 
            {
		        _outputBuffer[1] = (byte)((value >> 8) & 0x0F);
		        _outputBuffer[2] = (byte)(value & 0xFF);
            }
        }

        private UInt16 S3
        {
            get
            {
                return (UInt16)((_outputBuffer[1] << 8) + _outputBuffer[2]);
            }
            set
            {
                _outputBuffer[3] = (byte)((value >> 8) & 0x0F);
                _outputBuffer[4] = (byte)(value & 0xFF);
            }
        }

        #endregion

        /// <summary>
        /// Initializes communication with the DTS Card
        /// </summary>
        /// <returns>True if update is sucessful, false if there was a problem.</returns>
        public bool initialize()
        {
            if (_device == null) 
            {
                dispose();
            }

            HidDeviceLoader deviceLoader = new HidDeviceLoader();
            foreach (HidDevice device in deviceLoader.GetDevices(0x04d8, 0xf64e))
            {
                if (device.SerialNumber.Equals(SerialNumber))
                {
                    _device = device;
                    break;
                }
            }

            if (_device != null)
            {
                _device.TryOpen(out _stream);
            }

            return _stream != null;
        }

        /// <summary>
        /// Cleans up all open connections.  This should be called when you are done using the card.
        /// </summary>
        public void dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
                _stream = null;
            }
        }

        /// <summary>
        /// Sets the values of the D/A-converters on the board directly (0-4095).
        /// </summary>
        /// <param name="valueS1">D/A Value for S1</param>
        /// <param name="valueS3">D/A Value for S3</param>
        public void setValues(UInt16 valueS1, UInt16 valueS3)
        {
            S1 = valueS1;
            S3 = valueS3;
            sendValues();
        }


        /// <summary>
        /// Sets the values of the D/A-converters on the board directly (0-4095).
        /// TODO: Handle previous transmision blocking
        /// </summary>
        /// <param name="valueS1">D/A Value for S1</param>
        public void setValuesS1(UInt16 valueS1)
        {
            S1 = valueS1;
            sendValues();
        }

        /// <summary>
        /// Sets the values of the D/A-converters on the board directly (0-4095).
        /// TODO: Handle previous transmision blocking
        /// </summary>
        /// <param name="valueS3">D/A Value for S3</param>
        public void setValuesS3(UInt16 valueS3)
        {
            S3 = valueS3;
            sendValues();
        }

        /// <summary>
        /// Sets the D/A converters to represent a specific angle in degrees.
        /// </summary>
        /// <param name="angle">Angle in degrees you wan the synchro to turn to.</param>
        public void setAngle(double angle)
        {
            S1 = (UInt16)(Math.Sin(angle * Math.PI / 180 - 2 * Math.PI / 3) * 2048 + 2048);
            S3 = (UInt16)(-Math.Sin(angle * Math.PI / 180 + 2 * Math.PI / 3) * 2048 + 2048);
            sendValues(); ;
        }

        /// <summary>
        /// Sends the current set of values to the card.
        /// </summary>
        private void sendValues()
        {
            if (_stream != null && _stream.CanWrite)
            {                
                _stream.Write(_outputBuffer);
            }            
        }
    }
}
