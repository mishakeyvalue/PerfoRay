namespace DAL.Interfaces
{
    public interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}