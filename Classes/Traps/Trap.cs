public abstract class Trap
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public abstract void Activate(Piece piece);

    public virtual void Reset()
    {
        //Reset trap state
    }
}