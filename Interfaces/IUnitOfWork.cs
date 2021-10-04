namespace MarkMyDoctor.Interfaces
{
    public interface IUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }
        IReviewRepository ReviewRepository { get; }
        void Commit();
        void Rollback();
    }
}
