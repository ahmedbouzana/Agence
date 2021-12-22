using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agence.Models
{
    public class Registre
    {
        public int Id { get; set; }

        [Required]
        public int NOrdre { get; set; }

        [Required]
        public int Annee { get; set; }

        [NotMapped]
        public string OrdreAnnee { get { return string.Join("/", NOrdre, Annee); } }

        [Required]
        [MaxLength(12)]
        public string NAffaire { get; set; }

        [MaxLength(200)]
        public string NomClient { get; set; }

        [MaxLength(200)]
        public string AdresseClient { get; set; }

        //public int ClientId { get; set; }

        //public Client Client { get; set; }

        public int? NatureId { get; set; }

        public Nature Nature { get; set; }

        public DateTime? DatePaiement { get; set; }

        public DateTime? DateReception { get; set; }

        public int? EntrepriseId { get; set; }

        public Entreprise Entreprise { get; set; }

        public DateTime? DateAffectation { get; set; }

        public DateTime? DateRemise { get; set; }

        public bool Realise { get; set; }

        [MaxLength(250)]
        public string Observation { get; set; }
    }
}
