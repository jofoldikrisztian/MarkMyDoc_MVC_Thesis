namespace MarkMyDoctor.Interfaces
{
    public interface IUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IUserRepository UserRepository { get; }
        void Commit();
        void Dispose();
    }
}
