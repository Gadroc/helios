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

namespace GadrocsWorkshop.Helios.Util
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Utility class to create and use sharedmemory areas.
    /// </summary>
    public class SharedMemory
    {
        private bool _disposed;

        private string _sharedMemoryName;
        private int _checkValue;

        private IntPtr _sharedMemoryHandle = IntPtr.Zero;
        private IntPtr _sharedMemoryAddress = IntPtr.Zero;

        public SharedMemory(string sharedMemoryName)
        {
            _sharedMemoryName = sharedMemoryName;
        }

        public int CheckValue
        {
            get
            {
                return _checkValue;
            }
            set
            {
                _checkValue = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return (_sharedMemoryAddress != IntPtr.Zero);
            }
        }

        public bool IsDataAvailable
        {
            get
            {
                if (!IsOpen)
                {
                    Open();
                }

                if (IsOpen)
                {
                    int value = Marshal.ReadInt32(_sharedMemoryAddress);
                    if (value != CheckValue)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public object MarshalTo(Type type)
        {
            return Marshal.PtrToStructure(_sharedMemoryAddress, type);
        }

        public object MarshalTo(Type type, Int64 offset)
        {
            return Marshal.PtrToStructure(GetPointer(offset), type);
        }

        public IntPtr GetPointer()
        {
            return _sharedMemoryAddress;
        }

        public IntPtr GetPointer(Int64 offset)
        {
            return new IntPtr(_sharedMemoryAddress.ToInt64() + offset);
        }

        public bool Open()
        {
            if (_sharedMemoryAddress != IntPtr.Zero)
            {
                return true;
            }

            if (_sharedMemoryHandle == IntPtr.Zero)
            {
                _sharedMemoryHandle = NativeMethods.OpenFileMapping(NativeMethods.FileMapAccess.FileMapRead, false, _sharedMemoryName);
            }

            if (_sharedMemoryHandle != IntPtr.Zero)
            {
                _sharedMemoryAddress = NativeMethods.MapViewOfFile(_sharedMemoryHandle, NativeMethods.FileMapAccess.FileMapRead, 0, 0, 0);

                if (_sharedMemoryAddress != IntPtr.Zero)
                {
                    return true;
                }
            }

            return false;
        }

        public void Close()
        {
            if (_sharedMemoryAddress != IntPtr.Zero)
            {
                NativeMethods.UnmapViewOfFile(_sharedMemoryAddress);
                _sharedMemoryAddress = IntPtr.Zero;
            }
            if (_sharedMemoryHandle != IntPtr.Zero)
            {
                NativeMethods.CloseHandle(_sharedMemoryHandle);
                _sharedMemoryHandle = IntPtr.Zero;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
            }
            _disposed = true;
        }

        ~SharedMemory()
        {
            Dispose(false);
        }

        #endregion
    }
}
