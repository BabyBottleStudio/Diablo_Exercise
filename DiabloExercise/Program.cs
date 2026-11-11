
using System;
using System.Diagnostics.Contracts;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;

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

IUserInterface userInterface;

var jovana = heroFactory.Instantiate("Rogue", "Jovana", out userInterface);

Console.WriteLine(jovana);



public class EquipmentManager
{

    public HeadArmor? HeadArmor { get; private set; }

    //public Amulet Amulet { get; private set; }

    //public BodyArmor BodyPlate { get; private set; }

    //public IHand RightHand { get; private set; }
    //public IHand LeftHand { get; private set; }

    //public Ring LeftRing { get; private set; }
    //public Ring RightRing { get; private set; }

    public EquipmentManager() {

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

    public void UnequipHeadArmor()
    {
        HeadArmor = null;
    }
}

public abstract class Item
{
    // this class should enable using the potion as an item in the inventory
    // not sure how is it going to work yet

    public virtual string Name { get; set; }

    public Item(string name )
    {
        Name = name;
    }
}

public interface IHand
{

}


public abstract class ArmorEquipment : Item
{
    //bool _isEquipped;
    
    public int Armor { get; private set; }
    public int MaxDuration { get; private set; }

    public bool _isIndistructible;
    public int CurrentDurration { get; private set; }

    public int Requirement { get; init; }

    public StatsModifier? StatsModifier { get; init; } 

    public ArmorEquipment(string name, int armor, int maxDuration, int currentDurration, int requirement, StatsModifier? statsModifier) :base(name)
    {
       
        Armor = armor;
        MaxDuration = maxDuration;
        CurrentDurration = currentDurration;
        Requirement = requirement;
        StatsModifier = statsModifier;
    }

    public ArmorEquipment(string name, int armor, int maxDuration, int currentDurration, int requirement) : this(name, armor, maxDuration, currentDurration, requirement, null)
    {

        Armor = armor;
        MaxDuration = maxDuration;
        CurrentDurration = currentDurration;
        Requirement = requirement;
        StatsModifier = null;
    }

    public void Damage()
    {
        if (!_isIndistructible) return;
        // code for damage goes here
        // damage absorbed from the battle
    }
    public void Repair()
    {
        if (!_isIndistructible) return;

        CurrentDurration = MaxDuration;
    }
}

public class HeadArmor : ArmorEquipment
{
    public HeadArmor(string name, int armor, int maxDuration, int currentDurration, int require) : base(name, armor, maxDuration, currentDurration, require)
    {
    }
}















//public class Shield : ArmorEquipment, IHand
//{
//    public Shield(string name, int armor, int maxDuration, int currentDurration, int require) : base(name, armor, maxDuration, currentDurration, require)
//    {
//    }
//}

//public class BodyArmor : ArmorEquipment
//{
//    public BodyArmor(string name, int armor, int maxDuration, int currentDurration, int require) : base(name, armor, maxDuration, currentDurration, require)
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

