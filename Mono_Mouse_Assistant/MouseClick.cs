using System.Runtime.InteropServices;
using System.Diagnostics;
/*
 * Linux need: sudo apt-get install xdotool
 */
public class MouseClicker
{

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;

    //This simulates a left mouse click
    private static void LeftMouseClickWindows(int xpos, int ypos)
    {
        //SetCursorPos(xpos, ypos);
        mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
    }
    public static void click(int x, int y)
    {


        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            LeftMouseClickLinux(x, y);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            LeftMouseClickWindows(x, y);
        }


    }

    public static void LeftMouseClickLinux(int xpos,int ypos)
    {
        // xdotool mousemove 100 200 click 1 
        string args = string.Format("mousemove {0} {1} click {2}", xpos, ypos, 1);
        var proc = new Process
        {
            StartInfo =
                               {
                                       FileName = "xdotool",
                                       Arguments = args,
                                       UseShellExecute = false,
                                       RedirectStandardError = false,
                                       RedirectStandardInput = false,
                                       RedirectStandardOutput = false
                               }
        };
        proc.Start();
    }
}
