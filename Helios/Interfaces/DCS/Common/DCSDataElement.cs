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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Common
{
    using GadrocsWorkshop.Helios.UDPInterface;

    public class DCSDataElement : ExportDataElement
    {
        private string _exportFormat;
        private bool _everyFrame;

        public DCSDataElement(string id)
            : this(id, null, false)
        {
        }

        public DCSDataElement(string id, string format)
            : this (id, format, false)
        {
        }

        public DCSDataElement(string id, string format, bool everyFrame)
            : base(id)
        {
            _exportFormat = format;
            _everyFrame = everyFrame;
        }

        public string Format
        {
            get { return _exportFormat; }
        }

        public bool IsExportedEveryFrame
        {
            get { return _everyFrame; }
        }
    }
}
