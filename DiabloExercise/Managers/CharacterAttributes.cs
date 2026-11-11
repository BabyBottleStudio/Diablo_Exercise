using System.Text;

public interface IStrength
{
    int BaseStrength { get; }
    int CurrentStrength { get; }

}

public interface IMagical
{
    int BaseMagic { get; }
    int CurrentMagic { get; }
    int MagicItemsBonus { get; }
}

public interface IDexterity
{
    int BaseDexterity { get; }
    int CurrentDexterity { get; }
}

public interface IVitality
{
    int BaseVitality { get; }
    int CurrentVitality { get; }
    int VitalityItemsBonus { get; }
}

public interface ICharacterAttributes : IStrength, IMagical, IDexterity, IVitality
{
    public void ModifyStrengthItemBonus(int value);
    public void ModifyMagicItemBonus(int value);
    public void ModifyDexterityItemBonus(int value);
    public void ModifyVitalityItemBonus(int value);
}


public class CharacterAttributes : ICharacterAttributes //, IDisposable
{
    //int _characterLevel;

    int _baseStrength;
    int _maxStrength;
    int _strengthItemsBonus;

    public int BaseStrength => Math.Min(_maxStrength, _baseStrength); // it is capped differently depending on the hero class
    public int CurrentStrength => BaseStrength + _strengthItemsBonus;



    int _baseMagic;
    int _maxMagic;
    int _magicItemsBonus;

    public int BaseMagic => Math.Min(_maxMagic, _baseMagic); // it is capped differently depending on the hero class
    public int CurrentMagic => BaseMagic + MagicItemsBonus;
    public int MagicItemsBonus => _magicItemsBonus;


    int _baseDexterity;
    int _maxDexterity;
    int _dexterityItemsBonus;

    public int BaseDexterity => Math.Min(_maxDexterity, _baseDexterity); // it is capped differently depending on the hero class
    public int CurrentDexterity => BaseDexterity + _dexterityItemsBonus;


    int _baseVitality;
    int _maxVitality;
    int _vitalityItemsBonus;
    public int BaseVitality => Math.Min(_maxVitality, _baseVitality); // it is capped differently depending on the hero class
    public int CurrentVitality => BaseVitality + VitalityItemsBonus;
    public int VitalityItemsBonus => _vitalityItemsBonus;

    public CharacterAttributes(int startStrength, int maxStrength, int startMagic, int maxMagic, int startDexterity, int maxDexterity, int startVitality, int maxVitality)
    {
        // _characterLevel = 1;

        _baseStrength = startStrength;
        _maxStrength = maxStrength;

        _baseMagic = startMagic;
        _maxMagic = maxMagic;

        _baseDexterity = startDexterity;
        _maxDexterity = maxDexterity;

        _baseVitality = startVitality;
        _maxVitality = maxVitality;

        //EquipementEvents.OnItemEquip += AddModifiers;
        //EquipementEvents.OnItemUnequip += RemoveModifiers;
    }

    private void IncreaseBaseStrength()
    {
        if (BaseStrength >= _maxStrength) return;

        _baseStrength++;
    }


    public void ModifyStrengthItemBonus(int value)
    {
        _strengthItemsBonus += value;
    }

    public void ModifyMagicItemBonus(int value)
    {
        _magicItemsBonus += value;
    }

    public void ModifyDexterityItemBonus(int value)
    {
        _dexterityItemsBonus += value;
    }

    public void ModifyVitalityItemBonus(int value)
    {
        _vitalityItemsBonus += value;
    }


    // ModifyCharAtributes

    /*
    private void AddModifiers(StatsModifier statsModifier)
    {
        int strengthModifier = statsModifier.Strength;
        int magicModifier = statsModifier.Magic;
        int dexterityModifer = statsModifier.Dexterity;
        int vitalityModifier = statsModifier.Vitality;



        if (strengthModifier != 0)
        {
            _strengthItemsBonus += strengthModifier;
        }

        if (magicModifier != 0)
        {
            _magicItemsBonus += magicModifier;
            RecalculateMana();
        }

        if (dexterityModifer != 0)
        {
            _dexterityItemsBonus += dexterityModifer;
        }

        if (vitalityModifier != 0)
        {
            _vitalityItemsBonus += vitalityModifier;
            RecalculateLife();
        }
    }*/

     // ovo se ponavlja trebalo bi da se ubaci negde gde moze da se uvek pozove

    /*
    private void RemoveModifiers(StatsModifier statsModifier)
    {
        int strengthModifier = statsModifier.Strength;
        int magicModifier = statsModifier.Magic;
        int dexterityModifer = statsModifier.Dexterity;
        int vitalityModifier = statsModifier.Vitality;

        if (strengthModifier != 0)
        {
            _strengthItemsBonus += ChangeSign(strengthModifier);
        }

        if (magicModifier != 0)
        {
            _magicItemsBonus += ChangeSign(magicModifier);
            RecalculateMana();
        }

        if (dexterityModifer != 0)
        {
            _dexterityItemsBonus += ChangeSign(dexterityModifer);
        }

        if (vitalityModifier != 0)
        {
            _vitalityItemsBonus += ChangeSign(vitalityModifier);
            RecalculateLife();
        }
    } */

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("- STRENGTH - ");
        sb.AppendLine($"Base: {BaseStrength} || Now: {CurrentStrength}");
        sb.AppendLine("- MAGIC -");
        sb.AppendLine($"Base: {BaseMagic} || Now: {CurrentMagic}");
        sb.AppendLine("- DEXTERITY -");
        sb.AppendLine($"Base: {BaseDexterity} || Now: {CurrentDexterity}");
        sb.AppendLine("- VITALITY -");
        sb.AppendLine($"Base: {BaseVitality} || Now: {CurrentVitality}");

        return sb.ToString().Trim();
    }

    //public void Dispose()
    //{
    //    EquipementEvents.OnItemEquip -= AddModifiers;
    //    EquipementEvents.OnItemUnequip -= RemoveModifiers;
    //}

   
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
