public abstract class Item
{
    // this class should enable using the potion as an item in the inventory
    // not sure how is it going to work yet
}

/*
 * Potions of Healing and Mana restore life and mana as stated below. Potions of Rejuvenation are basically treated as one Potion of Mana and one Potion of Healing. For information on how scrolls of healing and the Healing spell work, see chapter 4.1.2.

Potion of Healing: bonus · maxlife/8 to bonus · 3·maxlife/8

Potion of Mana: bonus · maxmana/8 to bonus · 3·maxmana/8

    Any value in the range is equally probable.

    The bonus is summarized in the table below.

    maxlife and maxmana are the modified values. If they are negative, the amount added will also be negative and in fact subtract life or mana.

Type 	    Warrior 	Rogue 	Sorcerer 	Monk    	Bard    	Barbarian
Healing 	2.0 	    1.5 	1.0 	    1.51    	1.51    	2.0
Mana 	    1.0 	    1.5 	2.0 	    1.51    	1.51    	1.0

1 When using a Potion of Rejuvenation the bonus for a Monk and a Bard is 1.0 due to a bug in Hellfire.
 * 
 */

/*
public class Modifiers // this should be injected to hero 
{
    private float _healingModifier;

    public Modifiers(float healingModifier)
    {
        _healingModifier = healingModifier;
    }

    public float HealingModifier { get => _healingModifier; }
}
*/




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
