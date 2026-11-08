public class GoldManager : IGoldManager
{
    private int _goldCount;

    public int GoldCount => _goldCount;



    public void Add(int amountToAdd)
    {
        if (amountToAdd < 0) return;

        _goldCount += amountToAdd;

        HeroEvents.InvokeOnGoldCountChanged(_goldCount);
        // gold is added when:
        // player picks up the gold item in the dungeon
        // player sells the items in the shop

        // this should be subscribed to an event

    }

    public bool Decrease(int amountToDecrease)
    {
        // gold is removed when:
        // player drops it from the inventory
        // player buys items in the shop
        if (amountToDecrease < 0) return false;

        int difference = _goldCount - amountToDecrease;

        if (difference < 0)
            return false;


        _goldCount = difference;

        HeroEvents.InvokeOnGoldCountChanged(_goldCount);

        return true;
    }


    public override string ToString() => $"Gold: {GoldCount}";

}

/*
    Life

    Warrior: 2·Vit(character) + 2·Vit(items) + 2·clvl + Life(items) + 18

    Rogue: 1·Vit(character) + 1.5·Vit(items) + 2·clvl + Life(items) + 23

    Sorcerer: 1·Vit(character) + 1·Vit(items) + 1·clvl + Life(items) + 9
*/

/*

Mana

Warrior: 1·Mag(character) + 1·Mag(items) + 1·clvl + Mana(items) - 1

Rogue: 1·Mag(character) + 1.5·Mag(items) + 2·clvl + Mana(items) + 5

Sorcerer: 2·Mag(character) + 2·Mag(items) + 2·clvl + Mana(items) - 2

*/
