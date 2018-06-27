using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;

#if x64
[assembly: AssemblyTitle("Si13n7 Dev. ® CSharp Library (64-bit)")]
[assembly: AssemblyDescription("Si13n7 Dev. ® CSharp Library compiled for 64-bit platform environments")]
[assembly: AssemblyProduct("SilDev.CSharpLib64")]
#else
[assembly: AssemblyTitle("Si13n7 Dev. ® CSharp Library")]
[assembly: AssemblyDescription("Si13n7 Dev. ® CSharp Library compiled for 32-bit platform environments")]
[assembly: AssemblyProduct("SilDev.CSharpLib")]
#endif

#if debug
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Si13n7 Dev. ®")]
[assembly: AssemblyCopyright("Copyright © Si13n7 Dev. ® 2018")]
[assembly: AssemblyTrademark("Si13n7 Dev. ®")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("901502cc-aa7d-444e-944a-7fc063c34917")]

[assembly: AssemblyVersion("18.6.27.0")]

[assembly: NeutralResourcesLanguage("")]

