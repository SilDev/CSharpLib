using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;

#if x86
[assembly: AssemblyTitle("Si13n7 Dev.™ CSharp Library")]
[assembly: AssemblyDescription("Si13n7 Dev.™ CSharp Library compiled for 32-bit platform environments")]
[assembly: AssemblyProduct("SilDev.CSharpLib")]
#elif x64
[assembly: AssemblyTitle("Si13n7 Dev.™ CSharp Library (64-bit)")]
[assembly: AssemblyDescription("Si13n7 Dev.™ CSharp Library compiled for 64-bit platform environments")]
[assembly: AssemblyProduct("SilDev.CSharpLib64")]
#else
[assembly: AssemblyTitle("Si13n7 Dev.™ CSharp Library")]
[assembly: AssemblyDescription("Si13n7 Dev.™ CSharp Library compiled for any platform environment")]
[assembly: AssemblyProduct("SilDev.CSharpLib")]
#endif

#if debug
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Si13n7 Dev.™")]
[assembly: AssemblyCopyright("Copyright © Si13n7 Dev.™ 2023")]
[assembly: AssemblyTrademark("Si13n7 Dev.™")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("901502cc-aa7d-444e-944a-7fc063c34917")]

[assembly: AssemblyVersion("23.12.20.0")]

[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
