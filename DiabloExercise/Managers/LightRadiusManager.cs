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
        // -60% || -20% || 20% || 40%

        // amount should be 0.6 || 0.8 || 1.2 || 1.4
        float modPercent = statsModifier.LightRadius;

        _radiusModifier += modPercent;
        Console.WriteLine(this.ToString());
    }

    private void RevertLightRadius(StatsModifier statsModifier)
    {
        float modPercent = statsModifier.LightRadius * -1;

        HandleModifier(modPercent);
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
