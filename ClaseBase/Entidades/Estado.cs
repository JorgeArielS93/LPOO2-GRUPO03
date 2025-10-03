using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaseBase
{
    public class Estado
    {
        public int Est_ID { get; set; }
        public string Est_Nombre { get; set; } //Curso: programado, en_curso, finalizado, cancelado. Inscripcion: inscripto, confirmado, cancelado
        public int Esty_ID { get; set; }

        public EstadoType EstadoType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        } 
    }
}
