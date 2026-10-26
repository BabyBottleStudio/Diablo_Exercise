public class HeroDataRepository
{
    // this class is a dummy class. This data should be stored in JSon file or Scriptable Object if in Unity
    // index 0 -> lifeModifier
    // index 1 -> manaModifier

    public readonly Dictionary<string, Dictionary<string, float>> HeroData
        = new Dictionary<string, Dictionary<string, float>>() {

            { "Warrior", // key
                
                // value
                new Dictionary<string, float>()
                {
                    { "LifeModifier", 2.0f },
                    { "ManaModifier", 1.0f },
                    { "InitialLife", 100f},
                    { "InitialMaxLife", 100f},
                    { "InitialMana", 100f},
                    { "InitialMaxMana", 100f},
                    { "InitialGold", 0f}
                } // Warrior value end
             },

            { "Rogue", // key
                       
                new Dictionary<string, float>() // value
                {
                    { "LifeModifier", 2.0f },
                    { "ManaModifier", 1.0f },
                    { "InitialLife", 100f},
                    { "InitialMaxLife", 100f},
                    { "InitialMana", 100f},
                    { "InitialMaxMana", 100f},
                    { "InitialGold", 0f}
                } // rogue value end
            },

            { "Sorcerer", // key
                          
                new Dictionary<string, float>() // value
                {
                    { "LifeModifier", 1.0f },
                    { "ManaModifier", 2.0f },
                    { "InitialLife", 100f},
                    { "InitialMaxLife", 100f},
                    { "InitialMana", 100f},
                    { "InitialMaxMana", 100f},
                    { "InitialGold", 0f}
                } // sorcerer value end
             }
        };

    public float GetValue(string heroType, string key)
    {
        if (HeroData.ContainsKey(heroType))
        {
            if (HeroData[heroType].ContainsKey(key))
            {
                return HeroData[heroType][key];
            }
            else
            {
                Console.WriteLine($"{heroType} - {key} is not found in the HeroData dictionary.");
                return 0.0f;
            }
        }
        else
        {
            Console.WriteLine($"{heroType} is not found in the HeroData dictionary.");
            return 0.0f;
        }
    }

    //public readonly Dictionary<string, float> WarriorData = new Dictionary<string, float>() {

    //    { "LifeModifier", 2.0f },
    //    { "ManaModifier", 1.0f },
    //    { "InitialLife", 100f},
    //    { "InitialMaxLife", 100f},
    //    { "InitialMana", 100f},
    //    { "InitialMaxMana", 100f},
    //    { "InitialGold", 500f}

    //};

    //public readonly Dictionary<string, float> RogueData = new Dictionary<string, float>() {

    //    { "LifeModifier", 1.5f },
    //    { "ManaModifier", 1.5f },
    //    { "InitialLife", 100f},
    //    { "InitialMaxLife", 100f},
    //    { "InitialMana", 100f},
    //    { "InitialMaxMana", 100f},
    //    { "InitialGold", 500f}
    //};

    //public readonly Dictionary<string, float> SorcererData = new Dictionary<string, float>() {

    //    { "LifeModifier", 1.0f },
    //    { "ManaModifier", 2.0f },
    //    { "InitialLife", 100f},
    //    { "InitialMaxLife", 100f},
    //    { "InitialMana", 100f},
    //    { "InitialMaxMana", 100f},
    //    { "InitialGold", 500f}
    //};

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
