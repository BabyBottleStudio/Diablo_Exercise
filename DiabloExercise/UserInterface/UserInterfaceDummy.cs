public class UserInterfaceDummy : IDisposable, IUserInterface
{
    private float _life;
    private float _maxLife;

    private float _manaBar;
    private float _maxMana;

    private float _goldBar;

    private long _expirience;

    public UserInterfaceDummy(float initialLife, float initialMaxLife, float initialMana, float initialMaxMana, float initialGold)
    {
        _life = initialLife;
        _maxLife = initialMaxLife;
        _manaBar = initialMana;
        _maxMana = initialMaxMana;
        _goldBar = initialGold;
        _expirience = 0;

        HeroEvents.OnLifeAmountChanged += UptadeLifeBarUI;
        HeroEvents.OnMaxLifeChanged += UpdateMaxLife;

        HeroEvents.OnManaAmountChanged += UptadeManaBarUI;
        HeroEvents.OnMaxManaChanged += UptadeMaxMana;

        HeroEvents.OnGoldCountChanged += UpdateGoldUI;

        HeroEvents.OnExperienceIncreased += UpdateXPDisplay;
    }

    void UpdateXPDisplay(long xpToDisplay)
    {
        _expirience = xpToDisplay;
    }


    void UptadeManaBarUI(float amount)
    {
        _manaBar = amount;
    }
    void UptadeMaxMana(float amount)
    {
        _maxMana = amount;
    }
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


    Func<float, float, float> percentBar = (currentValue, maxValue) => currentValue * 100 / maxValue;

    public void DisplayUI()
    {
        Console.WriteLine("* I N T E R F A C E *");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Expirience: {_expirience}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Life:{_life} - MaxLife:{_maxLife} - HealthBar:{percentBar(_life, _maxLife):F2}%");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Mana:{_manaBar} - MaxMana:{_maxMana} - ManaBar:{percentBar(_manaBar, _maxMana):F2}%");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold:{_goldBar}");
        Console.WriteLine();
        Console.ResetColor();
    }

    public void Dispose()
    {
        HeroEvents.OnLifeAmountChanged -= UptadeLifeBarUI;
        HeroEvents.OnMaxLifeChanged -= UpdateMaxLife;

        HeroEvents.OnManaAmountChanged -= UptadeManaBarUI;
        HeroEvents.OnMaxManaChanged -= UptadeMaxMana;

        HeroEvents.OnGoldCountChanged -= UpdateGoldUI;

        HeroEvents.OnExperienceIncreased -= UpdateXPDisplay;
    }
}