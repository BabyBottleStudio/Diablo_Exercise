public abstract class Item
{
    public virtual string Name { get; set; }

    public Item(string name)
    {
        Name = name;
    }
}



