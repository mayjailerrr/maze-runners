public interface IAbility
{
    bool Execute(Context context);
    string Description { get; }
}