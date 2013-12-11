//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios
{
    using System.Collections.Generic;

    // A delegate type for handling digital inputs
    public delegate void DigitalInputHandler(object sender, DigitalInputEventArgs e);

    // A delegate type for handling analog inputs
    public delegate void AnalogInputHandler(object sender, AnalogInputEventArgs e);

    // A delegate type for handling analog inputs
    public delegate void PulseInputHandler(object sender, PulseInputEventArgs e);

    /// <summary>
    /// Interface for exposing physical devices which supplies inputs and outputs to the Helios Runtime.  Implemntation will
    /// be created
    /// </summary>
    public interface IDevice : IProfileObject
    {
        /// <summary>
        /// If true there should be a limit of only one of this device type per profile.
        /// </summary>
        bool IsUnique { get; }

        /// <summary>
        /// Number of digital on/off inputs this device provides.
        /// </summary>
        int DigitalInputCount { get; }

        /// <summary>
        /// Returns current known state of a digital input.
        /// </summary>
        /// <param name="index">Index of the digital input to read.</param>
        /// <returns>True/false value indicating the current known state of the digital input.</returns>
        bool ReadDigitalInput(int index);

        /// <summary>
        /// Fired when a digital input has changed state.
        /// </summary>
        event DigitalInputHandler DigitalInputChanged;

        /// <summary>
        /// Number of digital on/off outputs this device provides.
        /// </summary>
        int DigitalOutputCount { get; }

        /// <summary>
        /// Sets the state of a digital output.
        /// </summary>
        /// <param name="index">Index of the output to set.</param>
        /// <param name="value">True to set the digital output on/high and false to set the digital output off/low.</param>
        void SetDigitalOutput(int index, bool value);

        /// <summary>
        /// Number of analog inputs this device provides.
        /// </summary>
        int AnalogInputCount { get; }

        /// <summary>
        /// Returns the current known value of an analog input.
        /// </summary>
        /// <param name="index">Index of the analog input to read.</param>
        /// <returns>Int value between 0-65535 represneting the last known reading of the analog input.</returns>
        int ReadAnalogIntput(int index);

        /// <summary>
        /// Number of analog outputs (pwm / variable voltage outputs) this device provides.
        /// </summary>
        int AnalogOutputCount { get; }

        /// <summary>
        /// Fires when an analog inptut changes.
        /// </summary>
        event AnalogInputHandler AnalogInputChanged;

        /// <summary>
        /// Number of pulse inputs (rotary encoders or other acumulated input counters) this device provides.
        /// </summary>
        int PulseInputCount { get; }

        /// <summary>
        /// Fires when pulse inputs are received from this device.
        /// </summary>
        event PulseInputHandler PulseInputReceived;

        /// <summary>
        /// Number of servos which this device provides.
        /// </summary>
        int ServoOutputCount { get; }

        /// <summary>
        /// Number of steppers which this device provides.
        /// </summary>
        int StepperOutputCount { get; }

        /// <summary>
        /// Number of text output this device provides.
        /// </summary>
        int TextOutputCount { get; }

        /// <summary>
        /// Connects to this device in order to start receiving inputs and sending outputs.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects this device to stop receiving inputs and sending outputs.
        /// </summary>
        void Disconnect();
    }
}
