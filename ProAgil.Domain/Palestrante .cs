using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Domain
{
    public class Palestrante
    {
        public int PalestranteId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string MiniCurriculo { get; set; }
        [Required]
        public string ImagemURL { get; set; }
        [Phone]
        public string Telefone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<RedeSocial> RedesSociais { get; set; }
        public List<PalestranteEvento> PalestranteEventos { get; set; }
    }
}