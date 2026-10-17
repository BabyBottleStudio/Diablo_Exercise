// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;




var goldManager = new GoldManager();
var experienceManager = new ExperienceManager();
var healthManger = new HealthManager(100, 100);


var jovana = new Rogue(
    "Jovana",
    goldManager,
    experienceManager,
    healthManger
    );

UserInterfaceDummy userInterface = new UserInterfaceDummy(

    healthManger.Life,
    healthManger.MaxLife,
    100,
    100,
    goldManager.GoldCount
    );

//var petar = new Warrior("Petar", new GoldManager(), new ExperienceManager());
//petar.goldManager.AddGold(50);



//HeroEvents.InvokeOnGoldReceived(1500);
//HeroEvents.InvokeOnGoldRemoved(1000);

Console.WriteLine(jovana.ToString());

HeroEvents.InvokeOnExpirienceReceived(1079);

Console.WriteLine(jovana.ToString());

HeroEvents.InvokeOnExpirienceReceived(1);
Console.WriteLine(jovana.ToString());

HeroEvents.InvokeOnExpirienceReceived(22200);
Console.WriteLine(jovana.ToString());


goldManager.AddGold(1200);
goldManager.Remove(1300);
goldManager.Remove(328);
goldManager.Remove(432);
goldManager.AddGold(732);

healthManger.GetDamage(50);

Console.WriteLine(jovana.ToString());

userInterface.DisplayUI();



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
    private IHealthManager healthManager;


    /// <summary>
    /// Hero class returns the object's class name.
    /// Serves to be used in UI.
    /// Design choice: Every new class created will inherit this property and have a proper class name in the UI.
    /// </summary>
    public virtual string HeroClass => GetType().Name;

    protected Hero(string name, IGoldManager goldManager, IExperienceManager experienceManager, IHealthManager healthManager)
    {
        _name = name;
        this.goldManager = goldManager;
        this.experienceManager = experienceManager;
        this.healthManager = healthManager;
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
        sb.AppendLine($"MaxLife:{healthManager.MaxLife}. CurrentLife: {healthManager.Life}");
        sb.AppendLine();

        return sb.ToString();
    }
}

public interface IHealthManager
{
    public float Life { get; }
    public float MaxLife { get; }

}
public class HealthManager : IDisposable, IHealthManager // same class could be used for life and mana
{
    private float _life;
    public float Life => _life;

    private float _maxLife;
    public float MaxLife => _maxLife;

    private const float MAX_LIFE_INCREMENT = 2f;

    public HealthManager(float startHealth, float startMaxHealth)
    {
        _life = startHealth;
        _maxLife = startMaxHealth;

        HeroEvents.OnLevelUp += OnLevelUp;
    }

    public void Heal(float lifeToAdd)
    {
        if (lifeToAdd <= 0) return;

        var lifeBeforeHeal = _life;

        //if (_life > _maxLife) return;

        _life = Math.Min(_life + lifeToAdd, _maxLife);

        Console.WriteLine($"Hero had {lifeBeforeHeal}, and after healing {lifeToAdd} has {_life} health points.");

        // event 
        HeroEvents.InvokeOnLifeAmountChanged(_life);
    }

    public void IncreaseMaxLife(float amountToIncrease)
    {
        if (amountToIncrease <= 0) return;

        _maxLife += amountToIncrease;

        HeroEvents.InvokeOnMaxLifeChanged(_maxLife);
    }

    /// <summary>
    /// This method is invoked when hero is leveling up. <br/>
    /// Method is called by the respective event and serves to increase max health and heals hero to 100% health.
    /// </summary>
    public void OnLevelUp()
    {
        IncreaseMaxLife(MAX_LIFE_INCREMENT);
        _life = _maxLife; // heal
        HeroEvents.InvokeOnLifeAmountChanged(_life);
    }

    public void GetDamage(float amountOfLifeToLose)
    {
        if (amountOfLifeToLose <= 0) return;

        var lifeBeforeDamage = _life;

        _life = Math.Max(_life - amountOfLifeToLose, 0);

        HeroEvents.InvokeOnLifeAmountChanged(_life);

        Console.WriteLine($"Hero had {lifeBeforeDamage}, and after receiving damage of {amountOfLifeToLose} has {_life} life points.");

        if (_life == 0)
        {
            Console.WriteLine($"Hero is dead! Game Over");
            HeroEvents.InvokeOnHeroDeath();
        }
    }

