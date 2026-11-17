
public class FullMana : Potion
{
    public override string Name => $"{_namePrefix} Full Mana";
    public override string Description => "Fully Recover Mana";


    public override void Consume()
    {
        PotionEvents.InvokeOnFullHealthPotionConsumed();
        DisplayConsumeMessage();
    }
}
