using System.Text;

public abstract class Potion : IInventoryItem
{
    // potion should send a message, and hero class should calculate the effect

    // "Potion of Healing"
    // "Potion of Full Mana"
    public abstract string Name { get; }
    protected const string _namePrefix = "Potion of";
    //public abstract string Name { get; }
    public abstract string Description { get; }

    protected const string _instruction = "Right click to use";

    public string Instruction => _instruction;

    public int SizeInInventory => 1;

    public abstract void Consume();


    protected void DisplayConsumeMessage()
    {
        Console.WriteLine($"{GetType().Name} Consumed");
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Name);
        sb.AppendLine(Description);
        sb.AppendLine(Instruction);
        return sb.ToString();
    }
}