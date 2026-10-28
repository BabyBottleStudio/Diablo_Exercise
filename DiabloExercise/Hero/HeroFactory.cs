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


        float healthModifier = 1f;
        float manaModifier = 1f;

        float initialLife = 100f;
        float initialMaxLife = 100f;
        float initialMana = 100f;
        float initialMaxMana = 100f;


        var goldManager = new GoldManager();
        var experienceManager = new ExperienceManager();


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
                healthModifier = heroData.GetValue(flag, "LifeModifier");

                manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");
                break;

            case "Rogue":

                healthModifier = heroData.GetValue(flag, "LifeModifier");
                manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");

                break;

            case "Sorcerer":
                healthModifier = heroData.GetValue(flag, "LifeModifier");
                manaModifier = heroData.GetValue(flag, "ManaModifier");

                initialLife = heroData.GetValue(flag, "InitialLife");
                initialMaxLife = heroData.GetValue(flag, "InitialMaxLife");
                initialMana = heroData.GetValue(flag, "InitialMana");
                initialMaxMana = heroData.GetValue(flag, "InitialMaxMana");
                break;
        }

        var healthManger = new HealthResource(initialLife, initialMaxLife, healthModifier, new Random());
        var manaManager = new ManaResource(initialMana, initialMaxMana, manaModifier, new Random());

        userInterface = new UserInterfaceDummy(initialLife, initialMaxLife, initialMana, initialMaxMana, 0);

        switch (flag)
        {
            case "Warrior":

                return new Warrior(heroName, goldManager, experienceManager, healthManger, manaManager);

            case "Rogue":

                return new Rogue(heroName, goldManager, experienceManager, healthManger, manaManager);


            case "Sorcerer":
                return new Sorcerer(heroName, goldManager, experienceManager, healthManger, manaManager);
            default:
                return null;
        }
    }
}
