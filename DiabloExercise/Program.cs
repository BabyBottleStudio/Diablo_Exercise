
using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;


HeroDataRepository heroData = new HeroDataRepository();

HeroFactory heroFactory = new HeroFactory(heroData);

var jovana = heroFactory.Instantiate("Rogue", "Jovana", out IUserInterface userInterface); // hm

Console.WriteLine(jovana);



HeroEvents.InvokeOnDecreaseLife(99);
HeroEvents.InvokeOnDecreaseMana(99);

var smallHealing = new SmallHealing();
Console.WriteLine(smallHealing);
smallHealing.Consume();

userInterface.DisplayUI();


var fullHealing = new FullHealing();
Console.WriteLine(fullHealing);
fullHealing.Consume();

userInterface.DisplayUI();

var smallMana = new SmallMana();
Console.WriteLine(smallMana);
smallMana.Consume();

userInterface.DisplayUI();

var fullMana = new FullMana();
Console.WriteLine(fullMana);
fullMana.Consume();

userInterface.DisplayUI();

HeroEvents.InvokeOnDecreaseLife(25);
HeroEvents.InvokeOnDecreaseMana(25);

userInterface.DisplayUI();


var smallRejuvenation = new SmallRejuvenation();
Console.WriteLine(smallRejuvenation);
smallRejuvenation.Consume();

userInterface.DisplayUI();

var fullRejuvenation = new FullRejuvenation();
Console.WriteLine(fullRejuvenation);
fullRejuvenation.Consume();

userInterface.DisplayUI();


var lightRadius = new LightRadiusManager(100f);

var modifier = new StatsModifier()
{

    LightRadius = 20f

};

var weapon = new TestWeapon(2, 4, modifier);
var weapon2 = new TestWeapon(2, 4, modifier);
var weapon3 = new TestWeapon(2, 4, modifier);
var weapon4 = new TestWeapon(2, 4, modifier);



weapon.OnEquip();
weapon2.OnEquip();
weapon3.OnEquip();
weapon4.OnEquip();
weapon.OnUnequip();
weapon2.OnUnequip();

Console.WriteLine(modifier);

//var goldManager = new GoldManager();
//var experienceManager = new ExperienceManager();
//var healthManger = new HealthResource(100, 100);
//var manaManager = new ManaResource(100, 100);


//var jovana = new Rogue(
//    "Jovana",
//    goldManager,
//    experienceManager,
//    healthManger,
//    manaManager
//    );





//var petar = new Warrior("Petar", new GoldManager(), new ExperienceManager());
//petar.goldManager.AddGold(50);

//Console.WriteLine("MANA TESTING");


//manaManager.Increase(50); // should return 100. Mana is on the maximum
//Console.WriteLine(manaManager.CurrentValue);

//userInterface.DisplayUI();

//manaManager.Decrease(50); // should sucseed and return 100
//Console.WriteLine(manaManager.CurrentValue);
//userInterface.DisplayUI();

//manaManager.Increase(1); // should return 51.
//Console.WriteLine(manaManager.CurrentValue);
//userInterface.DisplayUI();

//manaManager.Decrease(100); // should return 51. Not enough mana!
//Console.WriteLine(manaManager.CurrentValue);
//userInterface.DisplayUI();

//manaManager.Decrease(51); // should return 0.
//Console.WriteLine(manaManager.CurrentValue);
//userInterface.DisplayUI();

//manaManager.Increase(1); // should return 1.
//Console.WriteLine(manaManager.CurrentValue);
//userInterface.DisplayUI();

//manaManager.IncreaseMaxValue(50); // max should be 150
//manaManager.Increase(120); // should increase and return 121
//Console.WriteLine(manaManager.CurrentValue); // should write 121
//userInterface.DisplayUI();

//manaManager.Increase(120); // should increase and return 150
//Console.WriteLine(manaManager.CurrentValue); // should write 150
//userInterface.DisplayUI();


//Console.WriteLine("HEALTH TESTING");

//healthManger.Increase(50); // should not increase. Life is full
//Console.WriteLine(healthManger.CurrentValue);
//userInterface.DisplayUI();

//healthManger.Decrease(99); // should return 1
//Console.WriteLine(healthManger.CurrentValue);
//userInterface.DisplayUI();

//healthManger.IncreaseMaxValue(50);// max life should be 150
//userInterface.DisplayUI();

//healthManger.Increase(250); // should be 150.
//userInterface.DisplayUI();

//healthManger.Decrease(100); // should be 50
//userInterface.DisplayUI();

//healthManger.Decrease(500); // should be 0 and death of hero message should be displayed
//userInterface.DisplayUI();



//Console.WriteLine("Gold TESTING");



//goldManager.AddGold(500); // should be 500
//userInterface.DisplayUI();

//goldManager.AddGold(1); // 501
//userInterface.DisplayUI();

//goldManager.Remove(500); // 1
//userInterface.DisplayUI();


//goldManager.Remove(2); // should return 1 - not enough gold
//userInterface.DisplayUI();

