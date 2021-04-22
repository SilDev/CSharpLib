#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: XmlHelper.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    internal static class XmlHelper
    {
        internal static bool SerializeToFile<TSource>(string path, TSource source, bool overwrite = true)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                var dest = PathEx.Combine(path);
                using var fs = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew);
                new XmlSerializer(typeof(TSource)).Serialize(fs, source);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        internal static TResult DeserializeFile<TResult>(string path, TResult defValue = default)
        {
            try
            {
                var src = PathEx.Combine(path);
                if (!File.Exists(src))
                    return defValue;
                TResult result;
                var fs = default(FileStream);
                try
                {
                    fs = new FileStream(src, FileMode.Open, FileAccess.Read);
                    using var xr = XmlReader.Create(fs, new XmlReaderSettings { Async = false });
                    fs = null;
                    result = (TResult)new XmlSerializer(typeof(TResult)).Deserialize(xr);
                }
                finally
                {
                    fs?.Dispose();
                }
                return result;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return defValue;
            }
        }
    }
}
