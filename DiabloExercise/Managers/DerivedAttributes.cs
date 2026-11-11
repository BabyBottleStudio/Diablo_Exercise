using System.Text;

public interface IChanceToHit
{
    int ChanceToHit { get; }
    int ChanceToHitSpecialItemsBonus { get; }
    void SetChanceToHit(int value);
    void ModifyChanceToHitSpecialItemsBonus(int value);
}

public interface IDamage
{
    int CurrentDamageMin { get; }
    int CurrentDamageMax { get; }

    int BaseDamageItemsMin { get; } // basic damage derived from weapon
    int BaseDamageItemsMax { get; }
    public int DamageSpecialtemsBonus { get; } // bonus damage derived from special items

    public void SetCurrentDamage(int minDamage, int maxDamage);

    public void SetBaseWeaponDamage(int minValue, int maxValue);
    void ModifyDamageSpecialItemsBonus(int value);
}

public interface IArmor
{
    int CurrentArmorClass { get; }
    int ArmorClassBasicItems { get; }
    int ArmorClassSpecialItems { get; }
    void SetCurrentArmor(int value);
    void ModifyArmorBasicItems(int value);
    //void RemoveArmorBasicItems(int value);
    void ModifyArmorSpecialItemsBonus(int value);

}

public interface IDerivedAttributes : IArmor, IChanceToHit, IDamage
{
}

public class DerivedAttributes : IDerivedAttributes
{
    // armor
    //int _baseArmorClass; // calculated by dexterity (dex / 5)
    //int _armorClassBaseItems; // armor derived from basic equipment like mails, shields and helmets
    //int _armorClassSpecialItems; // bonus derived from special items

    //public int ArmorClassBase => _baseArmorClass;
    public int ArmorClassBasicItems { get; private set; } // => _armorClassBaseItems;
    public int ArmorClassSpecialItems { get; private set; } // => _armorClassSpecialItems;

    public int CurrentArmorClass { get; private set; }

    public void SetCurrentArmor(int value)
    {
        CurrentArmorClass = value;
    }
    public void ModifyArmorBasicItems(int value)
    {
        ArmorClassBasicItems += value;
    }
    public void ModifyArmorSpecialItemsBonus(int value)
    {
        ArmorClassSpecialItems += value;
    }
    //public void RemoveArmorBasicItems(int value)
    //{
    //    _armorClassBaseItems -= value;
    //}

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
    public int ChanceToHitSpecialItemsBonus { get; private set; }

    public void SetChanceToHit(int value)
    {
        ChanceToHit = value;
    }

    public void ModifyChanceToHitSpecialItemsBonus(int value)
    {
        ChanceToHitSpecialItemsBonus += value;
    }

    //int _baseDamageFromItemsMin; 
    //int _baseDamageFromItemsMax;

    public int DamageSpecialtemsBonus { get; private set; } // bonus from special items

    // in real diablo Hellfire , naked hero has base "fist" damage of min and max = 1 and a hidden bonus of 5. 
    // when the weapon is equipped, bonus of 5 dissapears and "fist" damage is replaced with damage from equipped weapon

    public int CurrentDamageMin { get; private set; }
    public int CurrentDamageMax { get; private set; }

    public int BaseDamageItemsMin { get; private set; }// basic damage derived from weapon
    public int BaseDamageItemsMax { get; private set; }



    /*
    public DerivedAttributes()
    {
        EquipementEvents.OnItemEquip += AddModifiers;
        EquipementEvents.OnItemUnequip += RemoveModifiers;
    }*/


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

    public void ModifyDamageSpecialItemsBonus(int value)
    {
        DamageSpecialtemsBonus += value;
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




    /*
    private void AddModifiers(StatsModifier statsModifier)
    {
        int armorModifier = statsModifier.ArmorClass;
        int chanceToHitModifier = statsModifier.ChanceToHit;
        int damageModifier = statsModifier.Damage;

        if (armorModifier != 0)
            ArmorClassSpecialItems += armorModifier;

        if (chanceToHitModifier != 0)
            ChanceToHitSpecialItemsBonus += chanceToHitModifier;

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
            ArmorClassSpecialItems += ChangeSign(armorModifier);

        if (chanceToHitModifier != 0)
            ChanceToHitSpecialItemsBonus += ChangeSign(chanceToHitModifier);

        if (damageModifier != 0)
            BonusDamageFromItems += ChangeSign(damageModifier);
    } 
    */

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"ARMOR: {CurrentArmorClass}");

        sb.AppendLine($"ChanceToHit:{ChanceToHit}%");
        sb.AppendLine($"Damage: {CurrentDamageMin}/{CurrentDamageMax}");

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
