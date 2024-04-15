using System;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Domain.Interfaces
{
    public interface IReadOnlyUserRepository
    {
        IUserFilterBuilder FilterBuilder { get; set; }

        IQueryable<User> GetAll();
        User GetSingle(int id);

        User? GetByEmail(string email);
        bool ExistsByEmail(string email);
    }
}
