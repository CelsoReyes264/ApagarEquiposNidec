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

namespace ApagarEquipos
{
    public partial class Form1 : Form
    {
        private const int WM_SYSCOMMAND = 0x112; //en VB &H112&
        private const int SC_MONITORPOWER = 0xF170; //en BV &HF170&
        private const int MOUSEEVENTF_MOVE = 0x0001;

        [DllImport("user32.dll")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
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
            //El 2 es para apagar
        }

        private void BtnApagarPantalla_Click(object sender, EventArgs e)
        {
            ApagarMonitor();
        }

        private void btnApagarPC_Click(object sender, EventArgs e)
        {
            ApagarPC(); 
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Al iniciar el form ejecuta el metodo, revisar para hacerlo a cierta hora.
            //ApagarMonitor();
            //ApagarPC();
        }

        private void tmEncender_Tick(object sender, EventArgs e)
        {
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);

        }
    }
}
