public class FullHealing : Potion
{

    public override string Name => $"{_namePrefix} Full Healing";
    public override string Description => "Fully Recover Life";


    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
        DisplayConsumeMessage();
    }
}