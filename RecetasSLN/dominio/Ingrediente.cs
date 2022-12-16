using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    internal class Ingrediente
    {
        public int idIngrediente { get; set; }
        public string nombre { get; set; }
        public string unidad { get; set; }
      
        public Ingrediente(int idIngrediente, string nombre, string unidad)
        {
            this.idIngrediente = idIngrediente;
            this.nombre = nombre;
            this.unidad = unidad;
        }
    }
}