    public void Dispose()
    {
        HeroEvents.OnLevelUp -= OnLevelUp;
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
    public Rogue(string name, IGoldManager goldManager, IExperienceManager experienceManager, IHealthManager healthManager) : base(name, goldManager, experienceManager, healthManager)
    {
    }
}



public class Warrior : Hero
{
    public Warrior(string name, IGoldManager goldManager, IExperienceManager experienceManager, IHealthManager healthManager) : base(name, goldManager, experienceManager, healthManager)
    {
    }
}


public class HeroEvents
{
    public static Action<long>? OnExperienceReceived;
    public static event Action? OnLevelUp;

    public static event Action<int>? OnGoldReceived;
    public static event Func<int, bool>? OnGoldRemoved;
    public static event Action<int>? OnGoldCountChanged;

    public static event Action<float>? OnLifeAmountChanged;
    public static event Action<float>? OnMaxLifeChanged;

    public static event Action? OnHeroDeath;

    public static void InvokeOnExpirienceReceived(long expirienceToReceive)
    {
        OnExperienceReceived?.Invoke(expirienceToReceive);
    }
    public static void InvokeOnLevelUp()
    {
        OnLevelUp?.Invoke();
    }

    public static void InvokeOnGoldReceived(int amountToAdd)
    {
        OnGoldReceived?.Invoke(amountToAdd);
    }

    public static void InvokeOnGoldRemoved(int amountToRemove)
    {
        OnGoldRemoved?.Invoke(amountToRemove);
    }

    public static void InvokeOnGoldCountChanged(int newAmount)
    {
        OnGoldCountChanged?.Invoke(newAmount);
    }

    public static void InvokeOnLifeAmountChanged(float updatedLife)
    {
        OnLifeAmountChanged?.Invoke(updatedLife);
    }

    public static void InvokeOnMaxLifeChanged(float newMaxLife)
    {
        OnMaxLifeChanged?.Invoke(newMaxLife);
    }

    internal static void InvokeOnHeroDeath()
    {
        OnHeroDeath?.Invoke();
    }
}

public interface IUserInterface
{
    void DisplayUI();
}

public class UserInterfaceDummy : IDisposable, IUserInterface
{
    private float _life;
    private float _maxLife;

    private float _manaBar;
    private float _maxMana;

    private float _goldBar;

    public UserInterfaceDummy(float initialLife, float initialMaxLife, float initialMana, float initialMaxMana, float initialGold)
    {
        _life = initialLife;
        _maxLife = initialMaxLife;
        _manaBar = initialMana;
        _maxMana = initialMaxMana;
        _goldBar = initialGold;


        HeroEvents.OnLifeAmountChanged += UptadeLifeBarUI;
        HeroEvents.OnGoldCountChanged += UpdateGoldUI;

        HeroEvents.OnMaxLifeChanged += UpdateMaxLife;
    }

    void UptadeManaBarUI() { }
    void UpdateGoldUI(int newGoldCount)
    {
        _goldBar = newGoldCount;
    }
    void UpdateMaxLife(float newMaxLife)
    {
        _maxLife = newMaxLife;
    }
    void UptadeLifeBarUI(float newLifeAmount)
    {
        _life = newLifeAmount;
    }

    public void DisplayUI()
    {
        Console.WriteLine("* I N T E R F A C E *");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Life:{_life} - MaxLife:{_maxLife} - HealthBar:{_life * 100 / _maxLife:F2}%");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"Mana:{_manaBar} * ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"Gold:{_goldBar}");
        Console.ResetColor();
    }

    public void Dispose()
    {
        HeroEvents.OnLifeAmountChanged -= UptadeLifeBarUI;
        HeroEvents.OnGoldCountChanged -= UpdateGoldUI;

        HeroEvents.OnMaxLifeChanged -= UpdateMaxLife;
    }
}