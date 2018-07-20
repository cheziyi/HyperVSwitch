﻿using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("HyperVSwitch")]
[assembly: AssemblyTitle("HyperVSwitch")]
[assembly: AssemblyDescription("Hyper-V Switch")]
[assembly: AssemblyCopyright("© Calvin Che, GNU GPL v3")]
[assembly: AssemblyCompany("CCZY SYSTEMS")]

// Assembly identity version. Must be a dotted-numeric version.
[assembly: AssemblyVersion("0.1.0.0")]

// Repeat for Win32 file version resource because the assembly version is expanded to 4 parts.
[assembly: AssemblyFileVersion("0.1.0.0")]

// Informational version string, used for the About dialog, error reports and the setup script.
// Can be any freely formatted string containing punctuation, letters and revision codes.
//[assembly: AssemblyInformationalVersion("1.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Other attributes
[assembly: ComVisible(false)]
