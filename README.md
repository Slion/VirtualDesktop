# VirtualDesktop

.NET library enabling Windows multiple desktop operations

[![Windows 11](https://img.shields.io/badge/Windows_11-supported-dodgerblue)](https://www.microsoft.com/en-us/windows/learning-center/stay-organized-using-multiple-desktops-windows-11)
[![Windows 10](https://img.shields.io/badge/Windows_10-supported-dodgerblue)](https://support.microsoft.com/en-us/windows/configure-multiple-desktops-in-windows-36f52e38-5b4a-557b-2ff9-e1a60c976434)

[![Build](https://github.com/Slion/VirtualDesktop/workflows/Build/badge.svg)](https://github.com/Slion/VirtualDesktop/actions/workflows/build.yml)
[![Publish](https://github.com/Slion/VirtualDesktop/workflows/Publish/badge.svg)](https://github.com/Slion/VirtualDesktop/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/Slion/VirtualDesktop)](LICENSE)

| Platform | NuGet | Downloads |
| -- | -- |  -- |
| Core | [![NuGet Badge](https://img.shields.io/nuget/v/Slions.VirtualDesktop)](https://www.nuget.org/packages/Slions.VirtualDesktop/) | [![NuGet Downloads](https://img.shields.io/nuget/dt/Slions.VirtualDesktop)](https://www.nuget.org/packages/Slions.VirtualDesktop/) |
| Forms | [![NuGet Badge](https://img.shields.io/nuget/v/Slions.VirtualDesktop.WinForms)](https://www.nuget.org/packages/Slions.VirtualDesktop.WinForms/)| [![NuGet Downloads](https://img.shields.io/nuget/dt/Slions.VirtualDesktop.WinForms)](https://www.nuget.org/packages/Slions.VirtualDesktop.WinForms/) |
| WPF | [![NuGet Badge](https://img.shields.io/nuget/v/Slions.VirtualDesktop.WPF)](https://www.nuget.org/packages/Slions.VirtualDesktop.WPF/) | [![NuGet Downloads](https://img.shields.io/nuget/dt/Slions.VirtualDesktop.WPF)](https://www.nuget.org/packages/Slions.VirtualDesktop.WPF/) |


## Features

* Switch, add, and remove a virtual desktop.
* Move the window in the same process to any virtual desktop.
* Move the window of another process to any virtual desktop (Support in version 2.0 or later).
* Pin any window or application; will be display on all desktops.
* Notification for switching, deletion, renaming, etc.
* Change the wallpaper for each desktop.


### Sample app

![](https://user-images.githubusercontent.com/1779073/152605684-2d872356-1882-4bfd-821d-d4211ccac069.gif)
[samples/VirtualDesktop.Showcase](samples/VirtualDesktop.Showcase)


## Requirements

```xml
<TargetFramework>net6.0-windows10.0.19041.0</TargetFramework>
```
* .NET 6.0
* .NET 7.0
* .NET 8.0
* Windows 10 build 19041 (20H1) or later


## Installation

Install NuGet package(s).

```powershell
PM> Install-Package VirtualDesktop
```

* [VirtualDesktop](https://www.nuget.org/packages/Slions.VirtualDesktop/) - Core classes for VirtualDesktop.
* [VirtualDesktop.WPF](https://www.nuget.org/packages/Slions.VirtualDesktop.WPF/) - Provides extension methods for WPF [Window class](https://msdn.microsoft.com/en-us/library/system.windows.window(v=vs.110).aspx).
* [VirtualDesktop.WinForms](https://www.nuget.org/packages/Slions.VirtualDesktop.WinForms/) - Provides extension methods for [Form class](https://msdn.microsoft.com/en-us/library/system.windows.forms.form(v=vs.110).aspx).


## How to use

### Preparation
Because of the dependency on [C#/WinRT](https://aka.ms/cswinrt) ([repo](https://github.com/microsoft/CsWinRT)), the target framework must be set to `net5.0-windows10.0.19041.0` or later.
```xml
<TargetFramework>net5.0-windows10.0.19041.0</TargetFramework>
```
```xml
<TargetFramework>net6.0-windows10.0.19041.0</TargetFramework>
```

If it doesn't work, try creating an `app.manifest` file and optimize to work on Windows 10.
```xml
<compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
    <application>
	    <!-- Windows 10 / 11-->
	    <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
    </application>
</compatibility>
```

The namespace to use is `WindowsDesktop`.
```csharp
using WindowsDesktop;
```

### Get instance of VirtualDesktop class
```csharp 
// Get all virtual desktops
var desktops = VirtualDesktop.GetDesktops();

// Get Virtual Desktop for specific window
var desktop = VirtualDesktop.FromHwnd(hwnd);

// Get the left/right desktop
var left  = desktop.GetLeft();
var right = desktop.GetRight();
```

### Manage virtual desktops
```csharp
// Create new
var desktop = VirtualDesktop.Create();

// Remove
desktop.Remove();

// Switch
desktop.GetLeft().Switch();
```

### Subscribe virtual desktop events
```csharp
// Notification of desktop switching
VirtualDesktop.CurrentChanged += (_, args) => Console.WriteLine($"Switched: {args.NewDesktop.Name}");

// Notification of desktop creating
VirtualDesktop.Created += (_, desktop) => desktop.Switch();
```

### for WPF window
```csharp
// Need to install 'VirtualDesktop.WPF' package

// Check whether a window is on the current desktop.
var isCurrent = window.IsCurrentVirtualDesktop();

// Get Virtual Desktop for WPF window
var desktop = window.GetCurrentDesktop();

// Move window to specific Virtual Desktop
window.MoveToDesktop(desktop);

// Pin window
window.Pin()
```

### Windows version support

Since this library is using undocumented interfaces you need to properly [reverse engineer](https://github.com/Slion/VirtualDesktop/issues/14) your Windows version to support it.

The class IDs of some of the undocumented interfaces we use tend to change a lot between different versions of Windows.
If the demo application crashes on start-up chances are all you need to do is provide the proper IDs for the version of Windows you are running on.

Here are the interfaces we need:

- `IApplicationView`
- `IApplicationViewCollection`
- `IObjectArray`
- `IServiceProvider`
- `IVirtualDesktop`
- `IVirtualDesktopManager`
- `IVirtualDesktopManagerInternal`
- `IVirtualDesktopNotification`
- `IVirtualDesktopNotificationService`
- `IVirtualDesktopPinnedApps`

Once you have the IDs add them in a new `setting` element in [app.config].
Make sure to specify the correct 5 digits Windows build version.
You can get it using one of those methods:
- From the UI run: `winver`
- From shell run: `ver`
- From powershell run: `cmd /c ver`

Make sure to contribute back your changes.

## Publish

To publish a new release specify your version in [Directory.Build.props] and push the changes with a commit description such as:
`Release vx.y.z` where `x`, `y`, `z` form your version number. That should publish it on NuGet providing that your secret `NUGET_API_KEY` is still valid.

## Internals

Essentially a C# wrapper for [IVirtualDesktopManager](https://msdn.microsoft.com/en-us/library/windows/desktop/mt186440%28v%3Dvs.85%29.aspx) and related undocumented interfaces.
In order to support breaking binary changes between Windows versions we perform runtime compilation of a DLL providing access to the COM interfaces matching your OS build version.

## Resources
* [samples/README.md](samples/README.md)
* [StackOverflow](https://stackoverflow.com/questions/32416843/programmatic-control-of-virtual-desktops-in-windows-10)
* [Upstream repository](https://github.com/Grabacr07/VirtualDesktop) - unmaintained
* [VirtualDesktop command line tool](https://github.com/MScholtes/VirtualDesktop) - not using this library
* [VirtualDesktop AutoHotKey DLL](https://github.com/Ciantic/VirtualDesktopAccessor)

## License

This library is under [the MIT License](https://github.com/Grabacr07/VirtualDesktop/blob/master/LICENSE).


[app.config]: src/VirtualDesktop/app.config
[Directory.Build.props]: src/Directory.Build.props

## Credits

* Thanks [@Grabacr07](https://github.com/Grabacr07) for creating this great piece of software
* All contributors for sharing your work with the community