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
using Microsoft.Win32;

namespace ApagarEquipos
{
    public partial class Form1 : Form
    {
        private const int WM_SYSCOMMAND = 0x112; //en VB &H112&
        private const int SC_MONITORPOWER = 0xF170; //en BV &HF170&
        private const int MOUSEEVENTF_MOVE = 0x0001;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        [DllImport("user32.dll")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        public static DateTime HoraActual = DateTime.Now;

        #region Horarios
        public static DateTime HoraAlmuerzoConvertida = DateTime.ParseExact("12:30", "H:mm", null, System.Globalization.DateTimeStyles.None);
        public static DateTime HoraEntradaAlmuerzo = DateTime.ParseExact("12:35", "H:mm", null, System.Globalization.DateTimeStyles.None);
        public static DateTime HoraComidaConvertida= DateTime.ParseExact("14:20", "H:mm", null, System.Globalization.DateTimeStyles.None);
        public static DateTime HoraEntradaComida = DateTime.ParseExact("15:00", "H:mm", null, System.Globalization.DateTimeStyles.None);
        public static DateTime Tiempo = new DateTime();
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        private void ApagarPC()
        {
            Process.Start("shutdown.exe", "-s -f");
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
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
        }

        private void tmObtenerTiempo_Tick(object sender, EventArgs e)
        {
            ChecarTiempos();
        }

        private void ChecarTiempos()
        {
            HoraActual = DateTime.Now;
            if (HoraActual >= HoraAlmuerzoConvertida && HoraActual < HoraEntradaAlmuerzo)
            {
                while (HoraActual < HoraEntradaAlmuerzo)
                {
                    HoraActual = DateTime.Now;
                    ApagarMonitor();
                }
                EncenderMonitor();
            }
            else if (HoraActual >= HoraComidaConvertida && HoraActual < HoraEntradaComida)
            {
                while (HoraActual < HoraEntradaComida)
                {
                    HoraActual = DateTime.Now;
                    ApagarMonitor();
                }
                EncenderMonitor();
            }
        }

        public void ConsultaHora()
        {
            SqlConnection conn = new SqlConnection("Data Source=REY2MXNDSPPAP04;Initial Catalog=SIM;Persist Security Info=True;User ID=Developer;Password=Ac45035350");
            conn.Open();

            SqlCommand command = new SqlCommand("exec [Ingenieria].[ObtenerTiempo]", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Tiempo = Convert.ToDateTime(reader["Tiempo"]);
                }
            }
            conn.Close();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var regKeyGeoId = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Control Panel\International\Geo");
            var geoID = (string)regKeyGeoId.GetValue("Nation");
            var allRegions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.ToString()));
            var regionInfo = allRegions.FirstOrDefault(r => r.GeoId == Int32.Parse(geoID));

            if(regionInfo.Name == "es-MX")
            {
                ConsultaHora();
                var x = TimeZoneInfo.ConvertTimeToUtc(Tiempo);
                SYSTEMTIME st = new SYSTEMTIME();
                st.wYear = (ushort)x.Year;
                st.wMonth = (ushort)x.Month;
                st.wDay = (ushort)x.Day;
                st.wHour = (ushort)x.Hour;
                st.wMinute = (ushort)x.Minute;
                st.wSecond = (ushort)x.Second;
                SetSystemTime(ref st);
                tmObtenerTiempo.Enabled = true;
                tmObtenerTiempo.Interval = 100;
            }
            else if(regionInfo.Name == "chr-Cher-US")
            {
                ConsultaHora();
                var x = TimeZoneInfo.ConvertTimeToUtc(Tiempo);
                SYSTEMTIME st = new SYSTEMTIME();
                st.wYear = (ushort)x.Year;
                st.wMonth = (ushort)x.Month;
                st.wDay = (ushort)x.Day;
                st.wHour = (ushort)x.Hour;
                st.wMinute = (ushort)x.Minute;
                st.wSecond = (ushort)x.Second;
                SetSystemTime(ref st);
                tmObtenerTiempo.Enabled = true;
                tmObtenerTiempo.Interval = 100;
            }
        } 
    }
}
