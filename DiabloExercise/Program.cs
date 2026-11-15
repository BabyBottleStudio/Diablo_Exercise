
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
var helmet = new HeadArmor(name: "Helmet", armor: 5, strengthRequirement: 5, maxDuration: 25, currentDuration: 14, helmetModifier);


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
var negativeHelmet = new HeadArmor("Helmet", 5, 25, 14, 5, negativeHelmetModifier);
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
        if (headArmor == null) return;

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
        //    HeadArmor.OnEquip();
    }

    public void RemoveHeadArmor()
    {
        HeadArmor = null;
    }
}

public abstract class Item
{
    // this class should enable using the potion as an item in the inventory
    // not sure how is it going to work yet

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
    public ArmorEquipment(string name, int armor, int strengthRequirement, int maxDuration, int currentDuration, StatsModifier? statsModifier) : base(name)
    {
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
    public ArmorEquipment(string name, int armor, int strengthRequirement, int maxDuration, int currentDuration) : this(name, armor, strengthRequirement, maxDuration, currentDuration, null)
    {

    }

    /// <summary>
    /// Creates an indestructible armor item without special attributes.
    /// </summary>
    /// <param name="name">The name of the armor.</param>
    /// <param name="armor">The armor value provided by the item.</param>
    /// <param name="strengthRequirement">The minimum Strength required to equip the item.</param>
    public ArmorEquipment(string name, int armor, int strengthRequirement) : this(name, armor, strengthRequirement, int.MaxValue, int.MaxValue, null)
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
    public ArmorEquipment(string name, int armor, int strengthRequirement, StatsModifier? statsModifier) : this(name, armor, strengthRequirement, int.MaxValue, int.MaxValue, statsModifier)
    {
        IsIndistructible = true;
    }

    public bool CanEquip(int currentHeroStrength) => StrengthRequirement <= currentHeroStrength;

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
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="statsModifier"></param>
    public HeadArmor(string name, int armor, int strengthRequirement, StatsModifier? statsModifier) : base(name, armor, strengthRequirement, statsModifier)
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
    /// <param name="armor"></param>
    /// <param name="strengthRequirement"></param>
    /// <param name="maxDuration"></param>
    /// <param name="currentDuration"></param>
    /// <param name="statsModifier"></param>
    public HeadArmor(string name, int armor, int strengthRequirement, int maxDuration, int currentDuration, StatsModifier? statsModifier) : base(name, armor, strengthRequirement, maxDuration, currentDuration, statsModifier)
    {
    }
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



public class TestWeapon : Item, IEquipable, IDealDamage, IDurable
{
    bool isEquipped = false;
    public float MinDamage { get; init; }
    public float MaxDamage { get; init; }

    private bool _isIndistructable;
    public bool IsIndistructable => _isIndistructable;
    public float MaxDurability { get; }
    public float CurrentDurability { get; }


    private StatsModifier _statsModifier;
    public StatsModifier StatsModifier => _statsModifier;

    public TestWeapon(string name, float minDamage, float maxDamage, StatsModifier statsModifier) : base(name)
    {
        if (minDamage < 0 || maxDamage < 0)
        {
            throw new ArgumentOutOfRangeException("Arguemts must be positive numbers.");
        }

        if (statsModifier is null)
        {
            throw new ArgumentNullException("Stats modifier can't be null!");
        }


        if (minDamage < maxDamage)
        {
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
        }
        else
        {
            this.MinDamage = maxDamage;
            this.MaxDamage = minDamage;
        }

        _statsModifier = statsModifier;
    }

    public void OnEquip()
    {
        if (isEquipped)
        {
            Console.WriteLine("Weapon already equipped.");
            return;
        }

        EquipementEvents.InvokeOnItemEquip(StatsModifier);
        isEquipped = true;
    }

    public void OnUnequip()
    {
        if (!isEquipped)
        {
            Console.WriteLine("Unable to unequip: weapon is not equipped.");
            return;
        }

        EquipementEvents.InvokeOnItemUnequip(StatsModifier);
        isEquipped = false;
    }
}

