using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        // GERAIS
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        // EVENTO
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedesSociais);
            if (includePalestrantes)
                query = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);

            query = query.OrderBy(c => c.EventoId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedesSociais);
            if (includePalestrantes)
                query = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);

            query = query.OrderBy(c => c.EventoId).Where(c => c.EventoId == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string Tema, bool includePalestrantes=false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedesSociais);
            if (includePalestrantes)
                query = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);

            query = query.OrderByDescending(c => c.DataEvento).Where(c => c.Tema.ToLower().Contains(Tema.ToLower()));

            return await query.ToArrayAsync();
        }

        // PALESTRANTE
        public async Task<Palestrante[]> GetAllPalestranteAsyncByNome(string Nome, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(c => c.RedesSociais);
            if (includeEventos)
                query = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Evento);

            query = query.OrderBy(c => c.Nome).Where(c => c.Nome.ToLower().Contains(Nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(c => c.RedesSociais);
            if (includeEventos)
                query = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Evento);

            query = query.OrderBy(c => c.Nome).Where(c => c.PalestranteId == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}