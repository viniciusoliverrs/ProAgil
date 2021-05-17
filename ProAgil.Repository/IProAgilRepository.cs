using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        // Geral
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // EVENTOS
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
        Task<Evento[]> GetAllEventoAsyncByTema(string Tema, bool includePalestrantes);
        Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes);

        // PALESTRANTE 
        Task<Palestrante[]> GetAllPalestranteAsyncByNome(string Nome, bool includeEventos);
        Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos);
    }
}