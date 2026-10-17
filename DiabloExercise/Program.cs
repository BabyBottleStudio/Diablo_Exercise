// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;




var goldManager = new GoldManager();
var experienceManager = new ExperienceManager();
var healthManger = new HealthResource(100, 100);
var manaManger = new ManaResource(100, 100);


var jovana = new Rogue(
    "Jovana",
    goldManager,
    experienceManager,
    healthManger,
    manaManger
    );

UserInterfaceDummy userInterface = new UserInterfaceDummy(

    healthManger.CurrentValue,
    healthManger.MaxValue,
    100,
    100,
    goldManager.GoldCount
    );

//var petar = new Warrior("Petar", new GoldManager(), new ExperienceManager());
//petar.goldManager.AddGold(50);

Console.WriteLine("MANA TESTING");


manaManger.Increase(50); // should return 100. Mana is on the maximum
Console.WriteLine(manaManger.CurrentValue);

userInterface.DisplayUI();

manaManger.Decrease(50); // should sucseed and return 100
Console.WriteLine(manaManger.CurrentValue);
userInterface.DisplayUI();

manaManger.Increase(1); // should return 51.
Console.WriteLine(manaManger.CurrentValue);
userInterface.DisplayUI();

manaManger.Decrease(100); // should return 51. Not enough mana!
Console.WriteLine(manaManger.CurrentValue);
userInterface.DisplayUI();

manaManger.Decrease(51); // should return 0.
Console.WriteLine(manaManger.CurrentValue);
userInterface.DisplayUI();

manaManger.Increase(1); // should return 1.
Console.WriteLine(manaManger.CurrentValue);
userInterface.DisplayUI();

manaManger.IncreaseMaxValue(50); // max should be 150
manaManger.Increase(120); // should increase and return 121
Console.WriteLine(manaManger.CurrentValue); // should write 121
userInterface.DisplayUI();

manaManger.Increase(120); // should increase and return 150
Console.WriteLine(manaManger.CurrentValue); // should write 150
userInterface.DisplayUI();


Console.WriteLine("HEALTH TESTING");

healthManger.Increase(50); // should not increase. Life is full
Console.WriteLine(healthManger.CurrentValue);
userInterface.DisplayUI();

healthManger.Decrease(99); // should return 1
Console.WriteLine(healthManger.CurrentValue);
userInterface.DisplayUI();

healthManger.IncreaseMaxValue(50);// max life should be 150
userInterface.DisplayUI();

healthManger.Increase(250); // should be 150.
userInterface.DisplayUI();

healthManger.Decrease(100); // should be 50
userInterface.DisplayUI();

healthManger.Decrease(500); // should be 0 and death of hero message should be displayed
userInterface.DisplayUI();



Console.WriteLine("Gold TESTING");



goldManager.AddGold(500); // should be 500
userInterface.DisplayUI();

goldManager.AddGold(1); // 501
userInterface.DisplayUI();

goldManager.Remove(500); // 1
userInterface.DisplayUI();


goldManager.Remove(2); // should return 1 - not enough gold
userInterface.DisplayUI();

goldManager.Remove(1); // 0
userInterface.DisplayUI();



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


// consumables
// healling potion -> heals 50hp
// full healing potion -> restores full health
// mana potion -> restores 50 mana
// full mana potion -> restores full mana
// potion of rejuvination -> heals 50hp and 50 mana
// potion of full rejuvination - restores full health and mana

// value
// metoda -> consume


// Items
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
// reduce current duration 
//
//
