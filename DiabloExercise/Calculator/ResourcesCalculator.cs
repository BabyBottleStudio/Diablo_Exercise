public class ResourcesCalculator
{

    float vitalityMultiplier; // warrior = 2; rogue = 1; Sorcerer = 1
    float vitalityItemsMultiplier; // war = 2; rogue = 1.5; sorc = 1
    float heroLevelMultiplierLife; // W = 2 ; R = 2; S = 1
    float lifeFixedBonus; //W = 18; R = 23; S = 9

    float magicMultilplier; //W =1; R = 1; S = 2
    float magicItemsMultiplier; // W = 1; R = 1.5; S = 2;
    float heroLevelMultiplierMana; //W = 1; R = 2; S = 2
    float magicFixedBonus; // W=-1; R = 5; S = -2

    public ResourcesCalculator(float vitalityMultiplier, float vitalityItemsMultiplier, float characterLevelMultiplierLife, float lifeFixedBonus,
        float magicMultilplier, float magicItemsMultiplier, float characterLevelMultiplierMana, float magicFixedBonus)
    {
        this.vitalityMultiplier = vitalityMultiplier;
        this.vitalityItemsMultiplier = vitalityItemsMultiplier;
        this.heroLevelMultiplierLife = characterLevelMultiplierLife;
        this.lifeFixedBonus = lifeFixedBonus;

        this.magicMultilplier = magicMultilplier;
        this.magicItemsMultiplier = magicItemsMultiplier;
        this.heroLevelMultiplierMana = characterLevelMultiplierMana;
        this.magicFixedBonus = magicFixedBonus;
    }

    public float CalculateMaxLife(int characterLevel, float baseVitality, float vitalityItemsBonus, float lifeItemsBonus)
    {
        var outVitality = baseVitality * vitalityMultiplier;
        var outVitalityItemsBonus = vitalityItemsBonus * vitalityItemsMultiplier;
        var outCharLevel = characterLevel * heroLevelMultiplierLife;

        return outVitality + outVitalityItemsBonus + outCharLevel + lifeItemsBonus + lifeFixedBonus;

        //Warrior: 2·Vit(character) + 2·Vit(items) + 2·clvl + Life(items) + 18
        //Rogue: 1·Vit(character) + 1.5·Vit(items) + 2·clvl + Life(items) + 23
        //Sorcerer: 1·Vit(character) + 1·Vit(items) + 1·clvl + Life(items) + 9
    }

    public float CalculateMaxMana(int characterLevel, float baseMagic, float magicItemsBonus, float manaExuipmentBonus)
    {
        var outMagic = baseMagic * magicMultilplier;
        var outMagicItemsBonus = magicItemsBonus * magicItemsMultiplier;
        var outCharLevel = characterLevel * heroLevelMultiplierMana;

        return outMagic + outMagicItemsBonus + outCharLevel + manaExuipmentBonus + magicFixedBonus;

        //Warrior: 1·Mag(character) + 1·Mag(items) + 1·clvl + Mana(items) - 1
        //Rogue: 1·Mag(character) + 1.5·Mag(items) + 2·clvl + Mana(items) + 5
        //Sorcerer: 2·Mag(character) + 2·Mag(items) + 2·clvl + Mana(items) - 2
    }
}

