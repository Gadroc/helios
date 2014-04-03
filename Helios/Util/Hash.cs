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
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Utility class to calculate MD5 hash values from various resources.
    /// </summary>
    public static class Hash
    {
        static public string GetMD5HashFromResource(string resourceUri)
        {
            Stream resourceStream = Application.GetResourceStream(new Uri(resourceUri, UriKind.Absolute)).Stream;
            string retVal = GetMD5HashFromStream(resourceStream);
            resourceStream.Close();
            return retVal;
        }

        static public string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            string retVal = GetMD5HashFromStream(file);
            file.Close();
            return retVal;
        }

        static public string GetMD5HashFromStream(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(stream);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        static public string GetMD5HashFromString(string input)
        {
            MemoryStream data = new MemoryStream(Encoding.UTF8.GetBytes(input));
            string retVal = GetMD5HashFromStream(data);
            data.Close();
            return retVal;
        }
    }
}
