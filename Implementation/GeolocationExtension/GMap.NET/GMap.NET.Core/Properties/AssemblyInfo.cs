using System.Runtime.CompilerServices;

// internal visibility
[assembly:
#if DEBUG
    InternalsVisibleTo("ConsoleApplication"),
#endif
    InternalsVisibleTo("GMap.NET.WindowsForms"),
    InternalsVisibleTo("GMap.NET.WindowsMobile"),
    InternalsVisibleTo("GMap.NET.WindowsPresentation")]
