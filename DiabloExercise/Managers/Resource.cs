public interface IResource
{
    public float CurrentValue { get; }
    public float CurrentMaxValue { get; }
    public float ItemsBonus { get; }
    public void Decrease(float value);
    void UpdateBaseMaxValue(float value);
}

public abstract class Resource : IResource, IDisposable
{
    protected float _currentValue;
    public float CurrentValue => _currentValue;

    protected float _baseMaxValue;
    public float CurrentMaxValue => _baseMaxValue + _itemsBonus;

    public void UpdateBaseMaxValue(float value)
    {
        float currentValuePercent = currentValueToPercent(_currentValue, CurrentMaxValue);
        _baseMaxValue = value;

        _currentValue = percentToCurrentValue(CurrentMaxValue, currentValuePercent);
    }


    private const float MAX_VALUE_INCREMENT = 2f; // this should be in the HeroDataRepository and is different for hero types

    protected float _itemsBonus;
    public float ItemsBonus => _itemsBonus;


    protected ResourcesCalculator resourcesCalculator;

    protected Func<float, float, float> currentValueToPercent = (currentValue, maxValue) => maxValue != 0 ? (currentValue * 100) / maxValue : 0;
    protected Func<float, float, float> percentToCurrentValue = (currentMaxValue, valueInPercents) => currentMaxValue * valueInPercents / 100;

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


    protected abstract string _resourceName { get; }

    private Random _random;

    // bonus · maxlife/8 to bonus · 3·maxlife/8
    protected abstract void AddItemsBonus(StatsModifier statsModifier);
    protected abstract void RemoveItemsBonus(StatsModifier statsModifier);
    private float CalculateIncrease()
    {
        float baseValue = CurrentMaxValue / 8;
        float min = _itemsBonus * baseValue;
        float max = 3 * min;

        float output = min + _random.NextSingle() * (max - min);

        return MathF.Round(output, 2);
    }

    public Resource(ResourcesCalculator resourcesCalculator, Random random)
    {
        if (resourcesCalculator is null || random is null)
        {
            throw new ArgumentNullException("Null argument error");
        }

        this.resourcesCalculator = resourcesCalculator;

        //_baseMaxValue = startMaxValue;

        //if (startValue <= startMaxValue)
        //    _currentValue = startValue;
        //else
        //    _currentValue = startMaxValue;

        //Console.WriteLine($"modifier = {Modifier}");

        _random = random;
        HeroEvents.OnLevelUp += OnLevelUp;

        PotionEvents.OnFullRejuvenationPotionConsumed += Restore;

        EquipementEvents.OnItemEquip += AddItemsBonus;
        EquipementEvents.OnItemUnequip += RemoveItemsBonus;
    }


    protected void Increase()
    {
        //if (value <= 0) return;

        if (CurrentValue == CurrentMaxValue)
        {
            Console.WriteLine($"{_resourceName} is at maximum: {CurrentValue}");
            return;
        }

        var value = CalculateIncrease();

        var valueBeforeIncrease = _currentValue;

        var amountToMax = _baseMaxValue - valueBeforeIncrease;

        //if (_life > _maxLife) return;

        _currentValue = Math.Min(_currentValue + value, _baseMaxValue);

        Console.WriteLine($"Hero had {valueBeforeIncrease} {_resourceName}, and after receiving {value} has {_currentValue} {_resourceName}. Wasted:{(value <= amountToMax ? 0 : value - amountToMax)}");

        // event 
        InvokeOnCurrentValueChanged(_currentValue);
    }



    protected abstract void InvokeOnCurrentValueChanged(float currentValue);
    protected abstract void InvokeOnMaxValueChanged(float maxValue);

    protected void IncreaseMaxValue(float amount)
    {
        if (amount <= 0) return;

        var startValue = CurrentMaxValue;

        _baseMaxValue += amount;

        Console.WriteLine($"Hero's maximum {_resourceName} was {startValue}, and now is {CurrentMaxValue}.");

        InvokeOnMaxValueChanged(CurrentMaxValue);
    }

