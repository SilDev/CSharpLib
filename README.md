<p align="center"><a href="https://www.microsoft.com/download/details.aspx?id=55170"><img src="https://img.shields.io/badge/platform->=%20v4.7-lightgrey.svg?style=flat&logo=.net&logoColor=white" alt="platform"></a> &nbsp; <a href="https://github.com/Si13n7/SilDev.CSharpLib/blob/master/LICENSE.txt"><img src="https://img.shields.io/github/license/Si13n7/SilDev.CSharpLib.svg?style=flat" alt="license"></a> &nbsp; <a href="https://github.com/Si13n7/SilDev.CSharpLib/archive/master.zip"><img src="https://img.shields.io/badge/download-source-yellow.svg?style=flat" alt="download"></a> &nbsp; <a href="https://www.si13n7.com"><img src="https://img.shields.io/website/https/www.si13n7.com.svg?style=flat&down_color=red&down_message=offline&up_color=limegreen&up_message=online&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEwSURBVDhPxZJNSgNBEIXnCp5AcCO4CmaTRRaKBhdCFkGCCKLgz2Y2RiQgCiqZzmi3CG4COj0X8ApewSt4Ba%2FQ9leZGpyVG8GComtq3qv3qmeS%2Fw9nikHMd5sVn3bqLx7zom1NcW8z%2F6G9CjoPm722rPEv45EJ21vD0O30AvX12IWDvTRsrPXrnjPlUYO0u3McVpZXhch5cnguZ7vVDWfpjRAZgPqc%2BIMEgKQe9Pfr0xn%2FBqZJjAUNQKilp5cC1gHYYz8Usc3OQsTz9HZWK5BMJwFDwrbWbuIXhfhg%2FDpWuE2mK5lEgQtiz4baU14u3V09i5peiipy6qVAxFWtZiflJiq8AAiIZx1CnxpStGmEpEHDZf4r2pUd%2BMjYxomoxJofo4L%2FHqyR57OF6vEvIkm%2BAYRc%2BWd4P97CAAAAAElFTkSuQmCC" alt="website"></a> &nbsp; <a href="https://www.si13n7.de"><img src="https://img.shields.io/website/https/www.si13n7.de.svg?style=flat&down_color=red&down_message=offline&label=mirror&up_color=limegreen&up_message=online&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEwSURBVDhPxZJNSgNBEIXnCp5AcCO4CmaTRRaKBhdCFkGCCKLgz2Y2RiQgCiqZzmi3CG4COj0X8ApewSt4Ba%2FQ9leZGpyVG8GComtq3qv3qmeS%2Fw9nikHMd5sVn3bqLx7zom1NcW8z%2F6G9CjoPm722rPEv45EJ21vD0O30AvX12IWDvTRsrPXrnjPlUYO0u3McVpZXhch5cnguZ7vVDWfpjRAZgPqc%2BIMEgKQe9Pfr0xn%2FBqZJjAUNQKilp5cC1gHYYz8Usc3OQsTz9HZWK5BMJwFDwrbWbuIXhfhg%2FDpWuE2mK5lEgQtiz4baU14u3V09i5peiipy6qVAxFWtZiflJiq8AAiIZx1CnxpStGmEpEHDZf4r2pUd%2BMjYxomoxJofo4L%2FHqyR57OF6vEvIkm%2BAYRc%2BWd4P97CAAAAAElFTkSuQmCC" alt="mirror"></a></p>

# Si13n7 Dev.™ CSharp Library

I was tired of repeating the same steps over and over again when I started developing a new program. So I started writing some classes that I could easily use here and there. This got more and more over time, which made the whole thing rather cluttered and less useful. So I had to start documenting functions cleanly. The chaos became a useful little library, which contains many useful functions. Everything is documented within the classes, so I do not bother to explain this extra. Just browse the classes and take with you, whatever you need, everything here is free.

***

### Requirements:
- Microsoft Windows 7 or higher (64-bit)
- [Microsoft Visual Studio 2017 + .NET Framework 4.7 SDK](https://www.visualstudio.com/downloads/)

***

#### Example of using separate DLLs for the `Any CPU` platform target:

```cs
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += DynamicAssemblyResolve;
            /*
                some code ...
            */
        }

        private static Assembly DynamicAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name;
            if (!name.StartsWith("SilDev."))
                return null;
            var length = name.IndexOf(',');
            name = name.Substring(0, length);
            return (from dir in GetProbingDirs()
                    select Environment.Is64BitProcess ?
                        Path.Combine(dir, $"{name}64.dll") :
                        Path.Combine(dir, $"{name}32.dll")
                    into path
                    where File.Exists(path)
                    select Assembly.LoadFile(path)).FirstOrDefault();
        }

        private static IEnumerable<string> GetProbingDirs()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            yield return baseDir;

            var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (!File.Exists(configPath))
                yield break;

            string entry;
            try
            {
                const string xpath = "/*[name()='configuration']/*[name()='runtime']/*[name()='assemblyBinding']/*[name()='probing']/@privatePath";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);
                entry = (xmlDoc.SelectSingleNode(xpath) as XmlAttribute)?.Value;
                if (string.IsNullOrWhiteSpace(entry))
                    throw new ArgumentNullException();
            }
            catch
            {
                yield break;
            }

            if (entry.Contains(";"))
                foreach (var item in entry.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    yield return Path.Combine(baseDir, item);
            else
                yield return Path.Combine(baseDir, entry);
        }
    }
```