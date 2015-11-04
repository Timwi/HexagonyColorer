using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RT.Util;
using RT.Util.ExtensionMethods;
using RT.Util.Forms;

namespace HexagonyColorer
{
    [Settings("HexagonyColorer", SettingsKind.UserSpecific, SettingsSerializer.ClassifyJson)]
    sealed class HCSettings : SettingsBase
    {
        public ManagedForm.Settings FormSettings = new ManagedForm.Settings();
        public ManagedForm.Settings EditDialogSettings = new ManagedForm.Settings();

        public string LastDirectory;
        public string LastSourceDirectory;
    }
}
