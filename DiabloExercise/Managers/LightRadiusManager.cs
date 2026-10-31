public class LightRadiusManager : ILightRadius, IDisposable
{
    float _baseLightRadius;
    public float BaseLightRadius => _baseLightRadius;

    float _radiusModifier;
    public float ModifiedRadius
    {
        get
        {
            var newRadius = Math.Clamp(_baseLightRadius * (_radiusModifier / 100f), 20f, 160);
            return (float)Math.Round(newRadius, 0);
        }
    }


    public LightRadiusManager(float startRadius)
    {
        if (startRadius < 0)
        {
            throw new ArgumentOutOfRangeException($"Start radius must be positive number.");
        }

        _baseLightRadius = startRadius;
        _radiusModifier = 100f;

        EquipementEvents.OnItemEquip += ModifyLightRadius;
        EquipementEvents.OnItemUnequip += RevertLightRadius;
    }


    private void ModifyLightRadius(StatsModifier statsModifier)
    {
        float modPercent = statsModifier.LightRadius;

        if (modPercent != 0) HandleModifier(modPercent);
    }

    private void RevertLightRadius(StatsModifier statsModifier)
    {
        float modPercent = statsModifier.LightRadius;

        if (modPercent != 0) HandleModifier(-1 * modPercent);
    }
    
    private void HandleModifier(float modificationPercent)
    {
        _radiusModifier += modificationPercent;
        Console.WriteLine(this.ToString());
    }


    public override string ToString()
        => $"Base light radius: {BaseLightRadius}. Modified light radius: {ModifiedRadius}";



    public void Dispose()
    {
        EquipementEvents.OnItemEquip -= ModifyLightRadius;
        EquipementEvents.OnItemUnequip -= RevertLightRadius;
    }
}
