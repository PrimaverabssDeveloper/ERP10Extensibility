using System.Reflection;
using System.Resources;

[assembly: AssemblyCompany("PRIMAVERA Business Software Solutions, SA")]
[assembly: AssemblyProduct("PRIMAVERA ERP v10.0")]
[assembly: AssemblyVersion("10.0.0.0")]
[assembly: AssemblyFileVersion("10.0010.0000.0000")]
[assembly: AssemblyCopyright("Copyright © PRIMAVERA BSS, SA")]
[assembly: AssemblyTrademark("PRIMAVERA BSS")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("pt-PT")]

#if DEBUG
[assembly: AssemblyKeyFile("c:\\PRIMAVERA_ERP10.Public.snk")]
[assembly: AssemblyDelaySign(true)]
#else
    [assembly: AssemblyKeyName("PRIMAVERA_ERP10")]
    [assembly: AssemblyDelaySign(false)]
#endif