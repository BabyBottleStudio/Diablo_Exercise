public class HeroFactory
{
    private readonly string[] flags = new string[] { "Warrior", "Rogue", "Sorcerer" };

    private readonly HeroDataRepository heroData;

    public HeroFactory(HeroDataRepository heroData)
    {
        this.heroData = heroData;
    }

    public Hero? Instantiate(string classFlag, string heroName) //, out IUserInterface userInterface)
    {
        if (string.IsNullOrEmpty(classFlag))
        {
            throw new ArgumentNullException($"{GetType().Name} - Instantiate method: flag is null");
        }

        if (!flags.Contains(classFlag))
        {
            throw new ArgumentException("Flag not recognized");
        }

        if (string.IsNullOrEmpty(heroName))
        {
            heroName = $"Some {classFlag}";
        }



        // values for calculating mana and life
        //float vitalityMultiplier = 0; // warrior = 2; rogue = 1; Sorcerer = 1
        //float vitalityItemsMultiplier = 0; // war = 2; rogue = 1.5; sorc = 1
        //float heroLevelMultiplierLife = 0; // W = 2 ; R = 2; S = 1
        //float lifeFixedBonus = 0; //W = 18; R = 23; S = 9
        //
        //float magicMultilplier = 0; //W =1; R = 1; S = 2
        //float magicItemsMultiplier = 0; // W = 1; R = 1.5; S = 2;
        //float heroLevelMultiplierMana = 0; //W = 1; R = 2; S = 2
        //float magicFixedBonus = 0; // W=-1; R = 5; S = -2


        /*
         Keys
        "InitialLife"
        "InitialMaxLife"
        "InitialMana"
        "IntialMaxMana" 
        "InitialGold"
         */

        //switch (flag)
        //{
        //  case "Warrior":
        float initialLightRadius = heroData.GetValue(classFlag, "LightRadius");
        //healthModifier = heroData.GetValue(flag, "LifeModifier");

        //manaModifier = heroData.GetValue(flag, "ManaModifier");

        //initialLife = heroData.GetValue(flag, "InitialLife");
        //initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
        //initialMana = heroData.GetValue(flag, "InitialMana");
        //initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");
        int startStrength = (int)heroData.GetValue(classFlag, "StartBaseStrength");
        int maxStrength = (int)heroData.GetValue(classFlag, "MaxStrength");
        int startMagic = (int)heroData.GetValue(classFlag, "StartBaseMagic");
        int maxMagic = (int)heroData.GetValue(classFlag, "MaxMagic");
        int startDexterity = (int)heroData.GetValue(classFlag, "StartBaseDexterity");
        int maxDexterity = (int)heroData.GetValue(classFlag, "MaxDexterity");
        int startVitality = (int)heroData.GetValue(classFlag, "StartBaseVitality");
        int maxVitality = (int)heroData.GetValue(classFlag, "MaxVitality");



        float vitalityMultiplier = heroData.GetValue(classFlag, "VitalityMultiplier");
        float vitalityItemsMultiplier = heroData.GetValue(classFlag, "VitalityItemsMultiplier");
        float heroLevelMultiplierLife = heroData.GetValue(classFlag, "HeroLevelMultiplierLife");
        float lifeFixedBonus = heroData.GetValue(classFlag, "LifeFixedBonus");

        float magicMultilplier = heroData.GetValue(classFlag, "MagicMultiplier");
        float magicItemsMultiplier = heroData.GetValue(classFlag, "MagicItemsMultiplier");
        float heroLevelMultiplierMana = heroData.GetValue(classFlag, "HeroLevelMultiplierMana");
        float magicFixedBonus = heroData.GetValue(classFlag, "MagicFixedBonus");




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
        //var healthManger = new HealthResource(initialLife, initialMaxLife, attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova
        var healthManger = new Life(attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova
        //var manaManager = new ManaResource(initialMana, initialMaxMana, attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova
        var manaManager = new Mana(attributesCalculator, random); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova

        //userInterface = new UserInterfaceDummy(100, 100, 100, 100, 0); // sve ove vrednosti se kasnije preracunavaju na osnovu osnovnih staova


        ILightRadius lihgtRadiusManger = new LightRadius(initialLightRadius);

        IResistance resistnaceManger = new ResistanceManager();

        ICharacterAttributes characterAttributes = new CharacterAttributes(startStrength, maxStrength, startMagic, maxMagic, startDexterity, maxDexterity, startVitality, maxVitality);
        IDerivedAttributes derivedAttributes = new DerivedAttributes();
        EquipmentManager equipmentManager = new EquipmentManager();


        switch (classFlag)
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
                        attributesCalculator,
                        equipmentManager
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
                        attributesCalculator,
                        equipmentManager
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
                        attributesCalculator,
                        equipmentManager
                    );
            default:
                return null;
        }
    }
}
