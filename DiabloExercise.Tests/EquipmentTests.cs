using System.IO;

namespace DiabloExercise.Tests
{

    public class EquipmentTests
    {
        HeroDataRepository heroData;
        HeroFactory heroFactory;
        Hero? hero;


        [SetUp]
        public void Setup()
        {

            heroData = new HeroDataRepository();
            heroFactory = new HeroFactory(heroData);

            hero = heroFactory.Instantiate("Warrior", "Milisav");
        }

        // test put on regular helmet and update armor
        // put on special helmet and uptade stats affected by it
        // try to put helmet that has high requirements


        [Test]
        public void Warrior_ShouldReturnCorrectName()
        {
            //a
            string heroName = hero.Name;

            Assert.That("Milisav", Is.EqualTo(heroName));
        }


        [Test]
        public void Warrior_ShouldReturnCorrectClass()
        {
            //a
            string heroClass = hero.Class;

            Assert.That("Warrior", Is.EqualTo(heroClass));
        }

        [Test]
        public void HeadArmor_ShouldIncreaseArmorByPuttingHelmetItem()
        {
            int testArmorValue = 3;
            var testHelmet = new HeadArmor("Helmet", testArmorValue, 45, 14, 5);
            int initialArmor = hero.derivedAttributes.CurrentArmorClass;

            hero.EquipHeadArmor(testHelmet);

            Assert.That(initialArmor + testHelmet.Armor, Is.EqualTo(hero.derivedAttributes.CurrentArmorClass));
        }

        [Test]
        public void HeadArmor_ShouldReturnToOriginalArmorValueByRemovingTheHelmet()
        {
            int testArmorValue = 3;
            var testHelmet = new HeadArmor("Helmet", testArmorValue, 45, 14, 5);
            int initialArmor = hero.derivedAttributes.CurrentArmorClass;

            hero.EquipHeadArmor(testHelmet);

            Assert.That(initialArmor + testHelmet.Armor, Is.EqualTo(hero.derivedAttributes.CurrentArmorClass));

            hero.RemoveHeadArmor();

            Assert.That(initialArmor, Is.EqualTo(hero.derivedAttributes.CurrentArmorClass));
        }

        [Test]
        public void HeadArmor_ShouldNotEquipHelmetDueToHighRequirement()
        {
            //Arrange
            int testRequirementValue = 300;
            var testHelmet = new HeadArmor("Helmet", 5, 45, 14, testRequirementValue);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            hero.EquipHeadArmor(testHelmet);

            //Assert
            Assert.That(hero.EquipmentManager.HeadArmor, Is.Null);

            Assert.That(
                stringWriter.ToString(),
                Does.Contain("Requirements for this item are not met"));

        }

        [Test]
        public void HeadArmor_HelmetNotReplacedDueToHighRequirement()
        {
            //Arrange
            var equippedHelmet = new HeadArmor("CurrentHelmet", armor: 1, maxDuration: 50, currentDurration: 23, requirement: 0);

            int testRequirementValue = 300;
            var testHelmet = new HeadArmor("Helmet", 5, 45, 14, testRequirementValue);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            // Act
            hero.EquipHeadArmor(equippedHelmet);

            hero.EquipHeadArmor(testHelmet);

            //Assert
            Assert.That(hero.EquipmentManager.HeadArmor, Is.EqualTo(equippedHelmet));

            Assert.That(
                stringWriter.ToString(),
                Does.Contain("Requirements for this item are not met"));
        }

        [Test]
        public void HeadArmor_HelmetSucsesfullyReplacedAndArmorValueUpdated()
        {
            //Arrange
            int equippedHelmetArmorClass = 1;
            var equippedHelmet = new HeadArmor("CurrentHelmet", armor: equippedHelmetArmorClass, maxDuration: 50, currentDurration: 23, requirement: 0);

            int testArmor = 5;
            int testRequirementValue = 0;
            var testHelmet = new HeadArmor("Helmet", testArmor, 45, 14, testRequirementValue);
            int initialArmorClass = hero.derivedAttributes.CurrentArmorClass;    // warrior has 4    

            //Act1
            hero.EquipHeadArmor(equippedHelmet);

            // assert1
            Assert.That(hero.EquipmentManager.HeadArmor, Is.EqualTo(equippedHelmet));
            Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(5));

            //Act2
            hero.EquipHeadArmor(testHelmet);

            //Assert2
            Assert.That(hero.EquipmentManager.HeadArmor, Is.EqualTo(testHelmet));
            Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(9));
        }


        [Test]
        public void HeadArmor_EquippingUnidentifiedItemShouldHaveBasicEffect()
        {
            //Arrange
            var helmetModifier = new StatsModifier()
            {
                Strength = 1,
            };

            var testHelmet = new HeadArmor("Helmet", 1, 45, 14, 0, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            hero.EquipHeadArmor(testHelmet);

            //Assert
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(30));

        }

        [Test]
        public void HeadArmor_EquippingUnindentifiedItemAndIdentifyingItWhileStillEquipped()
        {
            //Arrange
            var helmetModifier = new StatsModifier()
            {
                Strength = 5,
            };

            var testHelmet = new HeadArmor("Helmet", 1, 45, 14, 0, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            hero.EquipHeadArmor(testHelmet);

            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(30));
            Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(4 + 1));

            hero.IdentifyItem(hero.EquipmentManager.HeadArmor);

            //Assert
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(35));


            hero.RemoveHeadArmor();
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(30));
            Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(4));

            hero.EquipHeadArmor(testHelmet);
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(35));
            Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(4 + 1));
        }


        [Test]
        public void HeadArmor_NegativeValuesOnSpecialItemsShouldHaveOppositeEffect()
        {

        }

        [Test]
        public void HeadArmor_ExtremeNegativeValuesShouldReturnZeroOnBasicStats()
        {
            var helmetModifier = new StatsModifier()
            {
                Strength = -50,
                Magic = -50,
                Dexterity = -50,
                Vitality = -50,

            };


            var testHelmet = new HeadArmor("Helmet", 1, 45, 14, 0, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            testHelmet.Identify();
            hero.EquipHeadArmor(testHelmet);

            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentMagic, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentDexterity, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentVitality, Is.EqualTo(0));
            //Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(4 + 1));

        }

        // testirati clamped vrednosti na heroju
        // testirati ultra negativne vrednosti da vrate 0 ili 1

    }
}
