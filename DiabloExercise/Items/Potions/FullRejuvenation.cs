public class FullRejuvenation : Potion
{
    public override string Name => $"{_namePrefix} Full Rejuvenation";
    public override string Description => "Fully Recover Life and Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
        PotionEvents.InvokeOnFullManaPotionConsumed();

        DisplayConsumeMessage();
    }
}