//goldManager.Remove(1); // 0
//userInterface.DisplayUI();



//HeroEvents.InvokeOnGoldReceived(1500);
//HeroEvents.InvokeOnGoldRemoved(1000);

//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(1079);

//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(1);
//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(22200);
//Console.WriteLine(jovana.ToString());


//goldManager.AddGold(1200);
//goldManager.Remove(1300);
//goldManager.Remove(328);
//goldManager.Remove(432);
//goldManager.AddGold(732);

//healthManger.Decrease(50);
//healthManger.Decrease(2);

//manaManger.Decrease(25);
//manaManger.Increase(50);
//manaManger.Increase(2);
//manaManger.Decrease(500);
//manaManger.Decrease(1);
//manaManger.Increase(789);

//Console.WriteLine(jovana.ToString());

//userInterface.DisplayUI();



//HeroEvents.InvokeOnExpirienceReceived(1307);
//HeroEvents.InvokeOnExpirienceReceived(1214);
//HeroEvents.InvokeOnExpirienceReceived(1258);
//HeroEvents.InvokeOnExpirienceReceived(124);

//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(654);
//HeroEvents.InvokeOnExpirienceReceived(368);
//HeroEvents.InvokeOnExpirienceReceived(248);

//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(202);
//HeroEvents.InvokeOnExpirienceReceived(365);
//HeroEvents.InvokeOnExpirienceReceived(637);

//Console.WriteLine(jovana.ToString());

//HeroEvents.InvokeOnExpirienceReceived(10200);


////petar.goldManager.Remove(300);
////petar.goldManager.Remove(25);


//Console.WriteLine(jovana.ToString());
//Console.WriteLine(petar.ToString());



public abstract class Hero
{
    // Name - created once at the begining of the game. Not changable.
    private string _name;
    public string Name => _name;

    public virtual string HeroClass => GetType().Name;


    private IExperienceManager experienceManager;
    private IGoldManager goldManager;
    private IResource healthManager;
    private IResource manaManager;


    protected Hero(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager)
    {
        _name = name;
        this.goldManager = goldManager;
        this.experienceManager = experienceManager;
        this.healthManager = healthManager;
        this.manaManager = manaManager;
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
        healthManager.Decrease(lifePointsToLose);
    }

    public void SpendMana(float manaToSpend)
    {
        manaManager.Decrease(manaToSpend); // information of sucsess potentionally needed
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("****** Hero Table ******");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Class: {HeroClass}");
        sb.AppendLine($"Level: {experienceManager.Level}");
        sb.AppendLine($"Experience:{experienceManager.Experience}. XPforNextLevel:{experienceManager.NextLevelExperience}");
        sb.AppendLine($"Gold: {goldManager.GoldCount}");
        sb.AppendLine($"MaxLife:{healthManager.MaxValue}. CurrentLife: {healthManager.CurrentValue}");
        sb.AppendLine($"MaxMana:{manaManager.MaxValue}. CurrentMana: {manaManager.CurrentValue}");
        sb.AppendLine();

        return sb.ToString();
    }
}


public class Warrior : Hero
{

    public Warrior(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}
public class Rogue : Hero
{

    public Rogue(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}
public class Sorcerer : Hero
{

    public Sorcerer(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}



public interface IExperienceManager
{
    int Level { get; }
    long Experience { get; }
    long NextLevelExperience { get; }

    void Increase(long xpPointsToAdd);
    //void LevelUp();
}
public class ExperienceManager : IDisposable, IExperienceManager
{
    private const int MAX_LEVEL = 50;
    private int _level;
    public int Level => _level;

    private long _experience;
    public long Experience => _experience;
    public long NextLevelExperience => _level < MAX_LEVEL ? nextLevelThreshold[_level + 1] : 0;

    private long[] nextLevelThreshold;

    //private double _nextLevelAt;
    //public double NextLevelAt => _nextLevelAt;

    // event for leveling up


    public ExperienceManager()
    {
        _level = 1;
        _experience = 0;

        nextLevelThreshold = new long[MAX_LEVEL + 1];

        InitializeNextLevelTresholds();

        //HeroEvents.OnExperienceReceived += GainExperience;
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

        //PrintProgressList();
    }

    private void PrintProgressList()
    {
        for (int i = 0; i < nextLevelThreshold.Length; i++)
        {

            Console.WriteLine($"Level{i} - {nextLevelThreshold[i]}");
        }
    }

    public void Increase(long xpPointsToAdd)
    {
        if (_level == MAX_LEVEL) return;

        if (xpPointsToAdd < 0) return;

        _experience += xpPointsToAdd;

        while (_level < MAX_LEVEL && _experience >= nextLevelThreshold[_level + 1])
        {
            LevelUp();
        }

        HeroEvents.InvokeOnExpirienceIncreased(Experience);
    }


