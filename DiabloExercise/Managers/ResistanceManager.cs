using System.Text;

public class ResistanceManager : IResistance //, IDisposable
{
    int _resistMagic;
    int _resistFire;
    int _resistLightning;

    // max je 100% i treba da bude capovan na to
    public int ResistMagic => Math.Clamp(_resistMagic, 0, 100);
    public int ResistFire => Math.Clamp(_resistFire, 0, 100);
    public int ResistLightning => Math.Clamp(_resistLightning, 0, 100);


    public void ModifyMagicResistance(int value)
    {
        _resistMagic += value;
    }

    public void ModifyFireResistance(int value)
    {
        _resistFire += value;
    }

    public void ModifyLightningResistance(int value)
    {
        _resistLightning += value;
    }

    Func<int, string> ModifyValueToString = resistancePower => resistancePower.ToString() + "%";

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();


        string resistMagic = ResistMagic >= 100 ? "Max" : ModifyValueToString(ResistMagic);
        string resistFire = ResistFire >= 100 ? "Max" : ModifyValueToString(ResistFire);
        string resistLightning = ResistLightning >= 100 ? "Max" : ModifyValueToString(ResistLightning);

        sb.AppendLine($"Resist Magic: {resistMagic}");
        sb.AppendLine($"Resist Fire: {resistFire}");
        sb.AppendLine($"Resist Lightning: {resistLightning}");
        return sb.ToString().Trim();
    }

}