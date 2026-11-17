public abstract class Item : IInventoryItem
{
    public virtual string Name { get; init; }

    public int InventorySlotSize { get; init; }

    public Item(string name, int inventorySlotSize)
    {
        Name = name;
        InventorySlotSize = inventorySlotSize;
    }
}



