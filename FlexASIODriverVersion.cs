using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using Microsoft.Win32;

namespace FlexASIOGUI
{
    internal static class FlexASIODriverVersion
    {
        public static string GetInstalledVersion()
        {
            try
            {
                using var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                return GetRegisteredVersion(localMachine);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException or ArgumentException)
            {
                return null;
            }
        }

        private static string GetRegisteredVersion(RegistryKey localMachine)
        {
            using var asio = localMachine.OpenSubKey(@"SOFTWARE\ASIO\FlexASIO");
            if (!Guid.TryParse(asio?.GetValue("CLSID") as string, out var classId))
                return null;

            using var server = localMachine.OpenSubKey($@"SOFTWARE\Classes\CLSID\{classId:B}\InprocServer32");
            var driverPath = (server?.GetValue(null) as string)?.Trim().Trim('"');
            if (string.IsNullOrEmpty(driverPath) || !Path.IsPathFullyQualified(driverPath) || !File.Exists(driverPath))
                return null;

            var version = FileVersionInfo.GetVersionInfo(driverPath).ProductVersion;
            // Older builds used a git description followed by the build timestamp.
            version = version?.Split(" built on ", 2, StringSplitOptions.None)[0].Trim();
            const string gitTagPrefix = "flexasio-";
            if (version?.StartsWith(gitTagPrefix, StringComparison.OrdinalIgnoreCase) == true)
                version = version.Substring(gitTagPrefix.Length);
            if (!string.IsNullOrWhiteSpace(version) && !version.Equals("unknown", StringComparison.OrdinalIgnoreCase))
                return version;

            using var installer = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FlexASIO_is1");
            var installLocation = (installer?.GetValue("InstallLocation") as string)?.Trim();
            if (string.IsNullOrEmpty(installLocation) || !Path.IsPathFullyQualified(installLocation))
                return null;

            var installedPath = Path.GetFullPath(Path.Combine(installLocation, "x64", "FlexASIO.dll"));
            if (!string.Equals(installedPath, Path.GetFullPath(driverPath), StringComparison.OrdinalIgnoreCase))
                return null;

            version = (installer.GetValue("DisplayVersion") as string)?.Trim();
            return string.IsNullOrEmpty(version) ? null : version;
        }
    }
}
