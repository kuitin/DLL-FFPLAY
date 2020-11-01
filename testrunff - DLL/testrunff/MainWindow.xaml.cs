using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using static testrunff.ControlHost;

namespace testrunff
{
    public class ControlHostDisplayer : HwndHost
    {
        [DllImport("example_dll.dll")]
        public static extern int Double(int arg1);

        [DllImport("example_dll.dll")]
        public static extern IntPtr StartFFPlay(int argc, IntPtr arg, int imgCount, IntPtr imgPartRect);

        [DllImport("example_dll.dll")]
        public static extern IntPtr show(int windows);
        [DllImport("example_dll.dll")]
        public static extern int GetIsVideoReadyToShow();
        [DllImport("example_dll.dll")]
        public static extern void KillVideo();

        IntPtr hwndControl;
        IntPtr hwndHost;
        int hostHeight, hostWidth;
        private Process _process;
        private Process _process2;
        public string pathApp = "";

        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        internal const int
          WS_CHILD = 0x40000000,
          WS_VISIBLE = 0x10000000,
          LBS_NOTIFY = 0x00000001,
          HOST_ID = 0x00000002,
          LISTBOX_ID = 0x00000001,
          WS_VSCROLL = 0x00200000,
          WS_BORDER = 0x00800000;
        private const int WS_MAXIMIZEBOX = 0x10000,
                     WS_MINIMIZEBOX = 0x20000;

        const UInt32 WS_MAXIMIZE = 0x1000000;

        private int _videoID = 2;  // the name field
        public int VideoID    // the Name property
        {
            get => _videoID;
            set => _videoID = value;
        }




        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);


        public ControlHostDisplayer(double height, double width)
        {
            hostHeight = (int)height;
            hostWidth = (int)width;
        }

        private HandleRef _hwndParent;
        public void resize()
        {
            //MoveWindow(_process.MainWindowHandle, 0,0, 200, 200, true);
            RECT Rect = new RECT();
            if (GetWindowRect(_hwndParent.Handle, ref Rect))
            {
                ControlHost.MoveWindow(_process.MainWindowHandle, Rect.left, Rect.right, Rect.right - Rect.left, Rect.bottom - Rect.top, true);
                this.InvalidateVisual();
            }

        }

        //public IntPtr GetSDLWin()
        //{
        //    return show(1);
        //}


        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            _hwndParent = hwndParent;
            //ProcessStartInfo psi = new ProcessStartInfo("C:\\Users\\Utilisateur\\Downloads\\ffmpeg-20200417-889ad93-win64-static\\bin\\ffplay.exe C:\\Users\\Utilisateur\\Desktop\\sdptest.sdp -x 300 -y 400 -protocol_whitelist file,udp,rtp -vcodec h264_cuvid -delay 0  -crf 0 -preset losslesshp -tune zerolatency");
            //ProcessStartInfo psi = new ProcessStartInfo("C:\\Users\\Utilisateur\\Downloads\\ffmpeg-4.2.2-win64-static-lgpl\\bin\\ffplay.exe");
            //// ProcessStartInfo psi = new ProcessStartInfo("C:\\Program Files\\VideoLAN\\VLC\\vlc.exe");
            //psi.Arguments = "udp://127.0.0.1:5354 -x 700 -y 800 -protocol_whitelist file,udp,rtp,tcp";
            //psi.CreateNoWindow = true;
            //psi.RedirectStandardOutput = true;
            //psi.UseShellExecute = false;
            //psi.WindowStyle = ProcessWindowStyle.Minimized;
            //_process = Process.Start(psi);
            //_process.WaitForInputIdle();

            // The main window handle may be unavailable for a while, just wait for it


            IntPtr notepadHandle = show(_videoID);

            int style = ControlHost.GetWindowLong(notepadHandle, GWL_STYLE);
            style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
            style |= ((int)WS_CHILD); // Must be a child window to be hosted
            //style |= ((int)WS_MAXIMIZE);
            style |= ((int)WS_MAXIMIZEBOX);
            style |= ((int)WS_MINIMIZEBOX);
            ControlHost.SetWindowLong(notepadHandle, GWL_STYLE, style);
            ControlHost.SetParent(notepadHandle, hwndParent.Handle);
            RECT Rect = new RECT();
            if (GetWindowRect(hwndParent.Handle, ref Rect))
                ControlHost.MoveWindow(notepadHandle, Rect.left, Rect.right, Rect.right - Rect.left, Rect.bottom - Rect.top, true);
            //MoveWindow(notepadHandle, 0, 0, 1025, 950, true);
            this.InvalidateVisual();

