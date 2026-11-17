public class SmallHealing : Potion
{


    public override string Name => $"{_namePrefix} Healing";
    public override string Description => "Recover Partial Life";

    public override void Consume()
    {
        PotionEvents.InvokeOnHealthPotionConsumed();
        DisplayConsumeMessage();
    }
}