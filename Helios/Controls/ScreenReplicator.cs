﻿//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Controls
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Xml;

    [HeliosControl("Helios.Base.ScreenReplicator", "Screen Replicator", "Miscellaneous", typeof(ScreenReplicatorRenderer))]
    public class ScreenReplicator : HeliosVisual
    {
        private Int32Rect _catprueLocation = new Int32Rect(0, 0, 300, 300);
        private int _replicationsPerSecond = 2;
        private bool _replicateOnProfileStart = false;
        private bool _isRunning = false;
        private bool _isReplicating = false;
        private bool _blankOnStop = true;

        private int _millisecondsPerReplication = 500;
        private int _lastReplication;

        private HeliosAction _startReplicating;
        private HeliosAction _stopReplicating;

        public ScreenReplicator()
            : base("Screen Shot Extractor", new Size(300, 300))
        {
            _startReplicating = new HeliosAction(this, "", "", "start replication", "Start replicating the screen.");
            _startReplicating.Execute += new HeliosActionHandler(StartReplicating_Execute);
            Actions.Add(_startReplicating);

            _stopReplicating = new HeliosAction(this, "", "", "stop replication", "Stops replicating the screen.");
            _stopReplicating.Execute += new HeliosActionHandler(StopReplicating_Execute);
            Actions.Add(_stopReplicating);
        }

        void StartReplicating_Execute(object action, HeliosActionEventArgs e)
        {
            _isReplicating = true;
        }

        void StopReplicating_Execute(object action, HeliosActionEventArgs e)
        {
            _isReplicating = false;
            OnDisplayUpdate();
        }

        #region Properties

        public int CaptureTop
        {
            get
            {
                return _catprueLocation.Y;
            }
            set
            {
                if (!_catprueLocation.Y.Equals(value))
                {
                    int oldValue = _catprueLocation.Y;
                    _catprueLocation.Y = value;
                    OnPropertyChanged("CaptureTop", oldValue, value, true);
                    OnDisplayUpdate();
                }
            }
        }

        public int CaptureLeft
        {
            get
            {
                return _catprueLocation.X;
            }
            set
            {
                if (!_catprueLocation.X.Equals(value))
                {
                    int oldValue = _catprueLocation.X;
                    _catprueLocation.X = value;
                    OnPropertyChanged("CaptureLeft", oldValue, value, true);
                    OnDisplayUpdate();
                }
            }
        }

        public int CaptureWidth
        {
            get
            {
                return _catprueLocation.Width;
            }
            set
            {
                if (!_catprueLocation.Width.Equals(value))
                {
                    int oldValue = _catprueLocation.Width;
                    _catprueLocation.Width = value;
                    OnPropertyChanged("CaptureWidth", oldValue, value, true);
                    OnDisplayUpdate();
                }
            }
        }

        public int CaptureHeight
        {
            get
            {
                return _catprueLocation.Height;
            }
            set
            {
                if (!_catprueLocation.Height.Equals(value))
                {
                    int oldValue = _catprueLocation.Height;
                    _catprueLocation.Height = value;
                    OnPropertyChanged("CaptureHeight", oldValue, value, true);
                    OnDisplayUpdate();
                }
            }
        }

        internal Int32Rect CaptureRectangle
        {
            get
            {
                return _catprueLocation;
            }
        }

        public int ReplicationsPerSecond
        {
            get
            {
                return _replicationsPerSecond;
            }
            set
            {
                if (!_replicationsPerSecond.Equals(value))
                {
                    int oldValue = _replicationsPerSecond;
                    _replicationsPerSecond = value;
                    _millisecondsPerReplication = Math.Max(1000 / _replicationsPerSecond, 100);
                    OnPropertyChanged("ReplicationsPerSecond", oldValue, value, true);
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            private set
            {
                if (!_isRunning.Equals(value))
                {
                    bool oldValue = _isRunning;
                    _isRunning = value;
                    OnPropertyChanged("IsRunning", oldValue, value, false);
                }
            }
        }

        public bool IsReplicating
        {
            get
            {
                return _isReplicating;
            }
            set
            {
                if (!_isReplicating.Equals(value))
                {
                    bool oldValue = _isReplicating;
                    _isReplicating = value;
                    OnPropertyChanged("IsReplicating", oldValue, value, false);
                }
            }
        }

        public bool ReplicatesOnProfileStart
        {
            get
            {
                return _replicateOnProfileStart;
            }
            set
            {
                if (!_replicateOnProfileStart.Equals(value))
                {
                    bool oldValue = _replicateOnProfileStart;
                    _replicateOnProfileStart = value;
                    OnPropertyChanged("ReplicatesOnProfileStart", oldValue, value, true);
                }
            }
        }

        public bool BlankOnStop
        {
            get
            {
                return _blankOnStop;
            }
            set
            {
                if (!_blankOnStop.Equals(value))
                {
                    bool oldValue = _blankOnStop;
                    _blankOnStop = value;
                    OnPropertyChanged("BlankOnStop", oldValue, value, true);
                }
            }
        }

        #endregion

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);

            if (oldProfile != null)
            {
                oldProfile.ProfileStarted -= new EventHandler(Profile_ProfileStarted);
                oldProfile.ProfileTick -= new EventHandler(Profile_ProfileTick);
                oldProfile.ProfileStopped -= new EventHandler(Profile_ProfileStopped);
            }

            if (Profile != null)
            {
                Profile.ProfileStarted += new EventHandler(Profile_ProfileStarted);
                Profile.ProfileTick += new EventHandler(Profile_ProfileTick);
                Profile.ProfileStopped += new EventHandler(Profile_ProfileStopped);
            }
        }

        void Profile_ProfileStopped(object sender, EventArgs e)
        {
            IsReplicating = false;
            IsRunning = false;
        }

        void Profile_ProfileTick(object sender, EventArgs e)
        {
            int currentTime = Environment.TickCount;

            if (currentTime - _lastReplication > _millisecondsPerReplication)
            {
                OnDisplayUpdate();
                _lastReplication = currentTime;
            }
        }

        void Profile_ProfileStarted(object sender, EventArgs e)
        {
            IsRunning = true;
            IsReplicating = ReplicatesOnProfileStart;
        }

        public override void MouseDown(System.Windows.Point location)
        {
            // No-Op
        }

        public override void MouseDrag(System.Windows.Point location)
        {
            // No-Op
        }

        public override void MouseUp(System.Windows.Point location)
        {
            // No-Op
        }

        public override void WriteXml(XmlWriter writer)
        {
            TypeConverter boolConverter = TypeDescriptor.GetConverter(typeof(bool));

            base.WriteXml(writer);
            writer.WriteElementString("CaptureTop", CaptureTop.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("CaptureLeft", CaptureLeft.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("CaptureWidth", CaptureWidth.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("CaptureHeight", CaptureHeight.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("UpdateFrequency", ReplicationsPerSecond.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("ReplicateOnStart", boolConverter.ConvertToInvariantString(ReplicatesOnProfileStart));
            writer.WriteElementString("BlankOnStop", boolConverter.ConvertToInvariantString(BlankOnStop));
        }

        public override void ReadXml(XmlReader reader)
        {
            TypeConverter boolConverter = TypeDescriptor.GetConverter(typeof(bool));

            base.ReadXml(reader);
            CaptureTop = int.Parse(reader.ReadElementString("CaptureTop"), CultureInfo.InvariantCulture);
            CaptureLeft = int.Parse(reader.ReadElementString("CaptureLeft"), CultureInfo.InvariantCulture);
            CaptureWidth = int.Parse(reader.ReadElementString("CaptureWidth"), CultureInfo.InvariantCulture);
            CaptureHeight = int.Parse(reader.ReadElementString("CaptureHeight"), CultureInfo.InvariantCulture);
            ReplicationsPerSecond = int.Parse(reader.ReadElementString("UpdateFrequency"), CultureInfo.InvariantCulture);
            ReplicatesOnProfileStart = (bool)boolConverter.ConvertFromInvariantString(reader.ReadElementString("ReplicateOnStart"));
            BlankOnStop = (bool)boolConverter.ConvertFromInvariantString(reader.ReadElementString("BlankOnStop"));
        }
    }
}
