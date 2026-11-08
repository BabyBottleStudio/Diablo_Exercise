public class DerivedAttributesCalculator
{
    const int FIST_DAMAGE = 1;
    const int FIST_BONUS = 5;

    public int CalculateArmorClass(int currentDexterity, int armorBasicItems, int armorItemsBonus)
    {
        return currentDexterity / 5 + armorBasicItems + armorItemsBonus; // * (_armorItemsBonusPercent/100);
    }

    public int CalculateChanceToHit(int currentDexterity, int chanceToHitItemsBonus)
    {
        return 50 + currentDexterity / 2 + chanceToHitItemsBonus;
    }


    public (int min, int max) CalculateDamageRange(int currentStrength, int characterLevel, int baseDamageItemsMin, int baseDamageItemsMax, int bonusDamageItems)
    {
        int basicDamage = currentStrength * characterLevel / 100;

        if (baseDamageItemsMin <= 0 && baseDamageItemsMax <= 0)
        {
            int fistDamage = basicDamage + FIST_DAMAGE + FIST_BONUS;
            return (fistDamage, fistDamage);
        }

        int minDamage = basicDamage + baseDamageItemsMin + bonusDamageItems;
        int maxDamage = basicDamage + baseDamageItemsMax + bonusDamageItems;

        return (minDamage, maxDamage);
    }
}

