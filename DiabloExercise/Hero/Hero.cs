using System.Text;
/*
*/

public abstract class Hero
{
    // Name - created once at the begining of the game. Not changable.
    private string _name;
    public string Name => _name;

    public virtual string HeroClass => GetType().Name;

    //public readonly ILevel heroLevel;
    public readonly IExperienceManager experienceManager;

    public readonly IGoldManager goldManager;
    public readonly IResource lifeManager;
    public readonly IResource manaManager;
    public readonly ILightRadius lightRadius;
    public readonly IResistance resistnaceManager;
    public readonly ICharacterAttributes characterAttributes;
    public readonly IDerivedAttributes derivedAttributes;
    public readonly ResourcesCalculator resourcesCalculator;
    public readonly EquipmentManager equipmentManager;

    //public readonly ItemsBonusManager ItemsBonusManager;
    public readonly DerivedAttributesCalculator derivedAttributesCalculator = new DerivedAttributesCalculator(); // INJECT IT LATER

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
        this.characterAttributes = characterAttributes;
        this.derivedAttributes = derivedAttributes;
        this.resourcesCalculator = resourcesCalculator;
        this.equipmentManager = equipmentManager;
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
                characterAttributes.CurrentDexterity,
                derivedAttributes.ArmorClassBaseItems,
                derivedAttributes.ArmorClassItemsBonus
            );
        derivedAttributes.SetCurrentArmor(currentArmorClass);
    }
    private void UpdateChanceToHit()
    {
        var chanceToHit = derivedAttributesCalculator.CalculateChanceToHit(characterAttributes.CurrentDexterity, derivedAttributes.ChanceToHitItemsBonus);
        derivedAttributes.SetChanceToHit(chanceToHit);
    }
    private void UpdateDamage()
    {
        var damage = derivedAttributesCalculator.CalculateDamageRange
            (
                characterAttributes.CurrentStrength,
                experienceManager.Level,
                derivedAttributes.BaseDamageItemsMin,
                derivedAttributes.BaseDamageItemsMax,
                derivedAttributes.BonusDamageFromItems
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
            characterAttributes.BaseVitality,
            characterAttributes.VitalityItemsBonus,
            manaManager.ItemsBonus
            );

        lifeManager.UpdateBaseMaxValue(value);
    }

    public void UpdateMaxMana()
    {
        var value = resourcesCalculator.CalculateMaxMana(
            experienceManager.Level,
            characterAttributes.BaseMagic,
            characterAttributes.MagicItemsBonus,
            manaManager.ItemsBonus
            );

        manaManager.UpdateBaseMaxValue(value);
    }

    public void EquipHeadArmor(HeadArmor headArmor)
    {
        if (headArmor == null) return;

        if (headArmor.Requirement > characterAttributes.CurrentStrength)
        {
            Console.WriteLine($"Requirements for this item are not met");
            return;
        }

        derivedAttributes.AddArmorBasicItems(headArmor.Armor);

        // recalculate armor
        UpdateCurrentArmor();

    }

    public void UnequipHeadArmor()
    {
        if (equipmentManager.HeadArmor == null) return;

        derivedAttributes.RemoveArmorBasicItems(equipmentManager.HeadArmor.Armor);

        equipmentManager.RemoveHeadArmor();


        // recalculate armor
        UpdateCurrentArmor();
    }

    public void MetodaKojaBiTrebaloDaUradi_OnUnquip()
    {

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


    public float GetLightRadius() => lightRadius.ModifiedRadius;
    public float GetBaseLightRadius() => lightRadius.BaseLightRadius;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("****** Hero Table ******");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Class: {HeroClass}");

        sb.AppendLine(experienceManager.ToString());
        //sb.AppendLine($"Level: {experienceManager.Level}");
        //sb.AppendLine($"Experience:{experienceManager.Experience}. XPforNextLevel:{experienceManager.NextLevelExperience}");

        sb.AppendLine(goldManager.ToString());
        sb.AppendLine(lifeManager.ToString());
        sb.AppendLine(manaManager.ToString());
        sb.AppendLine(characterAttributes.ToString());
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
