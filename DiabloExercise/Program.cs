
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
        HeadArmor = headArmor;
        HeadArmor.UpdateIsEquippedStatus(true);
    }

    public void RemoveHeadArmor()
    {
        HeadArmor?.UpdateIsEquippedStatus(false);

        HeadArmor = null;
    }
}

public interface IHand
{

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



