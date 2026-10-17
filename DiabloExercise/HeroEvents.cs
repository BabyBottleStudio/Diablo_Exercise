// See https://aka.ms/new-console-template for more information
public class HeroEvents
{
    public static Action<long>? OnExperienceReceived;
    public static event Action? OnLevelUp;

    //public static event Action<int>? OnGoldReceived;
    //public static event Func<int, bool>? OnGoldRemoved;
    public static event Action<int>? OnGoldCountChanged;

    public static event Action<float>? OnLifeAmountChanged;
    public static event Action<float>? OnMaxLifeChanged;

    public static event Action<float>? OnManaAmountChanged;
    public static event Action<float>? OnMaxManaChanged;

    public static event Action? OnHeroDeath;

    public static void InvokeOnExpirienceReceived(long expirienceToReceive)
    {
        OnExperienceReceived?.Invoke(expirienceToReceive);
    }
    public static void InvokeOnLevelUp()
    {
        OnLevelUp?.Invoke();
    }

    /*
    public static void InvokeOnGoldReceived(int amountToAdd)
    {
        OnGoldReceived?.Invoke(amountToAdd);
    }

    public static void InvokeOnGoldRemoved(int amountToRemove)
    {
        OnGoldRemoved?.Invoke(amountToRemove);
    }
    */

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

    public static void InvokeOnManaAmountChanged(float updatedMana)
    {
        OnManaAmountChanged?.Invoke(updatedMana);
    }

    public static void InvokeOnMaxManaChanged(float newMaxMana)
    {
        OnMaxManaChanged?.Invoke(newMaxMana);
    }

    internal static void InvokeOnHeroDeath()
    {
        OnHeroDeath?.Invoke();
    }
}
