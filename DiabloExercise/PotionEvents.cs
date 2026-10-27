public class PotionEvents
{
    public static event Action? OnHealthPotionConsumed;
    public static event Action? OnFullHealthPotionConsumed;

    public static event Action? OnManaPotionConsumed;
    public static event Action? OnFullManaPotionConsumed;

    public static event Action<float>? OnRejuvenationPotionConsumed;
    public static event Action? OnFullRejuvenationPotionConsumed;

    public static void InvokeOnHealthPotionConsumed()
    {
        OnHealthPotionConsumed?.Invoke();
    }
    public static void InvokeOnFullHealthPotionConsumed()
    {
        OnFullHealthPotionConsumed?.Invoke();
    }

    public static void InvokeOnManaPotionConsumed()
    {
        OnManaPotionConsumed?.Invoke();
    }
    public static void InvokeOnFullManaPotionConsumed()
    {
        OnFullManaPotionConsumed?.Invoke();
    }

    public static void InvokeOnRejuvenationPotionConsumed(float amountToRestore)
    {
        OnRejuvenationPotionConsumed?.Invoke(amountToRestore);
    }
    public static void InvokeOnFullRejuvenationPotionConsumed()
    {
        OnFullRejuvenationPotionConsumed?.Invoke();
    }

}
