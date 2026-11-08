public interface IGoldManager
{
    int GoldCount { get; }

    void Add(int amountToAdd);
    bool Decrease(int amount);
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
