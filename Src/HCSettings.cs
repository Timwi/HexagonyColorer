using RT.Util;
using RT.Util.Forms;

namespace HexagonyColorer
{
    [Settings("HexagonyColorer", SettingsKind.UserSpecific, SettingsSerializer.ClassifyJson)]
    sealed class HCSettings : SettingsBase
    {
        public ManagedForm.Settings FormSettings = new ManagedForm.Settings();
        public ManagedForm.Settings EditDialogSettings = new ManagedForm.Settings();

        public int[] CustomColorsData = null;

        public string LastDirectory;
        public string LastSourceDirectory;
    }
}
