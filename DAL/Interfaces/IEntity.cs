namespace DAL.Interfaces
{
    /// <summary>
    /// Entity to store in a IRepository
    /// </summary>
    /// <typeparam name="TIdentifier"> Primary key </typeparam>
    public interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}