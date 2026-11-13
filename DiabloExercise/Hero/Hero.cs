using System.Text;
/*
*/

public enum ItemAction
{
    Equip,
    Remove
}

public abstract class Hero
{
    // Name - created once at the begining of the game. Not changable.
    private string _name;
    public string Name => _name;

    public virtual string Class => GetType().Name;

    //public readonly ILevel heroLevel;
    public readonly IExperienceManager experienceManager;

    public readonly IGoldManager goldManager;
    public readonly IResource lifeManager;
    public readonly IResource manaManager;
    public readonly ILightRadius lightRadius;
    public readonly IResistance resistnaceManager;
    public readonly ICharacterAttributes CharacterAttributes;
    public readonly IDerivedAttributes derivedAttributes;
    public readonly ResourcesCalculator resourcesCalculator;
    public readonly EquipmentManager EquipmentManager;

    //public readonly ItemsBonusManager ItemsBonusManager;
    public readonly DerivedAttributesCalculator derivedAttributesCalculator = new DerivedAttributesCalculator(); // INJECT IT LATER

    Func<int, int> ChangeSign = (x) => -1 * x;

    protected Hero(
                        string name,
                        //ILevel heroLevel,
                        IGoldManager goldManager,
                        IExperienceManager experienceManager,
                        IResource healthManager,
                        IResource manaManager,
                        ILightRadius lightRadius,
                        IResistance resistnaceManager,
                        ICharacterAttributes characterAttributes,
                        IDerivedAttributes derivedAttributes,
                        ResourcesCalculator resourcesCalculator,
                        EquipmentManager equipmentManager
                 )
    {
        _name = name;
        //this.heroLevel = heroLevel;
        this.goldManager = goldManager;
        this.experienceManager = experienceManager;
        this.lifeManager = healthManager;
        this.manaManager = manaManager;
        this.lightRadius = lightRadius;
        this.resistnaceManager = resistnaceManager;
        this.CharacterAttributes = characterAttributes;
        this.derivedAttributes = derivedAttributes;
        this.resourcesCalculator = resourcesCalculator;
        this.EquipmentManager = equipmentManager;
        //this.ItemsBonusManager = ItemsBonusManager;

        UpdateMaxLife();
        UpdateMaxMana();

        UpdateDerivedAttributes();
        PotionEvents.InvokeOnFullRejuvenationPotionConsumed(); // this basicly heals the player with the potion :) and yet it works :)

    }




    public void UpdateDerivedAttributes()
    {
        // update armor
        UpdateCurrentArmor();

        // uptate chance to hit
        UpdateChanceToHit();

        // update damage
        UpdateDamage();
    }


    private void UpdateCurrentArmor()
    {
        var currentArmorClass = derivedAttributesCalculator.CalculateArmorClass
            (
                CharacterAttributes.CurrentDexterity,
                derivedAttributes.ArmorClassBasicItems,
                derivedAttributes.ArmorClassSpecialItems
            );
        derivedAttributes.SetCurrentArmor(currentArmorClass);
    }
    private void UpdateChanceToHit()
    {
        var chanceToHit = derivedAttributesCalculator.CalculateChanceToHit(CharacterAttributes.CurrentDexterity, derivedAttributes.ChanceToHitSpecialItemsBonus);
        derivedAttributes.SetChanceToHit(chanceToHit);
    }
    private void UpdateDamage()
    {
        var damage = derivedAttributesCalculator.CalculateDamageRange
            (
                CharacterAttributes.CurrentStrength,
                experienceManager.Level,
                derivedAttributes.BaseDamageItemsMin,
                derivedAttributes.BaseDamageItemsMax,
                derivedAttributes.DamageSpecialtemsBonus
            );
        derivedAttributes.SetCurrentDamage(damage.min, damage.max);
    }
    /*
       Life
       Warrior: 2·Vit(character) + 2·Vit(items) + 2·clvl + Life(items) + 18
       Rogue: 1·Vit(character) + 1.5·Vit(items) + 2·clvl + Life(items) + 23
       Sorcerer: 1·Vit(character) + 1·Vit(items) + 1·clvl + Life(items) + 9

       Mana
       Warrior: 1·Mag(character) + 1·Mag(items) + 1·clvl + Mana(items) - 1
       Rogue: 1·Mag(character) + 1.5·Mag(items) + 2·clvl + Mana(items) + 5
       Sorcerer: 2·Mag(character) + 2·Mag(items) + 2·clvl + Mana(items) - 2
   */


    public void UpdateMaxLife()
    {
        var value = resourcesCalculator.CalculateMaxLife(
            experienceManager.Level,
            CharacterAttributes.BaseVitality,
            CharacterAttributes.VitalityItemsBonus,
            manaManager.ItemsBonus
            );

        lifeManager.UpdateBaseMaxValue(value);
    }

