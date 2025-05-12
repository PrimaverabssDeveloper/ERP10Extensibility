using System;
using System.Windows.Forms;

namespace primavera.extensibility.kits
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OpenForm();
        }

        static void OpenForm()
        {
            Application.Run(new frmMain());
        }
    static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyFullName;

            System.Reflection.AssemblyName assemblyName;

            assemblyName = new System.Reflection.AssemblyName(args.Name);

            assemblyFullName = System.IO.Path.Combine(GetPathPlataforma(), assemblyName.Name + ".dll");

            if (System.IO.File.Exists(assemblyFullName))
                return System.Reflection.Assembly.LoadFrom(assemblyFullName);
            else
                return null;
        }

        private static string GetPathPlataforma()
        {
            string path;

            if (System.Environment.GetEnvironmentVariable("PERCURSOSGP100") != null)
            {
                path = System.Environment.GetEnvironmentVariable("PERCURSOSGP100");
            }
            else
            {
                path = System.Environment.GetEnvironmentVariable("PERCURSOSGE100");
            }

            return string.IsNullOrEmpty(path) ? @"C:\Program Files\PRIMAVERA\SG100\Apl\" : path;
        }
    }
}
