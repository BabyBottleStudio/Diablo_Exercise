
using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

var heroCalc = new HeroLevelingCalculator(50);

heroCalc.PrintProgressList();
Console.WriteLine(heroCalc.GetExpirienceThreshold(13));
Console.WriteLine(heroCalc.GetExpirienceThreshold(49));
Console.WriteLine(heroCalc.GetExpirienceThreshold(50));
Console.WriteLine(heroCalc.GetExpirienceThreshold(53));
Console.WriteLine(heroCalc.GetExpirienceThreshold(0));
Console.WriteLine(heroCalc.GetExpirienceThreshold(-3));

var heroData = new HeroDataRepository();


var heroFactory = new HeroFactory(heroData);

IUserInterface userInterface;

var jovana = heroFactory.Instantiate("Rogue", "Jovana", out userInterface);

Console.WriteLine(jovana);


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
                        ResourcesCalculator resourcesCalculator
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
        //this.ItemsBonusManager = ItemsBonusManager;

        UpdateMaxLife();
        UpdateMaxMana();

        UpdateDerivedAttributes();
        PotionEvents.InvokeOnFullRejuvenationPotionConsumed(); // this basicly heals the player with the potion :) and yet it works :)

    }




    public void UpdateDerivedAttributes()
    {
        // update armor
        // 
        var currentArmorClass = derivedAttributesCalculator.CalculateArmorClass(characterAttributes.CurrentDexterity, derivedAttributes.ArmorClassBaseItems, derivedAttributes.ArmorClassItemsBonus);
        derivedAttributes.SetCurrentArmorClass(currentArmorClass);

        // uptate chance to hit
        var chanceToHit = derivedAttributesCalculator.CalculateChanceToHit(characterAttributes.CurrentDexterity, derivedAttributes.ChanceToHitItemsBonus);
        derivedAttributes.SetChanceToHit(chanceToHit);

        // update damage
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

    public void MetodaKojaBiTrebaloDaUradi_OnEquip()
    {

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

public class Warrior : Hero
{
    public Warrior(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager, ILightRadius lightRadius, IResistance resistnaceManager, ICharacterAttributes characterAttributes, IDerivedAttributes derivedAttributes, ResourcesCalculator attributesCalculator) : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator)
    {
    }
}
public class Rogue : Hero
{
    public Rogue(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager, ILightRadius lightRadius, IResistance resistnaceManager, ICharacterAttributes characterAttributes, IDerivedAttributes derivedAttributes, ResourcesCalculator attributesCalculator) : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator)
    {
    }
}
public class Sorcerer : Hero
{
    public Sorcerer(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager, ILightRadius lightRadius, IResistance resistnaceManager, ICharacterAttributes characterAttributes, IDerivedAttributes derivedAttributes, ResourcesCalculator attributesCalculator) : base(name, goldManager, experienceManager, healthManager, manaManager, lightRadius, resistnaceManager, characterAttributes, derivedAttributes, attributesCalculator)
    {
    }
}



public interface IExperienceManager
{
    int Level { get; }
    long Experience { get; }
    //long NextLevelExperience { get; }

    void Increase(long xpPointsToAdd);
    //void LevelUp();
}

public class HeroLevelingCalculator
{
    //public long NextLevelExperience => heroLevel.Level < heroLevel.MaxLevel ? nextLevelThreshold[heroLevel.Level + 1] : 0;

    private long[] nextLevelThreshold;


    public HeroLevelingCalculator(int heroMaxLevel)
    {
        nextLevelThreshold = new long[heroMaxLevel + 1];
        InitializeNextLevelTresholds();
    }


    public long GetExpirienceThreshold(int targetLevel)
    {
        if (targetLevel <= 0)
        {
            //debug
            Console.WriteLine($"Invalid hero level. It should be in the range of 1 to {nextLevelThreshold.Length - 1}");
            return 0;
        }

        if (targetLevel < nextLevelThreshold.Length)
        {
            return nextLevelThreshold[targetLevel];
        }

        // Debug
        Console.WriteLine("Hero reached the maximum level");
        return 0;
    }


    private void InitializeNextLevelTresholds()
    {
        const double baseXP = 1000;
        const double power = 1.5;
        const double growth = 1.08;

        for (int i = 1; i < nextLevelThreshold.Length; i++)
        {
            int level = i - 1;

            nextLevelThreshold[i] = level == 0
                ? 0
                : (long)Math.Floor(baseXP * Math.Pow(level, power) * Math.Pow(growth, level));
        }
    }