    public void UpdateMaxMana()
    {
        var value = resourcesCalculator.CalculateMaxMana(
            experienceManager.Level,
            CharacterAttributes.BaseMagic,
            CharacterAttributes.MagicItemsBonus,
            manaManager.ItemsBonus
            );

        manaManager.UpdateBaseMaxValue(value);
    }

    public void EquipHeadArmor(HeadArmor headArmor)
    {
        if (headArmor == null) return;

        if (headArmor.Requirement > CharacterAttributes.CurrentStrength)
        {
            Console.WriteLine($"Requirements for this item are not met");
            return;
        }

        HeadArmor? oldHeadArmor = EquipmentManager.HeadArmor; // is there anything on the head

        // if there is an object, remove it and calculate removal
        if (oldHeadArmor != null)
        {
            RemoveHeadArmor();
        }


        // recalculate armor
        derivedAttributes.ModifyArmorBasicItems(headArmor.Armor);

        UpdateCurrentArmor();

        if (headArmor.IsIdentified && headArmor.StatsModifier is not null)
        {
            AddSpecialItemModifiers(headArmor.StatsModifier, ItemAction.Equip);

            UpdateDerivedAttributes();
        }

        EquipmentManager.EquipHeadArmor(headArmor);
    }


    public void RemoveHeadArmor()
    {
        HeadArmor? headArmor = EquipmentManager.HeadArmor;

        if (headArmor is null) return;

        // recalculate armor
        derivedAttributes.ModifyArmorBasicItems(ChangeSign(headArmor.Armor));

        UpdateCurrentArmor();

        if (headArmor.IsIdentified && headArmor.StatsModifier is not null)
        {
            AddSpecialItemModifiers(headArmor.StatsModifier, ItemAction.Remove);

            UpdateDerivedAttributes();
        }

        EquipmentManager.RemoveHeadArmor();
    }


    public void IdentifyItem(IIdentifiable itemToIdentify)
    {
        itemToIdentify.Identify();


        if (itemToIdentify.IsIdentified && itemToIdentify.StatsModifier is not null)
        {
            AddSpecialItemModifiers(itemToIdentify.StatsModifier, ItemAction.Equip);

            UpdateDerivedAttributes();
        }


    }

    private float GetSignFromItemAction(float value, ItemAction itemAction)
    {
        if (itemAction == ItemAction.Remove)
        {
            return -1 * value;
        }

        return value;
    }

    public void AddSpecialItemModifiers(StatsModifier? statsModifier, ItemAction itemAction)
    {
        if (statsModifier is null) return;

        //light
        var lightModifier = statsModifier.LightRadius;
        lightModifier = GetSignFromItemAction(lightModifier, itemAction);

        lightRadius.ModifyLightRadiusItemsBonus(lightModifier);

        // life
        var lifeModifier = statsModifier.Life;
        lifeModifier = GetSignFromItemAction(lifeModifier, itemAction);

        if (lifeModifier != 0) lifeManager.AddItemsBonus(lifeModifier);

        // mana
        var manaModifier = statsModifier.Mana;
        manaModifier = GetSignFromItemAction(manaModifier, itemAction);

        if (manaModifier != 0) lifeManager.AddItemsBonus(manaModifier);

        // strength
        var strengthModifier = statsModifier.Strength;
        strengthModifier = (int)GetSignFromItemAction(strengthModifier, itemAction);

        if (strengthModifier != 0) CharacterAttributes.ModifyStrengthItemBonus(strengthModifier);

        // magic
        var magicModifier = statsModifier.Magic;
        magicModifier = (int)GetSignFromItemAction(magicModifier, itemAction);

        if (magicModifier != 0) CharacterAttributes.ModifyMagicItemBonus(magicModifier);

        // dexterity
        var dexterityModifier = statsModifier.Dexterity;
        dexterityModifier = (int)GetSignFromItemAction(dexterityModifier, itemAction);

        if (dexterityModifier != 0) CharacterAttributes.ModifyDexterityItemBonus(dexterityModifier);

        // vitality
        var vitalityModifier = statsModifier.Vitality;
        vitalityModifier = (int)GetSignFromItemAction(vitalityModifier, itemAction);

        if (vitalityModifier != 0) CharacterAttributes.ModifyVitalityItemBonus(vitalityModifier);

        // armor
        var armorModifier = statsModifier.ArmorClass;
        armorModifier = (int)GetSignFromItemAction(armorModifier, itemAction);

        if (armorModifier != 0) derivedAttributes.ModifyArmorSpecialItemsBonus(armorModifier);

        // chanceToHit
        var chanceToHitModifier = statsModifier.ChanceToHit;
        chanceToHitModifier = (int)GetSignFromItemAction(chanceToHitModifier, itemAction);

        if (chanceToHitModifier != 0) derivedAttributes.ModifyChanceToHitSpecialItemsBonus(chanceToHitModifier);

        // damage
        var damageModifier = statsModifier.Damage;
        damageModifier = (int)GetSignFromItemAction(damageModifier, itemAction);

        if (damageModifier != 0) derivedAttributes.ModifyDamageSpecialItemsBonus(damageModifier);

        // magic resistance
        var magicResistance = statsModifier.MagicResistance;
        magicResistance = (int)GetSignFromItemAction(magicResistance, itemAction);

        if (magicResistance != 0) resistnaceManager.ModifyMagicResistance(magicResistance);

        // fire resistance
        var fireResistance = statsModifier.FireResistance;
        fireResistance = (int)GetSignFromItemAction(fireResistance, itemAction);

        if (fireResistance != 0) resistnaceManager.ModifyFireResistance(fireResistance);

        // lightning resistance
        var lightningResistance = statsModifier.LightningResistance;
        lightningResistance = (int)GetSignFromItemAction(lightningResistance, itemAction);

        if (lightningResistance != 0) resistnaceManager.ModifyLightningResistance(lightningResistance);

    }


