using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RT.Util;
using RT.Util.Serialization;

namespace HexagonyColorer
{
    static class HCProgram
    {
        public static HCSettings Settings;

        [STAThread]
        static int Main(string[] args)
        {
            try { Console.OutputEncoding = Encoding.UTF8; }
            catch { }

            if (args.Length == 2 && args[0] == "--post-build-check")
                return Ut.RunPostBuildChecks(args[1], Assembly.GetExecutingAssembly());

            Classify.DefaultOptions.AddTypeOptions(typeof(Color), new ClassifyColorSubstitute());
            SettingsUtil.LoadSettings(out Settings);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Mainform(Settings));

            Settings.SaveLoud();
            return 0;
        }
    }
}
