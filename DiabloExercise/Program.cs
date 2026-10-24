// See https://aka.ms/new-console-template for more information
using System;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;



HeroFactory heroFactory = new HeroFactory(
    new HeroData()
    );

var jovana = heroFactory.Instantiate("Rogue", "Jovana");

Console.WriteLine(jovana);

//float CalculateIncrease()
//{
//    var _random = new Random();

//    float Modifier = 1.5f;
//    float maxValue = 80f;

//    float baseValue = maxValue / 8; // 10
//    float min = Modifier * baseValue; // 1.5 * 10 = 15
//    float max = 3 * min; // 3 * 15 = 45

//    float output = min + _random.NextSingle() * (max - min);

//    return MathF.Round(output, 2);
//}

//for (int i = 0; i < 50; i++)
//{
//    Console.WriteLine(CalculateIncrease().ToString());
//}




var smallHealing = new SmallHealing();
Console.WriteLine(smallHealing);
smallHealing.Consume();

var fullHealing = new FullHealing();
Console.WriteLine(fullHealing);
fullHealing.Consume();


var smallMana = new SmallMana();
Console.WriteLine(smallMana);
smallMana.Consume();

var fullMana = new FullMana();
Console.WriteLine(fullMana);
fullMana.Consume();



var smallRejuvenation = new SmallRejuvenation();
Console.WriteLine(smallRejuvenation);
smallRejuvenation.Consume();

var fullRejuvenation = new FullRejuvenation();
Console.WriteLine(fullRejuvenation);
fullRejuvenation.Consume();







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

//UserInterfaceDummy userInterface = new UserInterfaceDummy(

//    healthManger.CurrentValue,
//    healthManger.MaxValue,
//    manaManager.CurrentValue,
//    manaManager.MaxValue,
//    goldManager.GoldCount
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

    private IExperienceManager experienceManager;
    private IGoldManager goldManager;
    private IResource healthManager;
    private IResource manaManager;


    /// <summary>
    /// Hero class returns the object's class name.
    /// Serves to be used in UI.
    /// Design choice: Every new class created will inherit this property and have a proper class name in the UI.
    /// </summary>
    public virtual string HeroClass => GetType().Name;

    protected Hero(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager)
    {
        _name = name;
        this.goldManager = goldManager;
        this.experienceManager = experienceManager;
        this.healthManager = healthManager;
        this.manaManager = manaManager;
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







public interface IExperienceManager
{
    int Level { get; }
    long Experience { get; }
    long NextLevelExperience { get; }

    void GainExperience(long xpPointsToAdd);
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

        HeroEvents.OnExperienceReceived += GainExperience;
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

    public void GainExperience(long xpPointsToAdd)
    {
        if (_level == MAX_LEVEL) return;

        if (xpPointsToAdd < 0) return;

        _experience += xpPointsToAdd;

        while (_level < MAX_LEVEL && _experience >= nextLevelThreshold[_level + 1])
        {
            LevelUp();
        }
    }


    private void LevelUp()
    {
        if (_level == MAX_LEVEL) return;

        _level++;

        HeroEvents.InvokeOnLevelUp();


        // printing message
        string nextLevelXP = "Hero reached the maximum level!";

        if (_level < MAX_LEVEL)
        {
            nextLevelXP = $"{nextLevelThreshold[_level + 1]}";
        }

        Console.WriteLine($"Level:{_level}; XP:{_experience}; XPtNextLevel: {nextLevelXP}");
    }

    public void Dispose()
    {
        HeroEvents.OnExperienceReceived -= GainExperience;
    }
}

public interface IGoldManager
{
    int GoldCount { get; }

    void AddGold(int amountToAdd);
    bool Remove(int amountToRemove);
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

    public void AddGold(int amountToAdd)
    {
        if (amountToAdd < 0) return;

        _goldCount += amountToAdd;


        Console.WriteLine($"Hero received {amountToAdd} gold, and now has {_goldCount}");
        HeroEvents.InvokeOnGoldCountChanged(_goldCount);
        // gold is added when:
        // player picks up the gold item in the dungeon
        // player sells the items in the shop

        // this should be subscribed to an event

    }

    public bool Remove(int amountToRemove)
    {
        // gold is removed when:
        // player drops it from the inventory
        // player buys items in the shop
        if (amountToRemove < 0) return false;

        var difference = _goldCount - amountToRemove;

        if (difference < 0)
        {
            Console.WriteLine($"Not enough gold!");

            return false;
        }

        _goldCount = difference;
        HeroEvents.InvokeOnGoldCountChanged(_goldCount);

        Console.WriteLine($"Hero deducted {amountToRemove} gold, and now has {_goldCount}");

        return true;
    }

    public void Dispose()
    {
        //HeroEvents.OnGoldReceived -= AddGold;
        //HeroEvents.OnGoldRemoved -= Remove;
    }
}

public class Rogue : Hero
{

    public Rogue(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}



public class Warrior : Hero
{

    public Warrior(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}

public class Sorcerer : Hero
{

    public Sorcerer(string name, IGoldManager goldManager, IExperienceManager experienceManager, IResource healthManager, IResource manaManager) : base(name, goldManager, experienceManager, healthManager, manaManager)
    {
    }
}

public class HeroFactory
{
    private readonly string[] flags = new string[] { "Warrior", "Rogue", "Sorcerer" };

    private readonly HeroData heroData;

    public HeroFactory(HeroData heroData)
    {
        this.heroData = heroData;
    }

    public Hero? Instantiate(string flag, string heroName)
    {
        if (string.IsNullOrEmpty(flag))
        {
            throw new ArgumentNullException($"{GetType().Name} - Instantiate method: flag is null");
        }

        if (!flags.Contains(flag))
        {
            throw new ArgumentException("Flag not recognized");
        }

        if (string.IsNullOrEmpty(heroName))
        {
            heroName = $"Some {flag}";
        }


        float healthModifier = 1f;
        float manaModifier = 1f;


        var goldManager = new GoldManager();
        var experienceManager = new ExperienceManager();


        switch (flag)
        {
            case "Warrior":
                healthModifier = heroData.WarriorData["LifeModifier"];
                manaModifier = heroData.WarriorData["ManaModifier"];
                break;

            case "Rogue":

                healthModifier = heroData.RogueData["LifeModifier"];
                manaModifier = heroData.RogueData["ManaModifier"];
                break;

            case "Sorcerer":
                healthModifier = heroData.SorcererData["LifeModifier"];
                manaModifier = heroData.SorcererData["ManaModifier"];
                break;
        }

        var healthManger = new HealthResource(100, 100, healthModifier, new Random());
        var manaManager = new ManaResource(100, 100, manaModifier, new Random());

        switch (flag)
        {
            case "Warrior":

                return new Warrior(heroName, goldManager, experienceManager, healthManger, manaManager);

            case "Rogue":

                return new Rogue(heroName, goldManager, experienceManager, healthManger, manaManager);


            case "Sorcerer":
                return new Sorcerer(heroName, goldManager, experienceManager, healthManger, manaManager);
            default:
                return null;
        }



    }
}

/*
 * Potions of Healing and Mana restore life and mana as stated below. Potions of Rejuvenation are basically treated as one Potion of Mana and one Potion of Healing. For information on how scrolls of healing and the Healing spell work, see chapter 4.1.2.

Potion of Healing: bonus · maxlife/8 to bonus · 3·maxlife/8

Potion of Mana: bonus · maxmana/8 to bonus · 3·maxmana/8

    Any value in the range is equally probable.

    The bonus is summarized in the table below.

    maxlife and maxmana are the modified values. If they are negative, the amount added will also be negative and in fact subtract life or mana.

Type 	    Warrior 	Rogue 	Sorcerer 	Monk    	Bard    	Barbarian
Healing 	2.0 	    1.5 	1.0 	    1.51    	1.51    	2.0
Mana 	    1.0 	    1.5 	2.0 	    1.51    	1.51    	1.0

1 When using a Potion of Rejuvenation the bonus for a Monk and a Bard is 1.0 due to a bug in Hellfire.
 * 
 */

/*
public class Modifiers // this should be injected to hero 
{
    private float _healingModifier;

    public Modifiers(float healingModifier)
    {
        _healingModifier = healingModifier;
    }

    public float HealingModifier { get => _healingModifier; }
}
*/


public abstract class Item
{
    // this class should enable using the potion as an item in the inventory
    // not sure how is it going to work yet
}



public abstract class Potion : Item
{
    // potion should send a message, and hero class should calculate the effect

    // "Potion of Healing"
    // "Potion of Full Mana"
    public abstract string Name { get; }
    public abstract string Description { get; }

    protected const string _instruction = "Right click to use";
    public string Instruction => _instruction;
    public abstract void Consume();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Name);
        sb.AppendLine(Description);
        sb.AppendLine(Instruction);
        return sb.ToString();
    }
}

public class SmallHealing : Potion
{
    public override string Name => "Potion of Healing";
    public override string Description => "Recover Partial Life";


    public override void Consume()
    {
        PotionEvents.InvokeOnHealthPotionConsumed();
    }
}

public class FullHealing : Potion
{
    public override string Name => "Potion of Full Healing";
    public override string Description => "Fully Recover Life";

    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
    }
}

