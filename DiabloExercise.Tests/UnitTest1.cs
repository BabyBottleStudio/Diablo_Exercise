using DiabloExercise;
using System.Xml.Serialization;
namespace DiabloExercise.Tests
{
    public class Tests
    {
        Hero warrior;
        Hero rogue;
        Hero sorcerer;

        [SetUp]
        public void Setup()
        {
            var heroData = new HeroDataRepository();
            var heroFactory = new HeroFactory(heroData);

            warrior = heroFactory.Instantiate("Warrior", "Milisav");
            rogue = heroFactory.Instantiate("Rogue", "Jovana");
            sorcerer = heroFactory.Instantiate("Sorcerer", "Milorad");

        }

        [Test]
        public void Warrior_InitialAttributesValues_Strength()
        {

            // act arrange assert
            var strength = warrior.CharacterAttributes.CurrentStrength;
            var magic = warrior.CharacterAttributes.CurrentMagic;
            Assert.That(30, Is.EqualTo(strength));
            Assert.That(10, Is.EqualTo(magic));
        }

        [Test]
        public void Rogue_InitialAttributesValue_Strength()
        {
            var strength = rogue.CharacterAttributes.CurrentStrength;

            Assert.That(20, Is.EqualTo(strength));
        }

        [Test]
        public void Sorcerer_InitialAttributesValue_Strength()
        {
            var strength = sorcerer.CharacterAttributes.CurrentStrength;

            Assert.That(15, Is.EqualTo(strength));
        }
    }
}
