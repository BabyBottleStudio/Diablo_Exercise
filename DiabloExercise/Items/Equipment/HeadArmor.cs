public class HeadArmor : ArmorEquipment
{
    /// <summary>
    /// Creates an indestructible armor item with no special attributes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    public HeadArmor(string name, int armor, int strengthRequirement) : base(name, armor, strengthRequirement)
    {
    }


    /// <summary>
    /// Creates an indestructible armor item with special attributes modifier. 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="specialPrefix"></param>
    /// <param name="specialSuffix"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="statsModifier"></param>
    public HeadArmor(string name, string specialPrefix, string specialSuffix, int armor, int strengthRequirement, StatsModifier? statsModifier) : base(name, specialPrefix, specialSuffix, armor, strengthRequirement, statsModifier)
    {
    }

    /// <summary>
    /// Creates an destructible armor item with no special attributes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="maxDuration"></param>
    /// <param name="currentDuration"></param>
    public HeadArmor(string name, int armor, int strengthRequirement, int maxDuration, int currentDuration) : base(name, armor, strengthRequirement, maxDuration, currentDuration)
    {
    }


    /// <summary>
    /// Creates an destructible armor item with special attributes modifier.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="specialPrefix"></param>
    /// <param name="specialSuffix"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="maxDuration"></param>
    /// <param name="currentDuration"></param>
    /// <param name="statsModifier"></param>
    public HeadArmor(string name, string specialPrefix, string specialSuffix, int armor, int strengthRequirement, int maxDuration, int currentDuration, StatsModifier? statsModifier) : base(name, specialPrefix, specialSuffix, armor, strengthRequirement, maxDuration, currentDuration, statsModifier)
    {
    }


    public override bool CanEquip(int currentHeroStrength) => StrengthRequirement <= currentHeroStrength;
}



