using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace ApagarEquipos
{
    public partial class Form1 : Form
    {
        private const int WM_SYSCOMMAND = 0x112; //en VB &H112&
        private const int SC_MONITORPOWER = 0xF170; //en BV &HF170&
        private const int MOUSEEVENTF_MOVE = 0x0001;
        //DateTime Almuerzo = new DateTime(2021, 01, 26, 10, 5, 0);
        //DateTime RetornoAlmuerzo = new DateTime(2021, 01, 26, 10, 15, 0);
        //public const string HoraAlmuerzo = "2022-01-26 09:10:00.000";
        //public const string HoraRetornoA = "2022-01-26 10:11:00.000";

        [DllImport("user32.dll")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);



        private void ApagarPC()
        {
            Process.Start("shutdown.exe", "-s -f");
            // s se usará para cerrar un ordenador
            // f si utilizamos esta extensión todos los programas activos se cerrarán automáticamente.
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void ApagarMonitor()
        {

            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
            tmEncender.Enabled = true;
            tmEncender.Interval = 60000;
        }

        private void BtnApagarPantalla_Click(object sender, EventArgs e)
        {
            ApagarMonitor();
        }

        private void btnApagarPC_Click(object sender, EventArgs e)
        {
            ApagarPC(); 
        }

        private static void MonitorOn()
        {
            Form frm = new Form();
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
            SendMessage(frm.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tmEncender_Tick(object sender, EventArgs e)
        {
            MonitorOn();
        }

        private void tmApagarMonitor_Tick(object sender, EventArgs e)
        {
            ApagarMonitor();
        }
    }
}
