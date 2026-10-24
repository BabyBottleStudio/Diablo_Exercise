public interface IResource
{
    public float CurrentValue { get; }
    public float MaxValue { get; }
}

public abstract class Resource : IResource, IDisposable
{
    protected float _currentValue;
    public float CurrentValue => _currentValue;

    protected float _maxValue;
    public float MaxValue => _maxValue;

    private const float MAX_VALUE_INCREMENT = 2f;

    private float Modifier { get; init; }
    protected abstract string _resourceName { get; }

    private Random _random;

    // bonus · maxlife/8 to bonus · 3·maxlife/8

    private float CalculateIncrease()
    {
        float baseValue = MaxValue / 8; 
        float min = Modifier * baseValue;
        float max = 3 * min;

        float output = min + _random.NextSingle() * (max - min);

        return MathF.Round(output, 2);
    }

    public Resource(float startValue, float startMaxValue, float modifier, Random random)
    {
        _currentValue = startValue;
        _maxValue = startMaxValue;
        Modifier = modifier;
        //Console.WriteLine($"modifier = {Modifier}");
        _random = random;
        HeroEvents.OnLevelUp += OnLevelUp;

        PotionEvents.OnFullRejuvenationPotionConsumed += Restore;
        Modifier = modifier;
    }


    public void Increase()
    {
        //if (value <= 0) return;

        if (CurrentValue == MaxValue)
        {
            Console.WriteLine($"{_resourceName} is at maximum: {CurrentValue}");
            return;
        }

        var value = CalculateIncrease();

        var valueBeforeIncrease = _currentValue;

        var amountToMax = _maxValue - valueBeforeIncrease;

        //if (_life > _maxLife) return;

        _currentValue = Math.Min(_currentValue + value, _maxValue);

        Console.WriteLine($"Hero had {valueBeforeIncrease} {_resourceName}, and after receiving {value} has {_currentValue} {_resourceName}. Wasted:{(value <= amountToMax ? 0 : value - amountToMax)}");

        // event 
        InvokeOnCurrentValueChanged(_currentValue);
    }



    protected abstract void InvokeOnCurrentValueChanged(float currentValue);
    protected abstract void InvokeOnMaxValueChanged(float maxValue);

    public void IncreaseMaxValue(float amount)
    {
        if (amount <= 0) return;

        var startValue = MaxValue;

        _maxValue += amount;

        Console.WriteLine($"Hero's maximum {_resourceName} was {startValue}, and now is {MaxValue}.");

        InvokeOnMaxValueChanged(MaxValue);
    }

    /// <summary>
    /// This method is invoked when hero is leveling up. <br/>
    /// Method is called by the respective event and serves to increase max health and heals hero to 100% health.
    /// </summary>
    public void OnLevelUp()
    {
        IncreaseMaxValue(MAX_VALUE_INCREMENT);
        Restore(); // restore to original value -- heal for health and refil mana to 100%
    }
    protected void Restore()
    {
        _currentValue = _maxValue; // restore to original value -- heal for health and refil mana to 100%
        InvokeOnCurrentValueChanged(_currentValue);
    }

    public void Decrease(float amount) // napravi da ova metoda poziva
    {
        // life
        // ukoliko je amount veci od CurrentValue -> CurrentValue postaje 0 -> heroj umire

        // mana
        // ukoliko je amount veci od CurrentValue -> CurrenValue ostaje koliki jeste -> mana se ne trosi i spell se ne ostvaruje

        if (amount <= 0) return;

        HandleDecrease(CurrentValue, amount);
    }

    protected virtual void DisplayDecreaseMessage(float originalValue, float decreaseAmount)
    {
        Console.WriteLine($"Hero had {originalValue} {_resourceName}, and after decrease of {decreaseAmount} has {_currentValue} {_resourceName}.");
    }

    protected abstract void HandleDecrease(float amountBeforeDecrease, float amount);
    protected virtual void HandleDispose() { }

    public void Dispose()
    {
        HeroEvents.OnLevelUp -= OnLevelUp;
        PotionEvents.OnFullRejuvenationPotionConsumed -= Restore;

        HandleDispose();
    }
}


public class HealthResource : Resource
{
    protected override string _resourceName => "health";
    private bool isHeroDead = false;




    public HealthResource(float startValue, float startMaxValue, float modifier, Random random) : base(startValue, startMaxValue, modifier, random)
    {
        PotionEvents.OnFullHealthPotionConsumed += Restore;
        PotionEvents.OnHealthPotionConsumed += Increase;
    }

    protected override void HandleDecrease(float amountBeforeDecrease, float amount)
    {
        // life
        // ukoliko je amount veci od CurrentValue -> CurrentValue postaje 0 -> heroj umire


        _currentValue = Math.Max(CurrentValue - amount, 0);

        InvokeOnCurrentValueChanged(_currentValue);

        DisplayDecreaseMessage(amountBeforeDecrease, amount);

        HandleZeroValue();
    }

    protected override void InvokeOnCurrentValueChanged(float currentValue)
    {
        HeroEvents.InvokeOnLifeAmountChanged(currentValue);
    }

    protected override void InvokeOnMaxValueChanged(float maxValue)
    {
        HeroEvents.InvokeOnMaxLifeChanged(maxValue);
    }

    protected void HandleZeroValue()
    {
        if (CurrentValue > 0) return;

        if (!isHeroDead)
        {
            isHeroDead = true;
            Console.WriteLine($"Hero is dead! Game Over");
            HeroEvents.InvokeOnHeroDeath();
        }
    }

    protected override void HandleDispose()
    {

        PotionEvents.OnFullHealthPotionConsumed -= Restore;
        PotionEvents.OnHealthPotionConsumed -= Increase;
    }
}


public class ManaResource : Resource
{
    protected override string _resourceName => "mana";



    public ManaResource(float startValue, float startMaxValue, float modifier, Random random) : base(startValue, startMaxValue, modifier, random)
    {
        PotionEvents.OnFullManaPotionConsumed += Restore;
        PotionEvents.OnManaPotionConsumed += Increase;

    }

    protected override void InvokeOnCurrentValueChanged(float currentValue)
    {
        HeroEvents.InvokeOnManaAmountChanged(currentValue);
    }

    protected override void InvokeOnMaxValueChanged(float maxValue)
    {
        HeroEvents.InvokeOnMaxManaChanged(maxValue);
    }

    protected override void HandleDecrease(float amountBeforeDecrease, float amount)
    {
        // mana
        // ukoliko je amount veci od CurrentValue -> CurrenValue ostaje koliki jeste -> mana se ne trosi i spell se ne ostvaruje


        if (amount > CurrentValue)
        { // if amount is larger than current mana available

            Console.WriteLine("Not enough mana");
            return;
        }

        _currentValue -= amount;

        InvokeOnCurrentValueChanged(_currentValue);

        DisplayDecreaseMessage(amountBeforeDecrease, amount);
    }

    protected override void HandleDispose()
    {
        PotionEvents.OnFullManaPotionConsumed -= Restore;
        PotionEvents.OnManaPotionConsumed -= Increase;
    }
}
