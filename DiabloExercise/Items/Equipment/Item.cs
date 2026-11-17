public abstract class Item : IInventoryItem
{
    public virtual string Name { get; init; }

    public int SizeInInventory { get; init; }

    public Item(string name, int sizeInInventory)
    {
        Name = name;
        SizeInInventory = sizeInInventory;
    }
}



