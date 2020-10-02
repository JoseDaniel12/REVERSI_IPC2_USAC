//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoIpc2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Partida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partida()
        {
            this.Encuentro = new HashSet<Encuentro>();
        }
    
        public int GameId { get; set; }
        public string GameType { get; set; }
        public string XmlRouteBoard { get; set; }
        public int Player1MovesNumber { get; set; }
        public int Player2MovesNumber { get; set; }
        public int Player1Points { get; set; }
        public int Player2Points { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public Nullable<int> RoundId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Encuentro> Encuentro { get; set; }
        public virtual Ronda Ronda { get; set; }
    }
}
