using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaseBase
{
    public class Inscripcion
    {
        public int Ins_ID { get; set; }
        public DateTime Ins_Fecha { get; set; }
        public string Cur_ID { get; set; }   
        public int Alu_ID { get; set; }      
        public int Est_ID { get; set; }

        public Curso Curso
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal Alumno Alumno
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Estado Estado
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
