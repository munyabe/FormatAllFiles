using System;
using System.Runtime.InteropServices;
using FormatAllFiles.Options;
using Microsoft.VisualStudio.Shell;

namespace FormatAllFiles
{
    /// <summary>
    /// 拡張機能として配置されるパッケージです。
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GeneralOptionPage), PackageName, "General", 100, 101, true, new string[] { PackageName, "Option" })]
    public sealed class FormatAllFilesPackage : Package
    {
        /// <summary>
        /// パッケージの名前です。
        /// </summary>
        public const string PackageName = "Format All Files";

        /// <summary>
        /// パッケージのIDです。
        /// </summary>
        public const string PackageGuidString = "78f3c948-c9f1-4387-8fa6-7177d99b483b";

        /// <summary>
        /// パッケージを初期化します。
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            FormatAllFilesCommand.Initialize(this);
        }
    }
}