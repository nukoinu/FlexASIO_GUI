This is a small GUI to make the configuration of https://github.com/dechamps/FlexASIO a bit quicker.

[日本語の使い方ガイド](USAGE.ja.md)

It should pick up your existing $Usersprofile/FlexASIO.toml file and read the basic parameters. Not all of them have been implemented yet...

The x64 installer includes the .NET 11 Desktop Runtime. To run a regular build from source, install the [x64 .NET 11 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/11.0) or the x64 .NET 11 SDK.

Run the following command from the repository root with the .NET 11 SDK to publish a self-contained x64 build.

```powershell
dotnet publish FlexASIOGUI.csproj -c Release -p:Platform=x64 -r win-x64 --self-contained true
```

Run `FlexASIOGUI.exe` from `bin\x64\Release\net11.0-windows\win-x64\publish` with the accompanying files. To create the installer, publish first, then compile `installer\FlexASIOGUI.iss` with Inno Setup 6.3 or later.

v0.3 adds a registry key with the install path to HKEY_LOCAL_MACHINE\SOFTWARE\Fabrikat\FlexASIOGUI\Install\Installpath

It also makes most settings optional so that default settings are not overwritten.

![image](https://user-images.githubusercontent.com/6930367/118895016-a4746a80-b905-11eb-806c-7fd3fee4fcd1.png)

