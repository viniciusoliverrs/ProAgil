using System;
using System.ComponentModel.DataAnnotations;
namespace ProAgil.Domain
{
    public class Lote
    {
        public int LoteId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        [Range(2, 5000)]
        public int Quantidade { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; }
    }
}