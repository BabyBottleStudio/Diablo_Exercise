// See https://aka.ms/new-console-template for more information
using System.Text;

public abstract class Potion : Item
{
    // potion should send a message, and hero class should calculate the effect

    // "Potion of Healing"
    // "Potion of Full Mana"
    protected const string _namePrefix = "Potion of";
    public abstract string Name { get; }
    public abstract string Description { get; }

    protected const string _instruction = "Right click to use";
    public string Instruction => _instruction;
    public abstract void Consume();


    protected void DisplayConsumeMessage()
    {
        Console.WriteLine($"{GetType().Name} Consumed");
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Name);
        sb.AppendLine(Description);
        sb.AppendLine(Instruction);
        return sb.ToString();
    }
}



// Items - base abstract class for all items that can go into the inventory - or interface?

// consumables - IConsumable
// healling potion -> heals 50hp
// full healing potion -> restores full health
// mana potion -> restores 50 mana
// full mana potion -> restores full mana
// potion of rejuvination -> heals 50hp and 50 mana
// potion of full rejuvination - restores full health and mana

// value
// metoda -> consume


// hero equipment - IEquipable (equip, unequip)
// - Weapons - deals damage to the enemies
//      - OneHand weapon
//          - Sword
//      - TwoHanded weapon
//          - Axe
//          - Staff
//          - Bow
// - Shields - protect from damage + special stuff
// - Head pieces - protect from damage + special stuff
// - Body wearables - protect from damage + special stuff
// - Amulet - special attributes
// - Ring - special attributes
//
// properties:
// name
// armor
// minimum damage
// maximum damage
// max duration
// current duration
// special affect
// requirements
// reduce current duration (get damage)
//
//