            HandleRef hwnd = new HandleRef(this, notepadHandle);
            return hwnd;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
        }

       
      

    
        public static IntPtr NativeUtf8FromString(string managedString)
        {
            int len = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }
    }
    public class ControlHost : HwndHost
    {
        [DllImport("example_dll.dll")]
        public static extern int Double(int arg1);

        [DllImport("example_dll.dll")]
        public static extern IntPtr StartFFPlay(int argc, IntPtr arg, int imgCount, IntPtr imgPartRect);

        [DllImport("example_dll.dll")]
        public static extern IntPtr show(int windows);
        [DllImport("example_dll.dll")]
        public static extern int GetIsVideoReadyToShow();
        [DllImport("example_dll.dll")]
        public static extern void KillVideo();

        IntPtr hwndControl;
        IntPtr hwndHost;
        int hostHeight, hostWidth;
        private Process _process;
        private Process _process2;
        public string pathApp = "";

        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        internal const int
          WS_CHILD = 0x40000000,
          WS_VISIBLE = 0x10000000,
          LBS_NOTIFY = 0x00000001,
          HOST_ID = 0x00000002,
          LISTBOX_ID = 0x00000001,
          WS_VSCROLL = 0x00200000,
          WS_BORDER = 0x00800000;
        private const int WS_MAXIMIZEBOX = 0x10000,
                     WS_MINIMIZEBOX = 0x20000;

        const UInt32 WS_MAXIMIZE = 0x1000000;

        private int _videoID = 1;  // the name field
        public int VideoID    // the Name property
        {
            get => _videoID;
            set => _videoID = value;
        }
    

    [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);


        public ControlHost(double height, double width)
        {
            hostHeight = (int)height;
            hostWidth = (int)width;
        }

        private HandleRef _hwndParent;
        public void resize()
        {
            //MoveWindow(_process.MainWindowHandle, 0,0, 200, 200, true);
            RECT Rect = new RECT();
            if (GetWindowRect(_hwndParent.Handle, ref Rect))
            {
                MoveWindow(_process.MainWindowHandle, Rect.left, Rect.right, Rect.right - Rect.left, Rect.bottom - Rect.top, true);
                this.InvalidateVisual();
            }

        }

        //public IntPtr GetSDLWin()
        //{
        //    return show(1);
        //}
        

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            _hwndParent = hwndParent;
            //ProcessStartInfo psi = new ProcessStartInfo("C:\\Users\\Utilisateur\\Downloads\\ffmpeg-20200417-889ad93-win64-static\\bin\\ffplay.exe C:\\Users\\Utilisateur\\Desktop\\sdptest.sdp -x 300 -y 400 -protocol_whitelist file,udp,rtp -vcodec h264_cuvid -delay 0  -crf 0 -preset losslesshp -tune zerolatency");
            //ProcessStartInfo psi = new ProcessStartInfo("C:\\Users\\Utilisateur\\Downloads\\ffmpeg-4.2.2-win64-static-lgpl\\bin\\ffplay.exe");
            //// ProcessStartInfo psi = new ProcessStartInfo("C:\\Program Files\\VideoLAN\\VLC\\vlc.exe");
            //psi.Arguments = "udp://127.0.0.1:5354 -x 700 -y 800 -protocol_whitelist file,udp,rtp,tcp";
            //psi.CreateNoWindow = true;
            //psi.RedirectStandardOutput = true;
            //psi.UseShellExecute = false;
            //psi.WindowStyle = ProcessWindowStyle.Minimized;
            //_process = Process.Start(psi);
            //_process.WaitForInputIdle();

            // The main window handle may be unavailable for a while, just wait for it
         

            IntPtr notepadHandle = show(_videoID);

            int style = GetWindowLong(notepadHandle, GWL_STYLE);
            style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
            style |= ((int)WS_CHILD); // Must be a child window to be hosted
            //style |= ((int)WS_MAXIMIZE);
            style |= ((int)WS_MAXIMIZEBOX);
            style |= ((int)WS_MINIMIZEBOX);
            SetWindowLong(notepadHandle, GWL_STYLE, style);
            SetParent(notepadHandle, hwndParent.Handle);
            RECT Rect = new RECT();
            if (GetWindowRect(hwndParent.Handle, ref Rect))
                MoveWindow(notepadHandle, Rect.left, Rect.right, Rect.right - Rect.left, Rect.bottom - Rect.top , true);
            //MoveWindow(notepadHandle, 0, 0, 1025, 950, true);
            this.InvalidateVisual();

            HandleRef hwnd = new HandleRef(this, notepadHandle);
            return hwnd;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            KillVideo();
            //_process.Close();
            //if (_process.MainWindowHandle != IntPtr.Zero)
            //_process.Kill();
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            // _process.Close();
            //if(isrunnig == true)
            //     _process.Kill();            
            KillVideo();
            isrunnig = false;
        }
        bool isrunnig = false;
        public void Play()
        {
            Task t1 = new Task(action);
            t1.Start();
            while (0 == GetIsVideoReadyToShow())
            {
                Thread.Sleep(2);
            }
          
            isrunnig = true;
        }

        private void action()
        {
            //"-protocol_whitelist file,udp,rtp -sync ext -fflags nobuffer -framedrop -f h264 -vcodec h264_cuvid -framerate 29.97  -an -i udp://127.0.0.1:5354 ";
            string[] command = new string[15];
            int i = 0;
            command[i] = "";
            command[++i] = "-protocol_whitelist";
            command[++i] = "file,udp,rtp";
            command[++i] = "-sync";
            command[++i] = "ext";
            command[++i] = "-fflags";
            command[++i] = "nobuffer";
            command[++i] = "-framedrop";
            command[++i] = "-f";
            command[++i] = "h264";
            command[++i] = "-vcodec";
            command[++i] = "h264_cuvid";
            command[++i] = "-an";
            command[++i] = "-i";
            command[++i] = "udp://127.0.0.1:5354";
            //command[++i] = "-vf";
            //command[++i] = "crop = 1920:1080:0:0";
            List<IntPtr> allocatedMemory = new List<IntPtr>();

            int sizeOfIntPtr = Marshal.SizeOf(typeof(IntPtr));
            IntPtr pointersToArguments = Marshal.AllocHGlobal(sizeOfIntPtr * command.Length);
            

            for (int itr = 0; itr < command.Length; ++itr)
            {
                IntPtr pointerToArgument = NativeUtf8FromString(command[itr]);
                allocatedMemory.Add(pointerToArgument);
                Marshal.WriteIntPtr(pointersToArguments, itr * sizeOfIntPtr, pointerToArgument);
            }
            int imageCount = 2;
            IntPtr pointersToSize = Marshal.AllocHGlobal(sizeOfIntPtr * imageCount);
            //for (int itr = 0; itr < imageCount; ++itr)
            //{
                int[] result = new int[4];
                result[0] = 0;
                result[1] = 0;
                result[2] = 1920;
                result[3] = 1080;
                // Initialize unmanaged memory to hold the array.
                int size = Marshal.SizeOf(result[0]) * result.Length;

                IntPtr pnt = Marshal.AllocHGlobal(size);
                Marshal.Copy(result, 0, pnt, result.Length);

                Marshal.WriteIntPtr(pointersToSize, 0 * sizeOfIntPtr, pnt);
            //}

            result = new int[4];
            result[0] = 1000;
            result[1] = 1000;
            result[2] = 1920;
            result[3] = 1080;
            // Initialize unmanaged memory to hold the array.
            size = Marshal.SizeOf(result[0]) * result.Length;

            pnt = Marshal.AllocHGlobal(size);
            Marshal.Copy(result, 0, pnt, result.Length);

            Marshal.WriteIntPtr(pointersToSize, 1 * sizeOfIntPtr, pnt);

            var temp = StartFFPlay(command.Length, pointersToArguments, imageCount, pointersToSize);
            string ansi = Marshal.PtrToStringAnsi(temp);

            Marshal.FreeHGlobal(pointersToArguments);

            foreach (IntPtr pointer in allocatedMemory)
            {
                Marshal.FreeHGlobal(pointer);
            }
        }
        public static IntPtr NativeUtf8FromString(string managedString)
        {
            int len = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }
    }
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ControlHost listControl;
        ControlHostDisplayer listControl2;
        int selectedItem;
        IntPtr hwndListBox;
        Application app;
        Window myWindow;
        int itemCount;

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public System.Windows.Forms.Panel pannel;

        public MainWindow()
        {
            InitializeComponent();
            //xxxFFplay();
        }
        private void On_UIReady(object sender, EventArgs e)
        {
            app = System.Windows.Application.Current;
            myWindow = app.MainWindow;
            myWindow.SizeToContent = SizeToContent.WidthAndHeight;
            
         //   listControl.xxxFFplay();
            //ControlHostElement.Child = listControl;
          
        }

        public Process ffplay = new Process();

        public void xxxFFplay()
        {
            // start ffplay 
            /*var ffplay = new Process
            {
                StartInfo =
                {
                    FileName = "ffplay.exe",
                    Arguments = "Revenge.mp4",
                    // hides the command window
                    CreateNoWindow = true,
                    // redirect input, output, and error streams..
                    //RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
             * */
            //public Process ffplay = new Process();
            ffplay.StartInfo.FileName = "C:\\Users\\Utilisateur\\Downloads\\ffmpeg-20200504-5767a2e-win64-static\\bin\\ffplay.exe";
            ffplay.StartInfo.Arguments = "udp://127.0.0.1:5354 -x 300 -y 400 -protocol_whitelist file,udp,rtp,tcp";
            ffplay.StartInfo.CreateNoWindow = true;
            ffplay.StartInfo.RedirectStandardOutput = true;
            ffplay.StartInfo.UseShellExecute = false;

            ffplay.EnableRaisingEvents = true;
            ffplay.OutputDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            ffplay.ErrorDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            ffplay.Exited += (o, e) => Debug.WriteLine("Exited", "ffplay");
            ffplay.Start();

            Thread.Sleep(500); // you need to wait/check the process started, then...

            // child, new parent
            // make 'this' the parent of ffmpeg (presuming you are in scope of a Form or Control)
            //SetParent(ffplay.MainWindowHandle, ControlHostElement.Child);

            // window, x, y, width, height, repaint
            // move the ffplayer window to the top-left corner and set the size to 320x280
            MoveWindow(ffplay.MainWindowHandle, 0, 0, 320, 280, true);

        }

        private void PlayFFplay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopFFplay_Click(object sender, RoutedEventArgs e)
        {
            listControl.Stop();
        }

        private void TabItem_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
        bool isrunning = false;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(videoTab.IsSelected )
            {
                Console.WriteLine("videotab selected");
                listControl = new ControlHost(0, 0);
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();

                //var listControl2 = new ControlHost(ControlHostElement2.ActualHeight, ControlHostElement2.ActualWidth);
                //BackgroundWorker worker2 = new BackgroundWorker();
                //worker2.WorkerReportsProgress = true;
                //worker2.DoWork += worker_DoWork2;
                //worker2.RunWorkerCompleted += worker_RunWorkerCompleted2;
                //worker2.RunWorkerAsync(argument: listControl2);

                isrunning = true;
            }
            else
            {
                if (listControl != null)
                {
                    listControl2.Dispose();
                    // Stop ffplay aplication
                    listControl.Stop();
                    //Dispose the border of its child
                    listControl.Dispose();
                }
            }
        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            var listControlc = (ControlHost)e.Result;
            ControlHostElement2.Child = listControlc; 
        }

        private void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
           var listControlc = (ControlHost) e.Argument;
            listControlc.Play();
            e.Result = listControlc;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ControlHostElement.Child = listControl;
            listControl2 = new ControlHostDisplayer(0,0);

            ControlHostElement3.Child = listControl2;
            //SetParent(listControl.GetSDLWin(), ControlHostElement.Child.han);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            listControl.Play();
        }

        private void ControlHostElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // listControl.resize();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (listControl != null)
            {
                listControl2.Dispose();
                // Stop ffplay aplication
                listControl.Stop();
                //Dispose the border of its child
                listControl.Dispose();
                
            }
        }
    }
}
