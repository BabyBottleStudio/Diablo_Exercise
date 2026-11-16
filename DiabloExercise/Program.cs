
using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

var heroCalc = new HeroLevelingCalculator(50);

heroCalc.PrintProgressList();
Console.WriteLine(heroCalc.GetExpirienceThreshold(13));
Console.WriteLine(heroCalc.GetExpirienceThreshold(49));
Console.WriteLine(heroCalc.GetExpirienceThreshold(50));
Console.WriteLine(heroCalc.GetExpirienceThreshold(53));
Console.WriteLine(heroCalc.GetExpirienceThreshold(0));
Console.WriteLine(heroCalc.GetExpirienceThreshold(-3));

var heroData = new HeroDataRepository();


var heroFactory = new HeroFactory(heroData);

//IUserInterface userInterface;

var jovana = heroFactory.Instantiate("Warrior", "Jovana"); //, out userInterface);

Console.WriteLine(jovana);


var helmetModifier = new StatsModifier()
{
    MagicResistance = 30,
    FireResistance = 25,
    LightningResistance = 15

};
var equippedHelmet = new HeadArmor(name: "CurrentHelmet", armor: 1, strengthRequirement: 0, maxDuration: 50, currentDuration: 23);
var helmet = new HeadArmor(name: "Helmet", specialPrefix: "Incredible", specialSuffix: "of the mind", armor: 5, strengthRequirement: 5, maxDuration: 25, currentDuration: 14, helmetModifier);


Console.WriteLine(equippedHelmet);
Console.WriteLine(helmet);

Console.WriteLine();
helmet.Identify();
Console.WriteLine(helmet);


jovana.EquipHeadArmor(equippedHelmet);

Console.WriteLine(jovana);

jovana.RemoveHeadArmor();
Console.WriteLine(jovana);

var negativeHelmetModifier = new StatsModifier()
{
    Strength = -50,
    Magic = -50,
    Dexterity = -50,
    Vitality = -50,

};

var milisav = heroFactory.Instantiate("Warrior", "Milisav");
var negativeHelmet = new HeadArmor("Helmet", specialPrefix: "Horrible", specialSuffix: "en retard", 5, 25, 14, 5, negativeHelmetModifier);
negativeHelmet.Identify();
milisav.EquipHeadArmor(negativeHelmet);

Console.WriteLine(milisav);


public class EquipmentManager
{

    public HeadArmor? HeadArmor { get; private set; }

    //public Amulet Amulet { get; private set; }

    //public BodyArmor BodyPlate { get; private set; }

    //public IHand RightHand { get; private set; }
    //public IHand LeftHand { get; private set; }

    //public Ring LeftRing { get; private set; }
    //public Ring RightRing { get; private set; }

    public EquipmentManager()
    {

        HeadArmor = null;
    }

    public EquipmentManager(HeadArmor headPiece)
    {
        HeadArmor = headPiece;
    }

    public void EquipHeadArmor(HeadArmor headArmor)
    {
        //if (headArmor == null)
        //{
        //    Console.WriteLine("EquipHeadArmor - Invalid operation: HeadArmor is null.");
        //    return;
        //}
        //if (headArmor.IsEquipped)
        //{
        //    Console.WriteLine("EquipHeadArmor - Invalid operation: HeadArmor is already equipped.");
        //    return;
        //}

        // da li je requirement ispunjen: strength
        //      true => equip => ucitaj parametre na statiskike karaktera
        //      false => vrati u inventori ili izbaci napolje
        // ako je ispunjen, kako prenosi dalje svoje statistike na sekundarne elemente
        // Ukoliko je item specijalan => ????????
        // ukoliko je postojao prethodno ekvipovan element, taj biva zamenjen sa novim i vraca se u inventori
        // ukoliko se baca iz inventorija napolje, odlazi u limbo repozitorijum gde se ne unistava vec ceka ponovno sakupljanje
        // ta mehanika bi trebalo da imitira mehaniku izbacivanja elementa napolje i ostavljanje negde u nivou.
        // inventori bi trebalo da ima opciju za Pick()
        // 


        HeadArmor = headArmor;
        HeadArmor.UpdateIsEquippedStatus(true);
    }

    public void RemoveHeadArmor()
    {
        //if (HeadArmor is null)
        //{
        //    Console.WriteLine("EquipHeadArmor - Invalid operation: HeadArmor is null.");
        //    return;
        //}

        //if (!HeadArmor.IsEquipped)
        //{
        //    Console.WriteLine("EquipHeadArmor - Invalid operation: Trying to remove item that is not equipped.");
        //    return;
        //}


        HeadArmor?.UpdateIsEquippedStatus(false);

        HeadArmor = null;
    }
}

public abstract class Item
{
    public virtual string Name { get; set; }

    public Item(string name)
    {
        Name = name;
    }
}

public interface IHand
{

}

public interface IIdentifiable
{
    //public bool IsIdentified { get; }

    //public StatsModifier? StatsModifier { get; }
    public StatsModifier? GetActiveStatsModifier();
    public void Identify();
}
public interface IReparable
{
    public bool ShouldRepare();
    public void Repare();
}
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















//public class Shield : ArmorEquipment, IHand
//{
//    public Shield(string name, int armor, int maxDuration, int currentDuration, int require) : base(name, armor, maxDuration, currentDuration, require)
//    {
//    }
//}

//public class BodyArmor : ArmorEquipment
//{
//    public BodyArmor(string name, int armor, int maxDuration, int currentDuration, int require) : base(name, armor, maxDuration, currentDuration, require)
//    {
//    }
//}


//public class Jewelry : IEquipable, IHand
//{
//    public bool IsIdentified { get; private set; }
//    public string NamePrefix { get; private set; }
//    public string Name { get; private set; }
//    public string NameSufix { get; private set; }

//    public void OnEquip()
//    {
//        throw new NotImplementedException();
//    }

//    public void OnUnequip()
//    {
//        throw new NotImplementedException();
//    }
//}

//public class Ring : Jewelry
//{

//}

//public class Amulet : Jewelry { }

public interface IEquipable
{
    void OnEquip();
    void OnUnequip();
}

public interface IDealDamage
{
    float MinDamage { get; init; }
    float MaxDamage { get; init; }
}

public interface IDurable
{
    bool IsIndistructable { get; }
    float MaxDurability { get; }
    float CurrentDurability { get; }
}



