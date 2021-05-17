using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ProAgil.Domain
{
    public class Evento
    {
        public int EventoId { get; set; }
        [Required]
        [StringLength(100,MinimumLength=3,ErrorMessage="Loca é entre 3 e 100 Caracters")]
        public string Local { get; set; }
        public DateTime DataEvento { get; set; }
        [Required]
        public string Tema { get; set; }
        [Range(2, 120000)]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        [Phone]
        public string Telefone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Lote> Lotes { get; set; }
        public List<RedeSocial> RedesSociais { get; set; }
        public List<PalestranteEvento> PalestranteEventos { get; set; }
    }
}