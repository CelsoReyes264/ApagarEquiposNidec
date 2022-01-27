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
using System.Globalization;
using System.Data.SqlClient;
using ApagarEquipos.DAO;

namespace ApagarEquipos
{
    public partial class Form1 : Form
    {
        private const int WM_SYSCOMMAND = 0x112; //en VB &H112&
        private const int SC_MONITORPOWER = 0xF170; //en BV &HF170&
        private const int MOUSEEVENTF_MOVE = 0x0001;

        //public const string HoraAlmuerzo = "12:55:00";
        //public const string HoraRetornoA = "2022-01-26 10:11:00.000";

        [DllImport("user32.dll")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        public static DateTime HoraActual = DateTime.Now;
        public static DateTime HoraAlmuerzo = new DateTime(2022, 01, 27, 08, 36, 00);
        public static DateTime EntradaAlmuerzo = new DateTime(2022, 01, 27, 08, 37, 00);
        public static DateTime Tiempo = new DateTime();
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
        //APAGA POR COMPLETO PC
        private void ApagarPC()
        {
            Process.Start("shutdown.exe", "-s -f");
        }

        public Form1()
        {
            InitializeComponent();
        }


        private void ApagarMonitor()
        {
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }

        private void BtnApagarPantalla_Click(object sender, EventArgs e)
        {
            ApagarMonitor();
        }

        private void btnApagarPC_Click(object sender, EventArgs e)
        {
            ApagarPC();
        }

        private static void EncenderMonitor()
        {
            Form frm = new Form();
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsultaHora();
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (short)Tiempo.Year; // must be short
            st.wMonth = (short)Tiempo.Month;
            st.wDay = (short)Tiempo.Day;
            st.wDayOfWeek = (short)Tiempo.DayOfWeek;
            st.wHour = (short)Tiempo.Hour;
            st.wMinute = (short)Tiempo.Minute;
            st.wSecond = (short)Tiempo.Second;
            st.wMilliseconds = (short)Tiempo.Millisecond
            SetSystemTime(ref st);
        } 

        private void tmObtenerTiempo_Tick(object sender, EventArgs e)
        {
             ChecarTiempos();
        }

        private void ChecarTiempos()
        {
            //aqui va a ir la funcion para traer hora servidor
            HoraActual = DateTime.Now;
            if (HoraActual >= HoraAlmuerzo &&  HoraActual < EntradaAlmuerzo)
            {
                while (HoraActual < EntradaAlmuerzo)
                {
                    //aqui va a ir la funcion para traer hora servidor
                    HoraActual = DateTime.Now;
                    ApagarMonitor();
                }
                //i++;
                EncenderMonitor();
            }
            else
            {
                //aqui hay que hacer algo

            }
        }

        public void ConsultaHora()
        {
            
            SqlConnection conn = new SqlConnection("Data Source=REY2MXNDSPPAP04;Initial Catalog=SIM;Persist Security Info=True;User ID=Developer;Password=Ac45035350");
            conn.Open();

            SqlCommand command = new SqlCommand("exec [Ingenieria].[ObtenerTiempo]", conn);
            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Tiempo = Convert.ToDateTime(reader["Tiempo"]);
                }
            }
            conn.Close();
        }

    }
}
