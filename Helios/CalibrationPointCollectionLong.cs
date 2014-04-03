//  Copyright 2014 Craig Courtney
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
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml;

    public class CalibrationPointCollectionLong : NotificationObject, ICollection<CalibrationPointLong>, INotifyCollectionChanged
    {
        private LinkedList<CalibrationPointLong> _points = new LinkedList<CalibrationPointLong>();

        private long _outputLimitMax = long.MaxValue;
        private long _outputLimitMin = long.MinValue;

        public event EventHandler CalibrationChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CalibrationPointCollectionLong()
        {
            Add(new CalibrationPointLong(0, 0));
            Add(new CalibrationPointLong(100, 1));
        }

        public CalibrationPointCollectionLong(double minimumInput, long minimumOuput, double maximumInput, long maximumOutput)
        {
            Add(new CalibrationPointLong(minimumInput, minimumOuput));
            Add(new CalibrationPointLong(maximumInput, maximumOutput));
        }

        public long Interpolate(double input)
        {
            if (_points.Count < 2)
            {
                return 0;
            }

            LinkedListNode<CalibrationPointLong> valuePoint = _points.First;
            while (valuePoint != null && valuePoint.Value.Value < input)
            {
                valuePoint = valuePoint.Next;
            }

            if (valuePoint == null)
            {
                return _points.Last.Value.Multiplier;
            }
            else if (valuePoint.Value.Value == input || valuePoint.Previous == null)
            {
                return valuePoint.Value.Multiplier;
            }
            else
            {
                double valueRange = valuePoint.Value.Value - valuePoint.Previous.Value.Value;
                long modifierRange = valuePoint.Value.Multiplier - valuePoint.Previous.Value.Multiplier;
                double offsetValue = input - valuePoint.Previous.Value.Value;
                double offsetMultiplier = offsetValue / valueRange;
                long modifier = (long)(modifierRange * offsetMultiplier);

                return valuePoint.Previous.Value.Multiplier + modifier;
            }
        }

        public long OutputLimitMin
        {
            get { return _outputLimitMin; }
            set
            {
                if (_outputLimitMin != value)
                {
                    _outputLimitMin = value;
                    if (MinimumOutputValue < _outputLimitMin)
                    {
                        MinimumOutputValue = _outputLimitMin;
                    }
                }
            }
        }

        public long OutputLimitMax
        {
            get { return _outputLimitMax; }
            set
            {
                if (_outputLimitMax != value)
                {
                    _outputLimitMin = value;
                    if (MaximumOutputValue > _outputLimitMax)
                    {
                        MaximumOutputValue = _outputLimitMax;
                    }
                }
            }
        }

        public double MaximumInputValue
        {
            get
            {
                return _points.Last.Value.Value;
            }
            set
            {
                double oldValue = _points.Last.Value.Value;
                double lastValue = value;
                //LinkedListNode<CalibrationPointLong> node = _points.Last.Previous;
                //while (node != null && node.Value.Value > lastValue)
                //{
                //    lastValue -= 0.1f;
                //    node.Value.Value = lastValue;
                //    node = node.Previous;
                //}
                _points.Last.Value.Value = value;
                OnCalibrationChanged();
                OnPropertyChanged("MaximumInputValue", oldValue, value, false);
            }
        }

        public long MaximumOutputValue
        {
            get
            {
                return _points.Last.Value.Multiplier;
            }
            set
            {
                long oldValue = _points.Last.Value.Multiplier;
                long lastValue = value;
                //LinkedListNode<CalibrationPointLong> node = _points.Last.Previous;
                //while (node != null && node.Value.Multiplier > lastValue)
                //{
                //    lastValue -= 1;
                //    node.Value.Multiplier = lastValue;
                //    node = node.Previous;
                //}
                _points.Last.Value.Multiplier = value;
                OnCalibrationChanged();
                OnPropertyChanged("MaximumOutputValue", oldValue, value, false);
            }
        }

        public double MinimumInputValue
        {
            get
            {
                return _points.First.Value.Value;
            }
            set
            {
                double oldValue = _points.First.Value.Value;
                double lastValue = value;
                LinkedListNode<CalibrationPointLong> node = _points.First.Next;
                while (node != null && node.Value.Value < lastValue)
                {
                    lastValue += 0.1f;
                    node.Value.Value = lastValue;
                    node = node.Next;
                }
                _points.First.Value.Value = value;
                OnCalibrationChanged();
                OnPropertyChanged("MinimumInputValue", oldValue, value, false);
            }
        }

        public long MinimumOutputValue
        {
            get
            {
                return _points.First.Value.Multiplier;
            }
            set
            {
                long oldValue = _points.First.Value.Multiplier;
                long lastValue = value;
                LinkedListNode<CalibrationPointLong> node = _points.First.Next;
                while (node != null && node.Value.Value < lastValue)
                {
                    lastValue += 1;
                    node.Value.Multiplier = lastValue;
                    node = node.Next;
                }
                _points.First.Value.Multiplier = value;
                OnCalibrationChanged();
                OnPropertyChanged("MinimumOutputValue", oldValue, value, false);
            }
        }

        #region ICollection<KeyPoint> Members

        public void Add(CalibrationPointLong item)
        {
            LinkedListNode<CalibrationPointLong> addBefore = _points.First;
            while (addBefore != null && addBefore.Value.Value < item.Value)
            {
                addBefore = addBefore.Next;
            }

            if (addBefore == null)
            {
                _points.AddLast(item);
            }
            else
            {
                _points.AddBefore(addBefore, item);
            }

            item.PropertyChanged += Item_PropertyChanged;

            OnCalibrationChanged();
            OnCollectionChanged();
        }

        public void AddPointAfter(CalibrationPointLong item)
        {
            LinkedListNode<CalibrationPointLong> addAfter = _points.Find(item);
            if (addAfter.Next != null)
            {
                CalibrationPointLong point = new CalibrationPointLong(item.Value + ((addAfter.Next.Value.Value - item.Value) / 2), item.Multiplier + ((addAfter.Next.Value.Multiplier - item.Multiplier) / 2));
                Add(point);
            }
        }

        void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("Changed");
            OnCalibrationChanged();
        }

        public void Clear()
        {
            CalibrationPointLong first = _points.First.Value;
            CalibrationPointLong last = _points.Last.Value;
            _points.Clear();
            _points.AddFirst(first);
            _points.AddLast(last);
            OnCalibrationChanged();
            OnCollectionChanged();
        }

        public bool Contains(CalibrationPointLong item)
        {
            return _points.Contains(item);
        }

        public void CopyTo(CalibrationPointLong[] array, int arrayIndex)
        {
            _points.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _points.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CalibrationPointLong item)
        {
            if (item != _points.First.Value && item != _points.Last.Value)
            {
                bool retValue = _points.Remove(item);
                item.PropertyChanged -= Item_PropertyChanged;
                OnCalibrationChanged();
                OnCollectionChanged();
                return retValue;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<KeyPoint> Members

        public IEnumerator<CalibrationPointLong> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        #endregion

        private void OnCalibrationChanged()
        {
            EventHandler handler = CalibrationChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnCollectionChanged()
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Write(XmlWriter writer)
        {
            writer.WriteElementString("PointCount", _points.Count.ToString(CultureInfo.InvariantCulture));
            writer.WriteStartElement("CalibrationPoints");
            LinkedListNode<CalibrationPointLong> point = _points.First;
            while (point != null)
            {
                writer.WriteStartElement("Point");
                writer.WriteElementString("Input", point.Value.Value.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Output", point.Value.Multiplier.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
                point = point.Next;
            }
            writer.WriteEndElement();
        }

        public void Read(XmlReader reader)
        {
            int count = int.Parse(reader.ReadElementString("PointCount"), CultureInfo.InvariantCulture);
            reader.ReadStartElement("CalibrationPoints");

            CalibrationPointLong point = ReadPoint(reader);
            MinimumInputValue = point.Value;
            MinimumOutputValue = point.Multiplier;

            for (int i = 2; i < count; i++)
            {
                Add(ReadPoint(reader));
            }

            point = ReadPoint(reader);
            MaximumInputValue = point.Value;
            MaximumOutputValue = point.Multiplier;

            reader.ReadEndElement();
        }

        private CalibrationPointLong ReadPoint(XmlReader reader)
        {
            reader.ReadStartElement("Point");
            double value = double.Parse(reader.ReadElementString("Input"), CultureInfo.InvariantCulture);
            long output = long.Parse(reader.ReadElementString("Output"), CultureInfo.InvariantCulture);
            reader.ReadEndElement();
            return new CalibrationPointLong(value, output);
        }
    }
}
