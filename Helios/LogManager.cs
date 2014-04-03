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
    using System.IO;
    using System.Reflection;

    public class LogManager
    {
        private string _logFile;
        private LogLevel _level = LogLevel.Info;

        private System.Object _lock = new System.Object();

        public LogManager(string path, LogLevel level)
        {
            _logFile = path;
            _level = level;
        }

        #region Properties

        public LogLevel LogLevel
        {
            get { return _level; }
            set { _level = value; }
        }

        #endregion

        public void LogDebug(string message)
        {
            WriteLogMessage(Helios.LogLevel.Debug, message, null);
        }

        public void Log(string message)
        {
            WriteLogMessage(Helios.LogLevel.All, message, null);
        }

        public void LogWarning(string message)
        {
            WriteLogMessage(LogLevel.Warning, message, null);
        }

        public void LogWarning(string message, Exception exception)
        {
            WriteLogMessage(LogLevel.Warning, message, exception);
        }

        public void LogError(string message)
        {
            WriteLogMessage(LogLevel.Error, message, null);
        }

        public void LogError(string message, Exception exception)
        {
            WriteLogMessage(LogLevel.Error, message, exception);
        }

        public void LogInfo(string message)
        {
            WriteLogMessage(LogLevel.Info, message, null);
        }

        private void WriteLogMessage(LogLevel level, string message, Exception exception)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            if (_level >= level)
            {
                lock (_lock)
                {
                    try
                    {
                        FileInfo errorFile = new FileInfo(_logFile);

                        StreamWriter errorWriter;

                        if (errorFile.Exists)
                        {
                            errorWriter = errorFile.AppendText();
                        }
                        else
                        {
                            errorWriter = errorFile.CreateText();
                        }

                        using (errorWriter)
                        {

                            errorWriter.Write(DateTime.Now.ToString());
                            errorWriter.Write(" - ");
                            errorWriter.Write(level.ToString());
                            errorWriter.Write(" - ");
                            errorWriter.WriteLine(message);

                            if (exception != null)
                            {
                                WriteException(errorWriter, exception);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Nothing to do but go on.
                    }
                }
            }
        }

        private void WriteException(StreamWriter writer, Exception exception)
        {
            if (exception.Source != null && exception.Source.Length > 0)
            {
                writer.WriteLine("Exception Source:" + exception.Source);
            }
            writer.WriteLine("Exception Message:" + exception.Message);
            writer.WriteLine("Stack Trace:");
            writer.WriteLine(exception.StackTrace);

            ReflectionTypeLoadException le = exception as ReflectionTypeLoadException;
            if (le != null)
            {
                foreach (Exception e2 in le.LoaderExceptions)
                {
                    WriteException(writer, e2);
                }
            }

            if (exception.InnerException != null)
            {
                writer.WriteLine();
                WriteException(writer, exception.InnerException);
            }
        }
    }
}
