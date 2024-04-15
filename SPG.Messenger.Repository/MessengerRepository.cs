using Microsoft.EntityFrameworkCore;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Infrastructure;
using SPG.Messenger.Repository.Builder;

namespace SPG.Messenger.Repository
{
    public class MessengerRepository :  IReadOnlyMessengerRepository, IWriteableMessengerRepository
    {
        private readonly MessengerContext _context;

        public IMessengerFilterBuilder FilterBuilder { get; set; }

        public MessengerRepository(MessengerContext context)
        {
            _context = context;

            FilterBuilder = new MessengerFilterBuilder(_context.MessengerEntries);
        }

        public int Create(MessengerEntry entity)
        {
            try
            {
                _context.MessengerEntries.Add(entity);
                return _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw RepositoryCreateException.FromDbError(ex, "Create Messenger fehlgeschlagen");
            }
            catch (DbUpdateException ex)
            {
                throw RepositoryCreateException.FromDbError(ex, "Create Messenger fehlgeschlagen");
            }
        }

        public IQueryable<MessengerEntry> GetAll()
        {
            return _context.MessengerEntries;
        }

        public MessengerEntry GetSingle(int id)
        {
            return _context.MessengerEntries.SingleOrDefault(s => s.Id == id)
                ?? throw RepositoryReadException.FromNotFound(id, nameof(MessengerEntry));
        }
    }
}

