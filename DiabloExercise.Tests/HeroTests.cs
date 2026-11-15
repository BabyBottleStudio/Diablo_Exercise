namespace DiabloExercise.Tests
{
    public class HeroTests
    {

        HeroDataRepository heroData;
        HeroFactory heroFactory;
        Hero? hero;


        [SetUp]
        public void Setup()
        {

            heroData = new HeroDataRepository();
            heroFactory = new HeroFactory(heroData);

        }

        [Test]
        public void Hero_InitializedWithName_ReturnCorrectName()
        {
            hero = heroFactory.Instantiate("Warrior", "Milisav");
            //
            string heroName = hero.Name;

            Assert.That("Milisav", Is.EqualTo(heroName));
        }


        [Test]
        public void Hero_Initialized_ReturnsCorrectClass()
        {
            hero = heroFactory.Instantiate("Warrior", "Milisav");
            //a
            string heroClass = hero.Class;

            Assert.That("Warrior", Is.EqualTo(heroClass));
        }
    }
}
