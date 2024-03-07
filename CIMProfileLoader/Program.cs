using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIMProfileLoader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CIMProfileLoaderForm());

            //SAVE GRAPH, DELETE GRAPH, UPDATE GRAPH, SPARQL QUERY BENCHMARK TIME+MEMORY
        }
    }
}