    private void LevelUp()
    {
        if (_level == MAX_LEVEL)
        {
            Console.WriteLine("Hero is already on the maximum level.");
            return;
        }

        _level++;



        // printing message
        string nextLevelXP = "Hero reached the maximum level!";

        if (_level < MAX_LEVEL)
        {
            nextLevelXP = $"{nextLevelThreshold[_level + 1]}";
        }
        Console.WriteLine($"Level:{_level}; XP:{_experience}; XPtNextLevel: {nextLevelXP}");


        HeroEvents.InvokeOnLevelUp();
    }

    public void Dispose()
    {
        //HeroEvents.OnExperienceReceived -= GainExperience;
    }
}




public interface IGoldManager
{
    int GoldCount { get; }

    void Add(int amountToAdd);
    bool Decrease(int amount);
}
public class GoldManager : IDisposable, IGoldManager
{
    private int _goldCount;

    public int GoldCount => _goldCount;

    public GoldManager()
    {
        //HeroEvents.OnGoldReceived += AddGold;
        //HeroEvents.OnGoldRemoved += Remove;
    }

    public void Add(int amountToAdd)
    {
        if (amountToAdd < 0) return;

        _goldCount += amountToAdd;

        HeroEvents.InvokeOnGoldCountChanged(_goldCount);
        // gold is added when:
        // player picks up the gold item in the dungeon
        // player sells the items in the shop

        // this should be subscribed to an event

    }

    public bool Decrease(int amountToDecrease)
    {
        // gold is removed when:
        // player drops it from the inventory
        // player buys items in the shop
        if (amountToDecrease < 0) return false;

        int difference = _goldCount - amountToDecrease;

        if (difference < 0)
            return false;


        _goldCount = difference;

        HeroEvents.InvokeOnGoldCountChanged(_goldCount);

        return true;
    }

    public void Dispose()
    {
        //HeroEvents.OnGoldReceived -= AddGold;
        //HeroEvents.OnGoldRemoved -= Remove;
    }
}


public interface IEquipable
{
    void OnEquip();
    void OnUnequip();
}

public interface IDamage
{
    float MinDamage { get; init; }
    float MaxDamage { get; init; }
}
public class TestWeapon : Item, IEquipable, IDamage
{
    bool isEquipped = false;
    public float MinDamage { get; init; }
    public float MaxDamage { get; init; }

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


        if (minDamage < MaxDamage)
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

public class StatsModifier
{
    public float LightRadius { get; init; }

    public float Strength { get; init; }
    public float Magic { get; init; }
    public float Dexterity { get; init; }
    public float Vitality { get; init; }

    public float ArmorClass { get; init; }
    public float ChanceToHit { get; init; }
    public float Damage { get; init; }

    public float ResistMagic { get; init; }
    public float ResistFire { get; init; }
    public float ResistLighting { get; init; }

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

        if (ResistMagic != 0) sb.AppendLine($"{FormatStat(ResistMagic)}% to Magic Resistance");
        if (ResistFire != 0) sb.AppendLine($"{FormatStat(ResistFire)}% to Fire Resistance");
        if (ResistLighting != 0) sb.AppendLine($"{FormatStat(ResistLighting)}% to Lightning Resistance");

        if (Life != 0) sb.AppendLine($"{FormatStat(Life)} to Life");
        if (Mana != 0) sb.AppendLine($"{FormatStat(Mana)} to Mana");

        return sb.ToString().TrimEnd(); // TrimEnd uklanja poslednji prazan red koji AppendLine napravi
    }

}


public interface ILightRadius
{
    float BaseLightRadius { get; }
    float ModifiedRadius { get; }
}

public class LightRadiusManager : ILightRadius, IDisposable
{
    float _baseLightRadius;
    public float BaseLightRadius => _baseLightRadius;

    float _radiusModifier;
    public float ModifiedRadius => Math.Clamp(_baseLightRadius * (_radiusModifier / 100f), 20f, 160);

        public LightRadiusManager(float startRadius)
    {
        if (startRadius < 0)
        {
            throw new ArgumentOutOfRangeException($"Start radius must be positive number.");
        }

        _baseLightRadius = startRadius;
        _radiusModifier = 100f;

        EquipementEvents.OnItemEquip += ModifyLightRadius;
        EquipementEvents.OnItemUnequip += RevertLightRadius;
    }


    private void ModifyLightRadius(StatsModifier statsModifier)
    {
        // -60% || -20% || 20% || 40%

        // amount should be 0.6 || 0.8 || 1.2 || 1.4
        float percent = statsModifier.LightRadius;

        _radiusModifier += percent;
        Console.WriteLine(this.ToString());
    }

    private void RevertLightRadius(StatsModifier statsModifier)
    {
        float percent = statsModifier.LightRadius * -1;

        _radiusModifier += percent;
        Console.WriteLine(this.ToString());
    }


    public override string ToString()
    {
        return $"Base light radius: {BaseLightRadius} || Modified light radius: {ModifiedRadius}";
    }

    public void Dispose()
    {
        EquipementEvents.OnItemEquip -= ModifyLightRadius;
        EquipementEvents.OnItemUnequip -= RevertLightRadius;
    }
}
