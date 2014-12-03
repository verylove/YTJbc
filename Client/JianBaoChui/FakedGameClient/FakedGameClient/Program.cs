using System;
using System.Windows.Forms;

namespace FakedGameClient
{
    static class Program
    {
        //public static Game Game { get; set; }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Properties.Settings.Default.AutoGame)
            {
                Application.Run(new FormAutoGame());
            }
            else
            {
                Application.Run(new FormGame());
            }

            //Game = new Game();
            //Game.Start();
            //Application.Run();
        }

        //public static void Exit()
        //{
        //    Application.Exit();
        //}
    }
}
