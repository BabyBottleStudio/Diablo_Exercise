public class SmallMana : Potion
{
    public override string Name => $"{_namePrefix} Mana";
    public override string Description => "Recover Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnManaPotionConsumed();
        DisplayConsumeMessage();
    }
}