    public void GainExpirience(long incrementAmount)
    {
        experienceManager.Increase(incrementAmount);
    }

    public void AddGold(int amountToAdd)
    {
        goldManager.Add(amountToAdd);

        Console.WriteLine($"Hero received {amountToAdd} gold, and now has {goldManager.GoldCount}");
    }

    public void SpendGold(int amountToSpend)
    {
        bool isSpent = goldManager.Decrease(amountToSpend);

        if (isSpent)
        {
            Console.WriteLine("Transaction sucsessfull");
            Console.WriteLine($"Hero deducted {amountToSpend} gold, and now has {goldManager.GoldCount}");
        }
        else
        {
            Console.WriteLine("Not enough gold");
        }
    }

    public void GetDamage(float lifePointsToLose)
    {
        lifeManager.Decrease(lifePointsToLose);
    }

    public void SpendMana(float manaToSpend)
    {
        manaManager.Decrease(manaToSpend); // information of sucsess potentionally needed
    }


    public float GetLightRadius() => lightRadius.CurrentLightRadius;
    public float GetBaseLightRadius() => lightRadius.BaseLightRadius;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("****** Hero Table ******");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Class: {Class}");

        sb.AppendLine(experienceManager.ToString());
        //sb.AppendLine($"Level: {experienceManager.Level}");
        //sb.AppendLine($"Experience:{experienceManager.Experience}. XPforNextLevel:{experienceManager.NextLevelExperience}");

        sb.AppendLine(goldManager.ToString());
        sb.AppendLine(lifeManager.ToString());
        sb.AppendLine(manaManager.ToString());
        sb.AppendLine(CharacterAttributes.ToString());
        sb.AppendLine(derivedAttributes.ToString());
        //sb.AppendLine($"MaxLife:{lifeManager.CurrentMaxValue}. CurrentLife: {lifeManager.CurrentValue}");
        //sb.AppendLine($"MaxMana:{manaManager.CurrentMaxValue}. CurrentMana: {manaManager.CurrentValue}");
        sb.AppendLine(lightRadius.ToString());
        sb.AppendLine("*** Resistance ***");
        sb.AppendLine(resistnaceManager.ToString());
        sb.AppendLine();

        return sb.ToString();
    }
}

public class Warrior : Hero
{
    public Warrior
        (
            string name,
            IGoldManager goldManager,
            IExperienceManager experienceManager,
            IResource healthManager,
            IResource manaManager,
            ILightRadius lightRadius,
            IResistance resistnaceManager,
            ICharacterAttributes characterAttributes,
            IDerivedAttributes derivedAttributes,
            ResourcesCalculator attributesCalculator,
            EquipmentManager equipmentManager
        )
        : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator, equipmentManager)
    {
    }
}
public class Rogue : Hero
{
    public Rogue
        (
            string name,
            IGoldManager goldManager,
            IExperienceManager experienceManager,
            IResource healthManager,
            IResource manaManager,
            ILightRadius lightRadius,
            IResistance resistnaceManager,
            ICharacterAttributes characterAttributes,
            IDerivedAttributes derivedAttributes,
            ResourcesCalculator attributesCalculator,
            EquipmentManager equipmentManager
        )
        : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator, equipmentManager)
    {
    }
}
public class Sorcerer : Hero
{
    public Sorcerer
        (
            string name,
            IGoldManager goldManager,
            IExperienceManager experienceManager,
            IResource healthManager,
            IResource manaManager,
            ILightRadius lightRadius,
            IResistance resistnaceManager,
            ICharacterAttributes characterAttributes,
            IDerivedAttributes derivedAttributes,
            ResourcesCalculator attributesCalculator,
            EquipmentManager equipmentManager
        )
        : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator, equipmentManager)
    {
    }
}