public class SmallMana : Potion
{
    public override string Name => "Potion of Mana";
    public override string Description => "Recover Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnManaPotionConsumed();
    }
}

public class FullMana : Potion
{
    public override string Name => "Potion of Full Mana";
    public override string Description => "Fully Recover Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
    }
}

public class SmallRejuvenation : Potion
{
    public override string Name => "Potion of Rejuvenation";
    public override string Description => "Recover Life and Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnHealthPotionConsumed();
        PotionEvents.InvokeOnManaPotionConsumed();
    }
}

public class FullRejuvenation : Potion
{
    public override string Name => "Potion of Full Rejuvenation";
    public override string Description => "Fully Recover Life and Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
        PotionEvents.InvokeOnFullManaPotionConsumed();
    }
}



// Items - base abstract class for all items that can go into the inventory - or interface?

// consumables - IConsumable
// healling potion -> heals 50hp
// full healing potion -> restores full health
// mana potion -> restores 50 mana
// full mana potion -> restores full mana
// potion of rejuvination -> heals 50hp and 50 mana
// potion of full rejuvination - restores full health and mana

// value
// metoda -> consume


// hero equipment - IEquipable (equip, unequip)
// - Weapons - deals damage to the enemies
//      - OneHand weapon
//          - Sword
//      - TwoHanded weapon
//          - Axe
//          - Staff
//          - Bow
// - Shields - protect from damage + special stuff
// - Head pieces - protect from damage + special stuff
// - Body wearables - protect from damage + special stuff
// - Amulet - special attributes
// - Ring - special attributes
//
// properties:
// name
// armor
// minimum damage
// maximum damage
// max duration
// current duration
// special affect
// requirements
// reduce current duration (get damage)
//
//
