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

}


public class CharacterAttributes : ICharacterAttributes, IDisposable
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

        EquipementEvents.OnItemEquip += AddModifiers;
        EquipementEvents.OnItemUnequip += RemoveModifiers;
    }

    private void IncreaseBaseStrength()
    {
        if (BaseStrength >= _maxStrength) return;

        _baseStrength++;
    }



    // ModifyCharAtributes

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
    }

    Func<int, int> ChangeSign = (x) => -1 * x; // ovo se ponavlja trebalo bi da se ubaci negde gde moze da se uvek pozove

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
    }

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


    private void RecalculateLife()
    {
        Console.WriteLine("Life should be recalculated. Not implemented yet");
    }


    private void RecalculateMana()
    {
        Console.WriteLine("Mana should be recalculated. Not implemented yet");
    }


    public void Dispose()
    {
        EquipementEvents.OnItemEquip -= AddModifiers;
        EquipementEvents.OnItemUnequip -= RemoveModifiers;
    }
}


public interface IArmor
{
    int CurrentArmorClass { get; }
    int ArmorClassBaseItems { get; }
    int ArmorClassItemsBonus { get; }
    void SetCurrentArmorClass(int value);
}

public interface IChanceToHit
{
    int ChanceToHit { get; }
    int ChanceToHitItemsBonus { get; }
    void SetChanceToHit(int value);
}

public interface IDamage
{
    int CurrentDamageMin { get; }
    int CurrentDamageMax { get; }

    int BaseDamageItemsMin { get; } // basic damage derived from weapon
    int BaseDamageItemsMax { get; }
    public int BonusDamageFromItems { get; } // bonus damage derived from special items

    public void SetCurrentDamage(int minDamage, int maxDamage);

    public void SetBaseWeaponDamage(int minValue, int maxValue);
}

public interface IDerivedAttributes : IArmor, IChanceToHit, IDamage
{
}

public class DerivedAttributes : IDerivedAttributes, IDisposable
{
    // armor
    //private int _baseArmorClass; // calculated by dexterity (dex / 5)
    private int _armorClassBaseItems; // armor derived from basic equipment like mails, shields and helmets
    int _armorClassItemsBonus; // bonus derived from special items

    //public int ArmorClassBase => _baseArmorClass;
    public int ArmorClassBaseItems => _armorClassBaseItems;
    public int ArmorClassItemsBonus => _armorClassItemsBonus;

    public int CurrentArmorClass { get; private set; }

    public void SetCurrentArmorClass(int value)
    {
        CurrentArmorClass = value;
    }

    /*
     To Hit %
        on character screen: 50 + Dex/2 + ToHititems
        melee: 50 + Dex/2 + ToHititems + clvl + bonus
        arrow: 50 + Dex + ToHititems + clvl + bonus - distance·distance/2

        magic versus monster: 50 + Mag - 2·mlvl + bonus
        magic versus player: 50 + Mag - 2·clvltarget + bonus
    */



    //int _chanceToHitBonus; // this bonus is calcualted during the combat and it is different depending on the hero class and weapon (range or melee)
    // it should be applied in the damage calculator...
    public int ChanceToHit { get; private set; }
    public int ChanceToHitItemsBonus { get; private set; }

    public void SetChanceToHit(int value)
    {
        ChanceToHit = value;
    }



    //int _baseDamageFromItemsMin; 
    //int _baseDamageFromItemsMax;

    public int BonusDamageFromItems { get; private set; } // bonus from special items

    // in real diablo Hellfire , naked hero has base "fist" damage of min and max = 1 and a hidden bonus of 5. 
    // when the weapon is equipped, bonus of 5 dissapears and "fist" damage is replaced with damage from equipped weapon

    public int CurrentDamageMin { get; private set; }
    public int CurrentDamageMax { get; private set; }

    public int BaseDamageItemsMin { get; private set; }// basic damage derived from weapon
    public int BaseDamageItemsMax { get; private set; }



    public DerivedAttributes()
    {
        EquipementEvents.OnItemEquip += AddModifiers;
        EquipementEvents.OnItemUnequip += RemoveModifiers;
    }



    public void SetCurrentDamage(int minDamage, int maxDamage)
    {
        CurrentDamageMin = minDamage;
        CurrentDamageMax = maxDamage;
    }

    public void SetBaseWeaponDamage(int minValue, int maxValue)
    {
        BaseDamageItemsMin = minValue;
        BaseDamageItemsMax = maxValue;
    }

    /*
    public void UpdateDamage(int currentStrength, int characterLevel)
    {
        if (_baseDamageFromItemsMin <= 0)
            _baseDamageMin = (currentStrength * characterLevel) / 100 + _baseFistDamage + _fistBonus;
        else
            _baseDamageMin = (currentStrength * characterLevel) / 100 + _baseDamageFromItemsMin + _bonusDamageFromItems;


        if (_baseDamageFromItemsMax <= 0)
            _baseDamageMax = (currentStrength * characterLevel) / 100 + _baseFistDamage + _fistBonus;
        else
            _baseDamageMax = (currentStrength * characterLevel) / 100 + _baseDamageFromItemsMax + _bonusDamageFromItems;

    }*/





    private void AddModifiers(StatsModifier statsModifier)
    {
        int armorModifier = statsModifier.ArmorClass;
        int chanceToHitModifier = statsModifier.ChanceToHit;
        int damageModifier = statsModifier.Damage;

        if (armorModifier != 0)
            _armorClassItemsBonus += armorModifier;

        if (chanceToHitModifier != 0)
            ChanceToHitItemsBonus += chanceToHitModifier;

        if (damageModifier != 0)
            BonusDamageFromItems += damageModifier;

        // fali da se doda osnovni damage od oruzja koji uvek ostje 0 za sada.
    }

    Func<int, int> ChangeSign = (x) => -1 * x; // this is repeating for the milionth time

    private void RemoveModifiers(StatsModifier statsModifier)
    {
        int armorModifier = statsModifier.ArmorClass;
        int chanceToHitModifier = statsModifier.ChanceToHit;
        int damageModifier = statsModifier.Damage;

        if (armorModifier != 0)
            _armorClassItemsBonus += ChangeSign(armorModifier);

        if (chanceToHitModifier != 0)
            ChanceToHitItemsBonus += ChangeSign(chanceToHitModifier);

        if (damageModifier != 0)
            BonusDamageFromItems += ChangeSign(damageModifier);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"ARMOR: {CurrentArmorClass}");

        sb.AppendLine($"ChanceToHit:{ChanceToHit}%");
        sb.AppendLine($"Damage: {CurrentDamageMin}/{CurrentDamageMax}");

        return sb.ToString().Trim();
    }

    public void Dispose()
    {
        EquipementEvents.OnItemEquip -= AddModifiers;
        EquipementEvents.OnItemUnequip -= RemoveModifiers;
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
