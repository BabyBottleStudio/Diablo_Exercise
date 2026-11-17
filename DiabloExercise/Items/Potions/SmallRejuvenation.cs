public class SmallRejuvenation : Potion
{
    public override string Name => $"{_namePrefix} Rejuvenation";
    public override string Description => "Recover Life and Mana";

    public override void Consume()
    {
        PotionEvents.InvokeOnHealthPotionConsumed();
        PotionEvents.InvokeOnManaPotionConsumed();

        DisplayConsumeMessage();
    }
}