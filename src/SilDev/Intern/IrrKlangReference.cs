#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangReference.cs
// Version:  2018-06-23 22:44
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Properties;

    internal static class IrrKlangReference
    {
        private static Assembly _assembly;
        private static bool _assemblyChecked;
        private static readonly object Locker = new object();

        internal static Assembly Assembly
        {
            get
            {
                lock (Locker)
                {
                    if (_assemblyChecked)
                        return _assembly;

                    const string coreFileName = "irrKlang.NET4.dll";
                    var fileMap = new Dictionary<string, Tuple<string, string>>
                    {
#if x86
                        {
                            "ikpFlac.dll",
                            Tuple.Create("c07092af7c938648125d2ec34990b80779399191341a54b6ffd96ef259b327724e7c67987e9f1af720496796857dfa06", 
                                         "c1407a08e134a35d13e872f502873140c01432cff68ab08bb80281b25872fa6938a6282cc15feae03857d43d5c70d6fddedade6dcec027f764914a3fbf89d161")
                        },            
                        {
                            "ikpMP3.dll",
                            Tuple.Create("7a15251650cc2f4712769b6be47af5293deb97143d6ba6905f9b8ab233767069e65ca0c0cd3cc8b06c7ecc5952138082", 
                                         "a2e57a537aa7d005a8f6cdfdef287fad9d52b107752aeee6bef66f7215caa7066c6844e501cf6fa8c8464c24626c81a6e6d1346a71e4ec1590fa6e7f3d7f95ca")
                        },            
                        {
                            coreFileName,
                            Tuple.Create("38699be5803259dbcbdae626aca33fc7e692d47a27ae1247377756e897520a089e1c0d2ebae93f4fba3013bea7348413", 
                                         "84aa7125cd8a08c373c09dbb6218703805383f0c35f0d85e8640956ebee452ee633bef0a0ee4a60a2109836b8b526f00cee17f036cc7645f7cd3538c06ccd01d")
                        }
#else
                        {
                            "ikpFlac.dll",
                            Tuple.Create("e50cbc6edfb8ccd6eff87814605e440b813f92dc0c77e3b78fc781c075b5c84550bf7a23031a41e213ddd083912899c5",
                                         "119262a8ca99e85f0c21c08cb4df5c4cdbe694d091d2d7d0934b8c39c68fd013bb0c95d3cee2b72fe0ed8211f12b800442b447667a50f6dad6ff0267e55d9128")
                        },
                        {
                            "ikpMP3.dll",
                            Tuple.Create("35823d36ea64950426496dec507a92d4511b86a256dcb4b2987c7268e42ef9c3acff24b826d4ad2abfaf5f03fb202ea2",
                                         "b0c5417f9bc29c3d321d53d00d23104d31da5af276a42523044e074c9b5b5f083257f2fea5056b66d2e80eb4cace823e914bcb57685da4696b02851740ac10ea")
                        },
                        {
                            coreFileName,
                            Tuple.Create("cde01a51220e198a2595c459ef6dbb67940e507f30e91eea6402ee2101c9f7945d80dbb67b2d1dc043593e1dd05e9718",
                                         "489b1cfc5716ce21f978de60a34bf7fe4402cc77444263f57ae658737ffb1896a00b2cce803d8a3618a980312a1bb71cc316166a039d26896ebdbc238818dde6")
                        }
#endif
                    };

                    var refDirs = string.Format(Resources.ReferenceDirs, Math.Abs(PathEx.LocalPath.GetHashCode())).SplitNewLine();
                    var isValid = false;
                    foreach (var entry in refDirs.Select(PathEx.Combine))
                    {
                        var dirs = new[]
                        {
                            entry,
#if x86
                            Path.Combine(entry, "32"),
                            Path.Combine(entry, "x86"),
#else
                            Path.Combine(entry, "64"),
                            Path.Combine(entry, "x64")
#endif
                        };
                        foreach (var dir in dirs)
                        {
                            var path = PathEx.Combine(dir, coreFileName);
#if x86
                            if (!File.Exists(path) || PortableExecutable.Is64Bit(path))
                                continue;
#else
                            if (!File.Exists(path) || !PortableExecutable.Is64Bit(path))
                                continue;
#endif
                            isValid = fileMap.Select(x => Tuple.Create(Path.Combine(dir, x.Key), x.Value.Item1, x.Value.Item2))
                                             .All(x => !File.Exists(x.Item1) ||
                                                       x.Item1.EncryptFile(ChecksumAlgorithms.Sha384).Equals(x.Item2) ||
                                                       x.Item1.EncryptFile(ChecksumAlgorithms.Sha512).Equals(x.Item3));
                            if (!isValid)
                                continue;

                            try
                            {
                                _assembly = Assembly.LoadFrom(path);
                                Location = dir;
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                                _assembly = null;
                                Location = null;
                                isValid = false;
                                continue;
                            }
                            break;
                        }
                        if (isValid)
                            break;
                    }

                    _assemblyChecked = true;
                    return _assembly;
                }
            }
        }

        internal static string Location { get; private set; }
    }
}
