using System.Text;

public abstract class ArmorEquipment : Item, IIdentifiable, IReparable
{
    // bool _isEquipped;
    // name system with prefix and suffix 
    // Duration system and _isIndistructible
    public bool IsEquipped { get; private set; }
    public string Prefix { get; init; }
    public string Suffix { get; init; }
    public string DisplayName => (IsIdentified && statsModifier is not null) ? $"{Prefix} {Name} {Suffix}" : Name;

    public bool IsIdentified { get; private set; }
    public int Armor { get; private set; }
    public int MaxDuration { get; private set; }

    public bool IsIndistructible { get; private set; }
    public int CurrentDuration { get; private set; }

    public int StrengthRequirement { get; init; }

    StatsModifier? statsModifier { get; init; }

    /// <summary>
    /// Creates an destructible armor item with special attributes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="maxDuration"></param>
    /// <param name="currentDuration"></param>
    /// <param name="statsModifier"></param>
    public ArmorEquipment(string name, string specialPrefix, string specialSuffix, int armor, int strengthRequirement, int maxDuration, int currentDuration, StatsModifier? statsModifier) : base(name)
    {
        Prefix = specialPrefix;
        Suffix = specialSuffix;
        Armor = armor;
        MaxDuration = maxDuration;
        CurrentDuration = currentDuration;
        StrengthRequirement = strengthRequirement;
        this.statsModifier = statsModifier;
        IsIdentified = this.statsModifier is null;
    }

    /// <summary>
    /// Creates an destructible armor item without special attributes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="maxDuration"></param>
    /// <param name="currentDuration"></param>
    public ArmorEquipment(string name, int armor, int strengthRequirement, int maxDuration, int currentDuration) : this(name, "", "", armor, strengthRequirement, maxDuration, currentDuration, null)
    {

    }

    /// <summary>
    /// Creates an indestructible armor item without special attributes.
    /// </summary>
    /// <param name="name">The name of the armor.</param>
    /// <param name="armor">The armor value provided by the item.</param>
    /// <param name="strengthRequirement">The minimum Strength required to equip the item.</param>
    public ArmorEquipment(string name, int armor, int strengthRequirement) : this(name, "", "", armor, strengthRequirement, int.MaxValue, int.MaxValue, null)
    {
        IsIndistructible = true;
    }

    /// <summary>
    /// Creates an indestructible armor item with special attributes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="statsModifier"></param>
    public ArmorEquipment(string name, string specialPrefix, string specialSuffix, int armor, int strengthRequirement, StatsModifier? statsModifier) : this(name, specialPrefix, specialSuffix, armor, strengthRequirement, int.MaxValue, int.MaxValue, statsModifier)
    {
        IsIndistructible = true;
    }

    public abstract bool CanEquip(int currentHeroStrength);

    public void UpdateIsEquippedStatus(bool isEquipped)
    {
        IsEquipped = isEquipped;
    }



    public StatsModifier? GetActiveStatsModifier() => (IsIdentified && statsModifier is not null) ? statsModifier : null;


    public void Identify()
    {
        if (IsIdentified) return;

        IsIdentified = true;
    }

    public void Damage()
    {
        if (IsIndistructible) return;
        // code for damage goes here
        // damage absorbed from the battle
        throw new NotImplementedException("Damage not implemented yet!");
    }

    public bool ShouldRepare() => !IsIndistructible && CurrentDuration < MaxDuration;


    public void Repare()
    {
        //if (IsIndistructible) return;

        CurrentDuration = MaxDuration;
    }

    public void MakeIndistructible()
    {
        IsIndistructible = true;
        CurrentDuration = int.MaxValue;
        MaxDuration = int.MaxValue;
    }

    // repare
    // damage
    // makeIndistructible
    // ToString
    public void IncreaseDurability()
    {
        throw new NotImplementedException();
    }



    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"** {Name} **"); // still has to work on prefix/suffix system
        sb.AppendLine($"Armor: {Armor}");

        if (!IsIndistructible)
        {
            sb.AppendLine($"Durability: {CurrentDuration}/{MaxDuration}");
        }
        else
        {
            sb.AppendLine("Indistructible");
        }

        if (StrengthRequirement > 0)
        {
            sb.AppendLine($"Requires: {StrengthRequirement}");
        }
        else
        {
            sb.AppendLine($"No special requirements");
        }


        if (!IsIdentified)
        {
            sb.AppendLine("Not identified");
            return sb.ToString();
        }
        if (statsModifier is not null)
        {
            sb.AppendLine(statsModifier.ToString());
        }

        return sb.ToString();
    }
}



