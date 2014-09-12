using System.Runtime.InteropServices;

namespace MyWPF.Utils
{
    public class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
