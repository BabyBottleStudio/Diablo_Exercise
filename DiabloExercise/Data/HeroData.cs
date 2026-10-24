public class HeroData
{
    // this class is a dummy class. This data should be stored in JSon file or Scriptable Object if in Unity
    // index 0 -> lifeModifier
    // index 1 -> manaModifier

    public readonly Dictionary<string, float> WarriorData = new Dictionary<string, float>() {

        { "LifeModifier", 2.0f },
        { "ManaModifier", 1.0f }
    };

    public readonly Dictionary<string, float> RogueData = new Dictionary<string, float>() {

        { "LifeModifier", 1.5f },
        { "ManaModifier", 1.5f }
    };

    public readonly Dictionary<string, float> SorcererData = new Dictionary<string, float>() {

        { "LifeModifier", 1.0f },
        { "ManaModifier", 2.0f }
    };
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