    /// <summary>
    /// This method is invoked when hero is leveling up. <br/>
    /// Method is called by the respective event and serves to increase max health and heals hero to 100% health.
    /// </summary>
    private void OnLevelUp()
    {
        IncreaseMaxValue(MAX_VALUE_INCREMENT);
        Restore(); // restore to original value -- heal for health and refil mana to 100%
    }
    protected void Restore()
    {
        var startValue = _currentValue;

        _currentValue = _baseMaxValue; // restore to original value -- heal for health and refil mana to 100%
        InvokeOnCurrentValueChanged(_currentValue);
        Console.WriteLine($"Hero had {startValue} {_resourceName}, and after restoring has {_currentValue} {_resourceName}.");
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

        EquipementEvents.OnItemEquip += AddItemsBonus;
        EquipementEvents.OnItemUnequip += RemoveItemsBonus;

        HandleDispose();
    }

    public override string ToString() => $"Max {_resourceName}: {CurrentMaxValue}. Current {_resourceName}: {CurrentValue}";

}




public class HealthResource : Resource
{
    protected override string _resourceName => "life";
    private bool isHeroDead = false;



    public HealthResource(ResourcesCalculator resourcesCalculator, Random random) : base(resourcesCalculator, random)
    {
        PotionEvents.OnFullHealthPotionConsumed += Restore;
        PotionEvents.OnHealthPotionConsumed += Increase;
        HeroEvents.OnDecreaseLife += Decrease;
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
    protected override void AddItemsBonus(StatsModifier statsModifier)
    {
        var lifeBonus = statsModifier.Life;
        if (lifeBonus! > 0)
        {
            float currentValuePercent = currentValueToPercent(_currentValue, CurrentMaxValue);
            _itemsBonus += lifeBonus;
            _currentValue = percentToCurrentValue(CurrentMaxValue, currentValuePercent);
        }
    }

    protected override void RemoveItemsBonus(StatsModifier statsModifier)
    {
        var lifeBonus = statsModifier.Life;
        if (lifeBonus! > 0)
        {
            float currentValuePercent = currentValueToPercent(_currentValue, CurrentMaxValue);
            _itemsBonus += -1 * lifeBonus;
            _currentValue = percentToCurrentValue(CurrentMaxValue, currentValuePercent);
        }

        // vitality
    }
    protected override void HandleDispose()
    {
        PotionEvents.OnFullHealthPotionConsumed -= Restore;
        PotionEvents.OnHealthPotionConsumed -= Increase;
        HeroEvents.OnDecreaseLife -= Decrease;
    }
}


public class ManaResource : Resource
{
    protected override string _resourceName => "mana";


    public ManaResource(ResourcesCalculator resourcesCalculator, Random random) : base(resourcesCalculator, random)
    {
        PotionEvents.OnFullManaPotionConsumed += Restore;
        PotionEvents.OnManaPotionConsumed += Increase;
        HeroEvents.OnDecreaseMana += Decrease;
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
    protected override void AddItemsBonus(StatsModifier statsModifier)
    {
        var manaBonus = statsModifier.Mana;
        if (manaBonus! > 0)
        {
            float currentValuePercent = currentValueToPercent(_currentValue, CurrentMaxValue);
            _itemsBonus += manaBonus;
            _currentValue = percentToCurrentValue(CurrentMaxValue, currentValuePercent);

        }

        // kako se resava vitality kad se promeni?
    }

    protected override void RemoveItemsBonus(StatsModifier statsModifier)
    {
        var manaBonus = statsModifier.Mana;
        if (manaBonus! > 0)
        {
            float currentValuePercent = currentValueToPercent(_currentValue, CurrentMaxValue);
            _itemsBonus += -1 * manaBonus;
            _currentValue = percentToCurrentValue(CurrentMaxValue, currentValuePercent);
        }

        // vitality
    }
    protected override void HandleDispose()
    {
        PotionEvents.OnFullManaPotionConsumed -= Restore;
        PotionEvents.OnManaPotionConsumed -= Increase;
        HeroEvents.OnDecreaseMana -= Decrease;
    }
}
