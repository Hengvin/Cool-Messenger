using System;
using Microsoft.EntityFrameworkCore;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Infrastructure;
using SPG.Messenger.Repository.Builder;

namespace SPG.Messenger.Repository
{
	public class UserRepository : IReadOnlyUserRepository, IWritableUserRepository
    {
        private readonly MessengerContext _context;

        public IUserFilterBuilder FilterBuilder { get; set; }

        public UserRepository(MessengerContext context)
		{
            _context = context;

            FilterBuilder = new UserFilterBuilder(_context.Users);
		}


        public IUserUpdateBuilder UpdateBuilder(User user)
        {
            return new UserUpdateBuilder(_context, user);
        }


        public int Create(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                return _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw RepositoryCreateException.FromDbError(ex, nameof(User));
            }
            catch (DbUpdateException ex)
            {
                throw RepositoryCreateException.FromDbError(ex, nameof(User));
            }
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id)
                ?? throw RepositoryReadException.FromNotFound(id, nameof(User));

            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw RepositoryDeleteException.FromDbError(ex, nameof(User));
            }
            catch (DbUpdateException ex)
            {
                throw RepositoryDeleteException.FromDbError(ex, nameof(User));
            }
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetSingle(int id)
        {
            return _context.Users.Find(id)
                ?? throw RepositoryReadException.FromNotFound(id, nameof(User));
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}

