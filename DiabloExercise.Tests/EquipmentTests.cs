using System.IO;

namespace DiabloExercise.Tests
{
    public class EquipmentTests
    {
        HeroDataRepository heroData;
        HeroFactory heroFactory;
        Hero? hero;

        private TextWriter originalConsoleOut;
        private StringWriter stringWriter;

        private const string prefix = "Amazing";
        private const string suffix = "of the bear";

        [SetUp]
        public void Setup()
        {
            originalConsoleOut = Console.Out;
            stringWriter = new StringWriter();

            Console.SetOut(stringWriter);

            heroData = new HeroDataRepository();
            heroFactory = new HeroFactory(heroData);

            hero = heroFactory.Instantiate("Warrior", "Milisav");
        }


        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalConsoleOut);
            stringWriter.Dispose();
        }

        [Test]
        public void HeadArmor_Constructor_BasicItem_ReturnsName()
        {
            // Arrange 
            string testName = "TestHelmet";
            HeadArmor testHeadArmor = new HeadArmor(name: testName, armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            // Act
            string name = testHeadArmor.DisplayName;

            // Assert
            Assert.That(name, Is.EqualTo(testName));
        }

        [Test]
        public void HeadArmor_Constructor_SpecialItem_NotIdentified_ReturnsBasicName()
        {
            // Arrange 
            string testName = "TestHelmet";
            StatsModifier statsModifier = new StatsModifier()
            {
                ArmorClass = 5,
            };

            HeadArmor testHeadArmor = new HeadArmor(name: testName, specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25, statsModifier);

            // Act
            string name = testHeadArmor.DisplayName;

            // Assert
            Assert.That(name, Is.EqualTo(testName));
        }

        [Test]
        public void HeadArmor_Constructor_SpecialItem_Identified_ReturnsSpecialName()
        {
            // Arrange 
            string testName = "TestHelmet";
            StatsModifier statsModifier = new StatsModifier()
            {
                ArmorClass = 5,
            };

            HeadArmor testHeadArmor = new HeadArmor(name: testName, specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25, statsModifier);

            // Act
            testHeadArmor.Identify();
            string name = testHeadArmor.DisplayName;

            // Assert
            Assert.That(name, Is.EqualTo($"{prefix} {testName} {suffix}"));
        }

        [Test]
        
        public void HeadArmor_Constructor_BasicDestructibleItem()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            Assert.That(testHeadArmor.IsIndistructible, Is.False);
            Assert.That(testHeadArmor.IsIdentified, Is.True); // all basic items are identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.Null);
        }

        [Test]
        public void HeadArmor_Constructor_BasicInDestructibleItem()
        {
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0);

            Assert.That(testHeadArmor.IsIndistructible, Is.True);
            Assert.That(testHeadArmor.IsIdentified, Is.True); // all basic items are identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.Null);
        }

        [Test]
        public void HeadArmor_Constructor_SpecialDestructibleItem()
        {
            StatsModifier statsModifier = new StatsModifier() // what if you pass this as null?
            {
                ArmorClass = 5,
            };

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25, statsModifier: statsModifier);

            Assert.That(testHeadArmor.IsIndistructible, Is.False);
            Assert.That(testHeadArmor.IsIdentified, Is.False); // all special items are not identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.Null);

            testHeadArmor.Identify();

            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.EqualTo(statsModifier));
        }

        [Test]
        public void HeadArmor_Constructor_SpecialInDestructibleItem_NotIdentified_GetActiveStatsModifierReturnsNull()
        {
            // Arrange
            StatsModifier statsModifier = new StatsModifier()
            {
                ArmorClass = 5,
            };

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, statsModifier: statsModifier);

            //Act

            //Assert I
            Assert.That(testHeadArmor.IsIdentified, Is.False); // all special items are not identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.Null); // not identified special items return null while unidentified
        }


        [Test]
        public void HeadArmor_Constructor_SpecialInDestructibleItem_Identified_GetActiveStatsModifierReturnsObject()
        {
            // Arrange
            StatsModifier statsModifier = new StatsModifier() // what if you pass this as null?
            {
                ArmorClass = 5,
            };

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, statsModifier: statsModifier);

            //Act
            testHeadArmor.Identify();

            //Assert II
            Assert.That(testHeadArmor.IsIdentified, Is.True); // all special items are not identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.EqualTo(statsModifier)); // not identified special items return null while unidentified
        }


        [Test]
        public void HeadArmor_Constructor_StatsModifierAsNull_ReturnsIdentifiedTrue_GetActiveStatsModiferReturnsNull()
        {
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25, statsModifier: null);

            Assert.That(testHeadArmor.IsIndistructible, Is.False); // it has parameters for duration
            Assert.That(testHeadArmor.IsIdentified, Is.True); // all basic items are identified by default
            Assert.That(testHeadArmor.GetActiveStatsModifier(), Is.Null);
        }


        [Test]
        public void HeadArmor_Repare_IncreaseCurrentDurabilityToMaxDurability()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            // Act
            testHeadArmor.Repare();

            // Assert
            Assert.That(testHeadArmor.CurrentDuration, Is.EqualTo(testHeadArmor.MaxDuration));
        }

        [Test]
        public void HeadArmor_Equip_ItemAlreadyEquipped()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            // Act
            hero.EquipHeadArmor(testHeadArmor);

            // Assert
            Assert.That(testHeadArmor.IsEquipped, Is.True);

            
            // Act
            hero.EquipHeadArmor(testHeadArmor);

            Console.SetOut(stringWriter);

           //Assert
            
            Assert.That(
                stringWriter.ToString(),
                Does.Contain("Invalid operation. Item is already equipped!"));

        }

        
        [Test]
        public void HeadArmor_MakeIndistructible_ShouldReturnIsIndistructibleTrue()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            // Act
            testHeadArmor.MakeIndistructible();

            // Assert
            Assert.That(testHeadArmor.IsIndistructible, Is.True);
            Assert.That(testHeadArmor.MaxDuration, Is.EqualTo(int.MaxValue));
            Assert.That(testHeadArmor.CurrentDuration, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void HeadArmor_ShouldRepare_IndistructibleItem_ReturnsFalse()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0);

            // Act
            bool shouldRepare = testHeadArmor.ShouldRepare();

            // Assert
            Assert.That(shouldRepare, Is.False);
        }

        [Test]
        public void HeadArmor_ShouldRepare_NotDamagedItem_ReturnsFalse()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 50);

            // Act
            bool shouldRepare = testHeadArmor.ShouldRepare();

            // Assert
            Assert.That(shouldRepare, Is.False);
        }


        [Test]
        public void HeadArmor_ShouldRepare()
        {
            // Arrange
            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: 5, strengthRequirement: 0, maxDuration: 50, currentDuration: 25);

            // Act
            bool shouldRepare = testHeadArmor.ShouldRepare();

            // Assert
            Assert.That(shouldRepare, Is.True);
        }

        [Test]
        public void HeadArmor_ToString_BasicDestructibleItem()
        {

            // Arrange
            int testArmor = 5;
            int testRequirement = 5;
            int testMaxDur = 50;
            int testCurrentDur = 25;

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: testArmor, strengthRequirement: testRequirement, maxDuration: testMaxDur, currentDuration: testCurrentDur);

            // Act
            string testResult = testHeadArmor.ToString();

            //
            Assert.That(testResult, Does.Contain("TestHelmet"));
            Assert.That(testResult, Does.Contain($"Armor: {testArmor}"));
            Assert.That(testResult, Does.Contain($"Durability: {testCurrentDur}/{testMaxDur}"));
            Assert.That(testResult, Does.Contain($"Requires: {testRequirement}"));
        }

        [Test]
        public void HeadArmor_ToString_BasicInDestructibleItem()
        {

            // Arrange
            int testArmor = 5;
            int testRequirement = 5;
            //int testMaxDur = 50;
            //int testCurrentDur = 25;

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: testArmor, strengthRequirement: testRequirement);

            // Act
            string testResult = testHeadArmor.ToString();

            //
            Assert.That(testResult, Does.Contain("TestHelmet"));
            Assert.That(testResult, Does.Contain($"Armor: {testArmor}"));
            Assert.That(testResult, Does.Contain($"Indistructible"));
            Assert.That(testResult, Does.Contain($"Requires: {testRequirement}"));
        }

        [Test]
        public void HeadArmor_ToString_NoSpecialRequirements()
        {

            // Arrange
            int testArmor = 5;
            int testRequirement = 0;
            int testMaxDur = 50;
            int testCurrentDur = 25;

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", armor: testArmor, strengthRequirement: testRequirement, maxDuration: testMaxDur, currentDuration: testCurrentDur);

            // Act
            string testResult = testHeadArmor.ToString();

            //
            Assert.That(testResult, Does.Contain("TestHelmet"));
            Assert.That(testResult, Does.Contain($"Armor: {testArmor}"));
            Assert.That(testResult, Does.Contain($"Durability: {testCurrentDur}/{testMaxDur}"));
            Assert.That(testResult, Does.Contain("No special requirements"));
        }

        [Test]
        public void HeadArmor_ToString_SpecialItemUnidentified()
        {
            // Arrange
            int strengthModifier = 5;

            StatsModifier statsModifier = new StatsModifier()
            {
                Strength = strengthModifier
            };


            int testArmor = 5;
            int testRequirement = 5;
            int testMaxDur = 50;
            int testCurrentDur = 25;

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: testArmor, strengthRequirement: testRequirement, maxDuration: testMaxDur, currentDuration: testCurrentDur, statsModifier);

            // Act
            string testResult = testHeadArmor.ToString();

            //
            Assert.That(testResult, Does.Contain("TestHelmet"));
            Assert.That(testResult, Does.Contain($"Armor: {testArmor}"));
            Assert.That(testResult, Does.Contain($"Durability: {testCurrentDur}/{testMaxDur}"));
            Assert.That(testResult, Does.Contain($"Requires: {testRequirement}"));
            Assert.That(testResult, Does.Contain("Not identified"));
        }


        [Test]
        public void HeadArmor_ToString_SpecialItemIdentified()
        {
            // Arrange
            int strengthModifier = 5;

            StatsModifier statsModifier = new StatsModifier()
            {
                Strength = strengthModifier
            };


            int testArmor = 5;
            int testRequirement = 5;
            int testMaxDur = 50;
            int testCurrentDur = 25;

            HeadArmor testHeadArmor = new HeadArmor(name: "TestHelmet", specialPrefix: prefix, specialSuffix: suffix, armor: testArmor, strengthRequirement: testRequirement, maxDuration: testMaxDur, currentDuration: testCurrentDur, statsModifier);

            // Act
            testHeadArmor.Identify();

            string testResult = testHeadArmor.ToString();

            //
            Assert.That(testResult, Does.Contain("TestHelmet"));
            Assert.That(testResult, Does.Contain($"Armor: {testArmor}"));
            Assert.That(testResult, Does.Contain($"Durability: {testCurrentDur}/{testMaxDur}"));
            Assert.That(testResult, Does.Contain($"Requires: {testRequirement}"));
            Assert.That(testResult, Does.Contain(statsModifier.ToString()));
        }



        [Test]
        public void HeadArmor_Equip_IncreaseHerosArmorClassValue()
        {
            int testArmorValue = 3;
            var testHelmet = new HeadArmor(name: "Helmet", armor: testArmorValue, strengthRequirement: 5, maxDuration: 45, currentDuration: 14);
            int initialArmor = hero.DerivedAttributes.CurrentArmorClass;

            hero.EquipHeadArmor(testHelmet);

            Assert.That(initialArmor + testHelmet.Armor, Is.EqualTo(hero.DerivedAttributes.CurrentArmorClass));
        }

        [Test]
        public void HeadArmor_Remove_ReturnToHerosBasicArmorClassValue()
        {
            int testArmorValue = 3;
            var testHelmet = new HeadArmor(name: "Helmet", armor: testArmorValue, strengthRequirement: 5, maxDuration: 45, currentDuration: 14);
            int initialArmor = hero.DerivedAttributes.CurrentArmorClass;

            hero.EquipHeadArmor(testHelmet);

            Assert.That(initialArmor + testHelmet.Armor, Is.EqualTo(hero.DerivedAttributes.CurrentArmorClass));

            hero.RemoveHeadArmor();

            Assert.That(initialArmor, Is.EqualTo(hero.DerivedAttributes.CurrentArmorClass));
        }

        [Test]
        public void HeadArmor_Equip_RequirementsNotMet()
        {
            //Arrange
            int testRequirementValue = 300;
            var testHelmet = new HeadArmor(name: "Helmet", armor: 5, strengthRequirement: testRequirementValue, maxDuration: 45, currentDuration: 14);

            //var stringWriter = new StringWriter();
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
        public void HeadArmor_Replace_ArmorClassValueUpdated()
        {
            //Arrange
            int equippedHelmetArmorClass = 1;
            var equippedHelmet = new HeadArmor(name: "CurrentHelmet", armor: equippedHelmetArmorClass, strengthRequirement: 0, maxDuration: 50, currentDuration: 23);

            int testArmor = 5;
            int testRequirementValue = 0;
            var testHelmet = new HeadArmor(name: "Helmet", armor: testArmor, strengthRequirement: testRequirementValue, maxDuration: 45, currentDuration: 14);
            int initialArmorClass = hero.DerivedAttributes.CurrentArmorClass;    // warrior has 4    

            //Act1
            hero.EquipHeadArmor(equippedHelmet);

            // assert1
            Assert.That(hero.EquipmentManager.HeadArmor, Is.EqualTo(equippedHelmet));
            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(5));

            //Act2
            hero.EquipHeadArmor(testHelmet);

            //Assert2
            Assert.That(hero.EquipmentManager.HeadArmor, Is.EqualTo(testHelmet));
            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(9));
        }

        [Test]
        public void HeadArmor_Replace_NotReplaced_RequirementsNotMet()
        {
            //Arrange
            var equippedHelmet = new HeadArmor(name: "CurrentHelmet", armor: 1, strengthRequirement: 0, maxDuration: 50, currentDuration: 23);

            int testRequirementValue = 300;
            var testHelmet = new HeadArmor(name: "Helmet", armor: 5, strengthRequirement: testRequirementValue, maxDuration: 45, currentDuration: 14);

            //var stringWriter = new StringWriter();
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
        public void HeadArmor_UnidentifiedItem_ApplyOnlyBasicEffect()
        {
            //Arrange
            var helmetModifier = new StatsModifier()
            {
                Strength = 1,
            };

            var testHelmet = new HeadArmor(name: "Helmet", specialPrefix: prefix, specialSuffix: suffix, armor: 1, strengthRequirement: 0, maxDuration: 45, currentDuration: 14, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            hero.EquipHeadArmor(testHelmet);

            //Assert
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(30));
        }

        [Test]
        public void HeadArmor_UnidentifiedItem_IdentifyWhileEquipped_AppliesSpecialModifiers()
        {
            // Arrange
            int strengthModiferValue = 5;
            var helmetModifier = new StatsModifier()
            {
                Strength = strengthModiferValue,
            };
            int testArmorValue = 1;
            var testHelmet = new HeadArmor(name: "Helmet", specialPrefix: prefix, specialSuffix: suffix, armor: testArmorValue, strengthRequirement: 0, maxDuration: 45, currentDuration: 14, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;
            var initialHeroArmor = hero.DerivedAttributes.CurrentArmorClass;

            // Act
            hero.EquipHeadArmor(testHelmet);

            // Assert
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(initialStrength));
            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(initialHeroArmor + testArmorValue));

            // Act
            hero.IdentifyItem(hero.EquipmentManager.HeadArmor);

            // Assert
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(initialStrength + strengthModiferValue));


            //hero.RemoveHeadArmor();
            //Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(30));
            //Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(4));

            //hero.EquipHeadArmor(testHelmet);
            //Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(35));
            //Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(4 + 1));
        }

        [Test]
        public void HeadArmor_SpecialItem_PuttingItemOnAndOff_ShouldUpdateCharacterAttributes()
        {
            // Arrange
            int strengthModiferValue = 5;
            var helmetModifier = new StatsModifier()
            {
                Strength = strengthModiferValue,
            };
            int testArmorValue = 1;
            var testHelmet = new HeadArmor(name: "Helmet", specialPrefix: prefix, specialSuffix: suffix, armor: testArmorValue, strengthRequirement: 0, maxDuration: 45, currentDuration: 14, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;
            var initialHeroArmor = hero.DerivedAttributes.CurrentArmorClass;

            // Act
            testHelmet.Identify();
            hero.EquipHeadArmor(testHelmet);

            // Assert
            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(initialHeroArmor + testArmorValue));
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(initialStrength + strengthModiferValue));

            // Act
            hero.RemoveHeadArmor();

            // Assert
            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(initialHeroArmor));
            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(initialStrength));

        }

        [Test]
        public void HeadArmor_SpecialItem_NegativeModifiers_DecreaseValues()
        {
            var helmetModifier = new StatsModifier()
            {
                Strength = -5,
                Magic = -5,
                Dexterity = -5,
                Vitality = -5,
            };

            var testHelmet = new HeadArmor(name: "Helmet", specialPrefix: prefix, specialSuffix: suffix, armor: 1, strengthRequirement: 0, maxDuration: 45, currentDuration: 14, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            testHelmet.Identify();
            hero.EquipHeadArmor(testHelmet);

            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(25));
            Assert.That(hero.CharacterAttributes.CurrentMagic, Is.EqualTo(5));
            Assert.That(hero.CharacterAttributes.CurrentDexterity, Is.EqualTo(15));
            Assert.That(hero.CharacterAttributes.CurrentVitality, Is.EqualTo(20));

        }

        [Test]
        public void HeadArmor_SpecialItem_ExtremeNegativeValues_ClampsAttributesOnZero()
        {
            var helmetModifier = new StatsModifier()
            {
                Strength = -50,
                Magic = -50,
                Dexterity = -50,
                Vitality = -50,

            };


            var testHelmet = new HeadArmor(name: "Helmet", specialPrefix: prefix, specialSuffix: suffix, armor: 1, strengthRequirement: 0, maxDuration: 45, currentDuration: 14, helmetModifier);

            var initialStrength = hero.CharacterAttributes.CurrentStrength;

            //Act
            testHelmet.Identify();
            hero.EquipHeadArmor(testHelmet);

            Assert.That(hero.CharacterAttributes.CurrentStrength, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentMagic, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentDexterity, Is.EqualTo(0));
            Assert.That(hero.CharacterAttributes.CurrentVitality, Is.EqualTo(0));

            Assert.That(hero.DerivedAttributes.CurrentArmorClass, Is.EqualTo(1));

            //50 + (currentDexterity / 2) + chanceToHitItemsBonus;
            Assert.That(hero.DerivedAttributes.ChanceToHit, Is.EqualTo(50));
            //Assert.That(hero.derivedAttributes.CurrentArmorClass, Is.EqualTo(4 + 1));

        }
    }
}
