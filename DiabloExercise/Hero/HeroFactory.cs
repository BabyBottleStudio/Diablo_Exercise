public class HeroFactory
{
    private readonly string[] flags = new string[] { "Warrior", "Rogue", "Sorcerer" };

    private readonly HeroDataRepository heroData;

    public HeroFactory(HeroDataRepository heroData)
    {
        this.heroData = heroData;
    }

    public Hero? Instantiate(string flag, string heroName, out IUserInterface userInterface)
    {
        if (string.IsNullOrEmpty(flag))
        {
            throw new ArgumentNullException($"{GetType().Name} - Instantiate method: flag is null");
        }

        if (!flags.Contains(flag))
        {
            throw new ArgumentException("Flag not recognized");
        }

        if (string.IsNullOrEmpty(heroName))
        {
            heroName = $"Some {flag}";
        }


        //float healthModifier = 1f;
        //float manaModifier = 1f;



        float initialLife = 100f;
        float initialMaxLife = 100f;
        float initialMana = 100f;
        float initialMaxMana = 100f;
        float initialLightRadius = 100f;


        // values for calculating mana and life
        float vitalityMultiplier = 0; // warrior = 2; rogue = 1; Sorcerer = 1
        float vitalityItemsMultiplier = 0; // war = 2; rogue = 1.5; sorc = 1
        float heroLevelMultiplierLife = 0; // W = 2 ; R = 2; S = 1
        float lifeFixedBonus = 0; //W = 18; R = 23; S = 9

        float magicMultilplier = 0; //W =1; R = 1; S = 2
        float magicItemsMultiplier = 0; // W = 1; R = 1.5; S = 2;
        float heroLevelMultiplierMana = 0; //W = 1; R = 2; S = 2
        float magicFixedBonus = 0; // W=-1; R = 5; S = -2


        /*
         Keys
        "InitialLife"
        "InitialMaxLife"
        "InitialMana"
        "IntialMaxMana" 
        "InitialGold"
         */

        switch (flag)
        {
            case "Warrior":
                initialLightRadius = heroData.GetValue(flag, "LightRadius");
                //healthModifier = heroData.GetValue(flag, "LifeModifier");

                //manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");



                vitalityMultiplier = heroData.GetValue(flag, "VitalityMultiplier");
                vitalityItemsMultiplier = heroData.GetValue(flag, "VitalityItemsMultiplier");
                heroLevelMultiplierLife = heroData.GetValue(flag, "HeroLevelMultiplierLife");
                lifeFixedBonus = heroData.GetValue(flag, "LifeFixedBonus");

                magicMultilplier = heroData.GetValue(flag, "MagicMultiplier");
                magicItemsMultiplier = heroData.GetValue(flag, "MagicItemsMultiplier");
                heroLevelMultiplierMana = heroData.GetValue(flag, "HeroLevelMultiplierMana");
                magicFixedBonus = heroData.GetValue(flag, "MagicFixedBonus");
                break;

            case "Rogue":

                initialLightRadius = heroData.GetValue(flag, "LightRadius");
                //healthModifier = heroData.GetValue(flag, "LifeModifier");
                //manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");

                vitalityMultiplier = heroData.GetValue(flag, "VitalityMultiplier");
                vitalityItemsMultiplier = heroData.GetValue(flag, "VitalityItemsMultiplier");
                heroLevelMultiplierLife = heroData.GetValue(flag, "HeroLevelMultiplierLife");
                lifeFixedBonus = heroData.GetValue(flag, "LifeFixedBonus");

                magicMultilplier = heroData.GetValue(flag, "MagicMultiplier");
                magicItemsMultiplier = heroData.GetValue(flag, "MagicItemsMultiplier");
                heroLevelMultiplierMana = heroData.GetValue(flag, "HeroLevelMultiplierMana");
                magicFixedBonus = heroData.GetValue(flag, "MagicFixedBonus");

                break;

            case "Sorcerer":
                initialLightRadius = heroData.GetValue(flag, "LightRadius");
                //healthModifier = heroData.GetValue(flag, "LifeModifier");
                //manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");

                vitalityMultiplier = heroData.GetValue(flag, "VitalityMultiplier");
                vitalityItemsMultiplier = heroData.GetValue(flag, "VitalityItemsMultiplier");
                heroLevelMultiplierLife = heroData.GetValue(flag, "HeroLevelMultiplierLife");
                lifeFixedBonus = heroData.GetValue(flag, "LifeFixedBonus");

                magicMultilplier = heroData.GetValue(flag, "MagicMultiplier");
                magicItemsMultiplier = heroData.GetValue(flag, "MagicItemsMultiplier");
                heroLevelMultiplierMana = heroData.GetValue(flag, "HeroLevelMultiplierMana");
                magicFixedBonus = heroData.GetValue(flag, "MagicFixedBonus");
                break;
        }



        var heroLevel = new HeroLevel();

        var attributesCalculator = new ResourcesCalculator( 
                    vitalityMultiplier,
                    vitalityItemsMultiplier,
                    heroLevelMultiplierLife,
                    lifeFixedBonus,
                    magicMultilplier,
                    magicItemsMultiplier,
                    heroLevelMultiplierMana,
                    magicFixedBonus
            ); // this one is a dummy

        var heroLvlCalc = new HeroLevelingCalculator(heroLevel.MaxLevel);
        var goldManager = new GoldManager();
        var experienceManager = new ExperienceManager(heroLevel, heroLvlCalc);


        var random = new Random();
        var healthManger = new HealthResource(initialLife, initialMaxLife, attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova
        var manaManager = new ManaResource(initialMana, initialMaxMana, attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova

        userInterface = new UserInterfaceDummy(initialLife, initialMaxLife, initialMana, initialMaxMana, 0); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova


        ILightRadius lihgtRadiusManger = new LightRadiusManager(initialLightRadius);

        IResistance resistnaceManger = new ResistanceManager();

        ICharacterAttributes characterAttributes = new CharacterAttributes();
        IDerivedAttributes derivedAttributes = new DerivedAttributes();



        switch (flag)
        {
            case "Warrior":

                return new Warrior
                    (
                        heroName,

                        goldManager,
                        experienceManager,
                        healthManger,
                        manaManager,
                        lihgtRadiusManger,
                        resistnaceManger,
                        characterAttributes,
                        derivedAttributes,
                        attributesCalculator
                    );

            case "Rogue":

                return new Rogue
                    (
                        heroName,

                        goldManager,
                        experienceManager,
                        healthManger,
                        manaManager,
                        lihgtRadiusManger,
                        resistnaceManger,
                        characterAttributes,
                        derivedAttributes,
                        attributesCalculator
                    );


            case "Sorcerer":
                return new Sorcerer
                    (
                        heroName,

                        goldManager,
                        experienceManager,
                        healthManger,
                        manaManager,
                        lihgtRadiusManger,
                        resistnaceManger,
                        characterAttributes,
                        derivedAttributes,
                        attributesCalculator
                    );
            default:
                return null;
        }
    }
}
