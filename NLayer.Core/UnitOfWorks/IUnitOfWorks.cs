namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWorks
    {
        Task CommitAsync();
        void Commit();
    }
}
