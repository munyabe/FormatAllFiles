using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FormatAllFiles
{
    /// <summary>
    /// 出力ウィンドウの表示領域を表します。
    /// </summary>
    public class OutputWindow
    {
        /// <summary>
        /// 出力元の名前を取得します。
        /// </summary>
        public string SourceName { get; private set; }

        private IVsOutputWindowPane _pane;
        /// <summary>
        /// 出力ウィンドウのペインを取得します。
        /// </summary>
        private IVsOutputWindowPane Pane
        {
            get
            {
                if (_pane == null)
                {
                    var outputWindow = (IVsOutputWindow)Package.GetGlobalService(typeof(SVsOutputWindow));

                    var guidGeneral = Guid.NewGuid();
                    outputWindow.CreatePane(guidGeneral, SourceName, 1, 1);
                    outputWindow.GetPane(guidGeneral, out _pane);
                }

                return _pane;
            }
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="sourceName">出力元の名前</param>
        public OutputWindow(string sourceName)
        {
            SourceName = sourceName;
        }

        /// <summary>
        /// 出力を初期化します。
        /// </summary>
        public void Clear()
        {
            Pane.Clear();
        }

        /// <summary>
        /// 指定した文字列と改行を出力します。
        /// </summary>
        /// <param name="value">出力する文字列</param>
        public void WriteLine(string value)
        {
            var pane = Pane;
            pane.OutputString(value);
            pane.OutputString(Environment.NewLine);
        }
    }
}
