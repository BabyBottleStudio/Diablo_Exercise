public class EquipementEvents
{
    public static Action<StatsModifier>? OnItemEquip;
    public static Action<StatsModifier>? OnItemUnequip;


    public static void InvokeOnItemEquip(StatsModifier statsModifier)
    {
        OnItemEquip?.Invoke(statsModifier);
    }

    public static void InvokeOnItemUnequip(StatsModifier statsModifier)
    {
        OnItemUnequip?.Invoke(statsModifier);
    }
}
