using System.Text;

public class StatsModifier
{
    public float LightRadius { get; init; }

    public int Strength { get; init; }
    public int Magic { get; init; }
    public int Dexterity { get; init; }
    public int Vitality { get; init; }

    public int ArmorClass { get; init; }
    public int ChanceToHit { get; init; }
    public int Damage { get; init; }

    public int MagicResistance { get; init; }
    public int FireResistance { get; init; }
    public int LightningResistance { get; init; }

    public float Life { get; init; }
    public float Mana { get; init; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        // Pomoćna lokalna funkcija za lepši RPG format (+5 umesto 5)
        string FormatStat(float value) => value > 0 ? $"+{value}" : value.ToString();

        if (LightRadius != 0) sb.AppendLine($"{FormatStat(LightRadius)} to Light Radius.");
        if (Strength != 0) sb.AppendLine($"{FormatStat(Strength)} to Strength");
        if (Magic != 0) sb.AppendLine($"{FormatStat(Magic)} to Magic");
        if (Dexterity != 0) sb.AppendLine($"{FormatStat(Dexterity)} to Dexterity");
        if (Vitality != 0) sb.AppendLine($"{FormatStat(Vitality)} to Vitality");

        if (ArmorClass != 0) sb.AppendLine($"{FormatStat(ArmorClass)} to Armor");
        if (ChanceToHit != 0) sb.AppendLine($"{FormatStat(ChanceToHit)}% to Chance to hit"); // % je čest kod šanse za pogodak
        if (Damage != 0) sb.AppendLine($"{FormatStat(Damage)} to Damage");

        if (MagicResistance != 0) sb.AppendLine($"{FormatStat(MagicResistance)}% to Magic Resistance");
        if (FireResistance != 0) sb.AppendLine($"{FormatStat(FireResistance)}% to Fire Resistance");
        if (LightningResistance != 0) sb.AppendLine($"{FormatStat(LightningResistance)}% to Lightning Resistance");

        if (Life != 0) sb.AppendLine($"{FormatStat(Life)} to Life");
        if (Mana != 0) sb.AppendLine($"{FormatStat(Mana)} to Mana");

        return sb.ToString().TrimEnd(); // TrimEnd uklanja poslednji prazan red koji AppendLine napravi
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
