using System.Text;

public class ResistanceManager : IResistance, IDisposable
{
    int _resistMagic;
    int _resistFire;
    int _resistLightning;

    // max je 100% i treba da bude capovan na to
    public int ResistMagic => Math.Clamp(_resistMagic, 0, 100);
    public int ResistFire => Math.Clamp(_resistFire, 0, 100);
    public int ResistLightning => Math.Clamp(_resistLightning, 0, 100);

    public ResistanceManager()
    {
        // subscribe to events
        EquipementEvents.OnItemEquip += AddModifier;
        EquipementEvents.OnItemUnequip += RemoveModifier;
    }

    //public void Test(int rm, int rf, int rl)
    //{
    //    _resistMagic = rm;
    //    _resistFire = rf;
    //    _resistLightning = rl;
    //}

    Func<int, int> SwithcSign = (x) => -1 * x;

    private void AddModifier(StatsModifier statsModifier)
    {
        int resistMagicMod = statsModifier.ResistMagic;
        int resistFireMod = statsModifier.ResistFire;
        int resistLightningMod = statsModifier.ResistLightning;

        if (resistMagicMod != 0) _resistMagic += resistMagicMod;
        if (resistFireMod != 0) _resistFire += resistFireMod;
        if (resistLightningMod != 0) _resistLightning += resistLightningMod;
    }

    private void RemoveModifier(StatsModifier statsModifier)
    {
        int resistMagicMod = statsModifier.ResistMagic;
        int resistFireMod = statsModifier.ResistFire;
        int resistLightningMod = statsModifier.ResistLightning;

        if (resistMagicMod != 0) _resistMagic += SwithcSign(resistMagicMod);
        if (resistFireMod != 0) _resistFire += SwithcSign(resistFireMod);
        if (resistLightningMod != 0) _resistLightning += SwithcSign(resistLightningMod);
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
        return sb.ToString();
    }

    public void Dispose()
    {
        // unsubscribe from events
        EquipementEvents.OnItemEquip -= AddModifier;
        EquipementEvents.OnItemUnequip -= RemoveModifier;
    }
}

/*
    Life

    Warrior: 2·Vit(character) + 2·Vit(items) + 2·clvl + Life(items) + 18

    Rogue: 1·Vit(character) + 1.5·Vit(items) + 2·clvl + Life(items) + 23

    Sorcerer: 1·Vit(character) + 1·Vit(items) + 1·clvl + Life(items) + 9
*/

/*

Mana

Warrior: 1·Mag(character) + 1·Mag(items) + 1·clvl + Mana(items) - 1

Rogue: 1·Mag(character) + 1.5·Mag(items) + 2·clvl + Mana(items) + 5

Sorcerer: 2·Mag(character) + 2·Mag(items) + 2·clvl + Mana(items) - 2

*/
