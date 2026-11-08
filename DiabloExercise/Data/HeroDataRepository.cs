public class HeroDataRepository
{
    // this class is a dummy class. This data should be stored in JSon file or Scriptable Object if in Unity
    // index 0 -> lifeModifier
    // index 1 -> manaModifier

    /*
    float vitalityMultiplier; //        warrior = 2;    rogue = 1;      Sorcerer = 1
    float vitalityItemsMultiplier; //   war = 2;        rogue = 1.5;    sorc = 1
    float heroLevelMultiplierLife; //   W = 2 ;         R = 2;          S = 1
    float lifeFixedBonus; //            W = 18;         R = 23;         S = 9

    float magicMultilplier; //          W = 1;          R = 1;          S = 2
    float magicItemsMultiplier; //      W = 1;          R = 1.5;        S = 2;
    float heroLevelMultiplierMana; //   W = 1;          R = 2;          S = 2
    float magicFixedBonus; //           W = -1;         R = 5;          S = -2
    */
    public readonly Dictionary<string, Dictionary<string, float>> HeroData
        = new Dictionary<string, Dictionary<string, float>>() {

            { "Warrior", // key
                
                // value
                new Dictionary<string, float>()
                {
                    { "StartBaseStrength", 30f},
                    { "MaxStrength", 250f},
                    { "StartBaseMagic", 10f},
                    { "MaxMagic", 50f},
                    { "StartBaseDexterity", 20f},
                    { "MaxDexterity", 60f},
                    { "StartBaseVitality", 25f},
                    { "MaxVitality", 100f},

                    { "LightRadius", 100f},
                    //{ "LifeModifier", 2.0f },
                    //{ "ManaModifier", 1.0f },
                    //{ "InitialLife", 100f},
                    //{ "InitialMaxLife", 100f},
                    //{ "InitialMana", 100f},
                    //{ "InitialMaxMana", 100f},
                    { "InitialGold", 0f},
                    // custom numbers to calculate life
                    { "VitalityMultiplier", 2f},
                    { "VitalityItemsMultiplier", 2f},
                    { "HeroLevelMultiplierLife", 2},
                    { "LifeFixedBonus", 18},
                    // custom numbers to calculate mana
                    { "MagicMultiplier", 1 },
                    { "MagicItemsMultiplier", 1 },
                    { "HeroLevelMultiplierMana", 1},
                    { "MagicFixedBonus", 1}

                } // Warrior value end
             },

            { "Rogue", // key
                       
                new Dictionary<string, float>() // value
                {
                    { "StartBaseStrength", 20f},
                    { "MaxStrength", 55f},
                    { "StartBaseMagic", 15f},
                    { "MaxMagic", 70f},
                    { "StartBaseDexterity", 30f},
                    { "MaxDexterity", 250f},
                    { "StartBaseVitality", 20f},
                    { "MaxVitality", 80f},

                    { "LightRadius", 100f},
                    //{ "LifeModifier", 2.0f },
                    //{ "ManaModifier", 1.0f },
                    //{ "InitialLife", 100f},
                    //{ "InitialMaxLife", 100f},
                    //{ "InitialMana", 100f},
                    //{ "InitialMaxMana", 100f},
                    { "InitialGold", 0f},
                    // custom numbers to calculate life
                    { "VitalityMultiplier", 1f},
                    { "VitalityItemsMultiplier", 1.5f},
                    { "HeroLevelMultiplierLife", 2f},
                    { "LifeFixedBonus", 23f},
                    // custom numbers to calculate mana
                    { "MagicMultiplier", 1f },
                    { "MagicItemsMultiplier", 1.5f },
                    { "HeroLevelMultiplierMana", 2f},
                    { "MagicFixedBonus", 5f}
                } // rogue value end
            },

            { "Sorcerer", // key
                          
                new Dictionary<string, float>() // value
                {
                    { "StartBaseStrength", 15f},
                    { "MaxStrength", 45f},
                    { "StartBaseMagic", 35f},
                    { "MaxMagic", 250f},
                    { "StartBaseDexterity", 15f},
                    { "MaxDexterity", 85f},
                    { "StartBaseVitality", 20f},
                    { "MaxVitality", 80f},

                    { "LightRadius", 100f},
                    //{ "LifeModifier", 1.0f },
                    //{ "ManaModifier", 2.0f },
                    //{ "InitialLife", 100f},
                    //{ "InitialMaxLife", 100f},
                    //{ "InitialMana", 100f},
                    //{ "InitialMaxMana", 100f},
                    { "InitialGold", 0f},

                     // custom numbers to calculate life
                    { "VitalityMultiplier", 1f},
                    { "VitalityItemsMultiplier", 1f},
                    { "HeroLevelMultiplierLife", 1f},
                    { "LifeFixedBonus", 9f},
                    // custom numbers to calculate mana
                    { "MagicMultiplier", 2f },
                    { "MagicItemsMultiplier", 2f },
                    { "HeroLevelMultiplierMana", 2f},
                    { "MagicFixedBonus", -2f}
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


// hero Items - IEquipable (equip, unequip)
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
