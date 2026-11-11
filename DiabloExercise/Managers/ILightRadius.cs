public interface ILightRadius
{
    float BaseLightRadius { get; }
    float CurrentLightRadius { get; }
    void ModifyLightRadiusItemsBonus(float value);
}