    public void PrintProgressList()
    {
        for (int i = 0; i < nextLevelThreshold.Length; i++)
        {
            Console.WriteLine($"Level{i:D2} - {nextLevelThreshold[i]}");
        }
    }
}


public class ExperienceManager : IExperienceManager
{
    private readonly ILevel heroLevel;
    private readonly HeroLevelingCalculator heroLvlCalc;
    //private readonly DerivedCharacterAttributesCalculator resourcesCalculator;

    private long _experience;
    public long Experience => _experience;
    public int Level => heroLevel.Level;
    //public int MaxLevel => heroLevel.MaxLevel;

    //private double _nextLevelAt;
    //public double NextLevelAt => _nextLevelAt;

    // event for leveling up


    public ExperienceManager(ILevel heroLevel, HeroLevelingCalculator heroLevelingCalculator)
    {
        this.heroLevel = heroLevel;
        heroLvlCalc = heroLevelingCalculator;
        _experience = 0;

    }

    public void Increase(long xpPointsToAdd)
    {
        if (heroLevel.Level == heroLevel.MaxLevel) return;

        if (xpPointsToAdd < 0) return;

        _experience += xpPointsToAdd;

        while (heroLevel.Level < heroLevel.MaxLevel && _experience >= heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1))
        {
            LevelUp();
        }

        HeroEvents.InvokeOnExpirienceIncreased(Experience);
    }


    private void LevelUp()
    {
        if (heroLevel.Level == heroLevel.MaxLevel)
        {
            Console.WriteLine("Hero is already on the maximum level.");
            return;
        }

        heroLevel.IncreaseLevel();

        // printing message
        string nextLevelXP = "Hero reached the maximum level!";

        if (heroLevel.Level < heroLevel.MaxLevel)
        {
            nextLevelXP = $"{heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1)}";
        }
        Console.WriteLine($"Level:{heroLevel.Level}; XP:{_experience}; XPtNextLevel: {nextLevelXP}");


        HeroEvents.InvokeOnLevelUp();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Level: {heroLevel.Level}");
        sb.AppendLine($"Experience:{Experience}. XPforNextLevel:{heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1)}");

        return sb.ToString().TrimEnd();
    }
}


public interface IEquipable
{
    void OnEquip();
    void OnUnequip();
}

public interface IDealDamage
{
    float MinDamage { get; init; }
    float MaxDamage { get; init; }
}

public interface IDurable
{
    bool IsIndistructable { get; }
    float MaxDurability { get; }
    float CurrentDurability { get; }
}

public class TestWeapon : Item, IEquipable, IDealDamage, IDurable
{
    bool isEquipped = false;
    public float MinDamage { get; init; }
    public float MaxDamage { get; init; }

    private bool _isIndistructable;
    public bool IsIndistructable => _isIndistructable;
    public float MaxDurability { get; }
    public float CurrentDurability { get; }


    private StatsModifier _statsModifier;
    public StatsModifier StatsModifier => _statsModifier;

    public TestWeapon(float minDamage, float maxDamage, StatsModifier statsModifier)
    {
        if (minDamage < 0 || maxDamage < 0)
        {
            throw new ArgumentOutOfRangeException("Arguemts must be positive numbers.");
        }

        if (statsModifier is null)
        {
            throw new ArgumentNullException("Stats modifier can't be null!");
        }


        if (minDamage < maxDamage)
        {
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
        }
        else
        {
            this.MinDamage = maxDamage;
            this.MaxDamage = minDamage;
        }

        _statsModifier = statsModifier;
    }

    public void OnEquip()
    {
        if (isEquipped)
        {
            Console.WriteLine("Weapon already equipped.");
            return;
        }

        EquipementEvents.InvokeOnItemEquip(StatsModifier);
        isEquipped = true;
    }

    public void OnUnequip()
    {
        if (!isEquipped)
        {
            Console.WriteLine("Unable to unequip: weapon is not equipped.");
            return;
        }

        EquipementEvents.InvokeOnItemUnequip(StatsModifier);
        isEquipped = false;
    }
}




public interface ILevel
{
    int Level { get; }
    int MaxLevel { get; }
    void IncreaseLevel();
}

public class HeroLevel : ILevel
{
    private const int MAX_LEVEL = 50;
    public int Level { get; private set; }
    public int MaxLevel => MAX_LEVEL;

    public HeroLevel()
    {
        Level = 1;
    }
    public void IncreaseLevel()
    {
        if (Level < MAX_LEVEL)
        {
            Level += 1;
        }
    }

    public override string ToString()
    {
        return $"Hero is at {Level} level!";
    }
}

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

