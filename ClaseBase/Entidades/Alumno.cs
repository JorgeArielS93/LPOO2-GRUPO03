using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClaseBase
{
    public class Alumno : IDataErrorInfo
    {
        public int Alu_ID { get; set; }
        public string Alu_DNI { get; set; }
        public string Alu_Apellido { get; set; }
        public string Alu_Nombre { get; set; }
        public string Alu_Email { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get {
                string msg_error = null;

                switch (columnName)
                {
                    case "Alu_ID":
                        msg_error = "El ID es obligatorio";
                        break;
                    case "Alu_DNI":
                        msg_error = validar_atributo(Alu_DNI);
                        break;
                    case "Alu_Apellido":
                        msg_error = validar_atributo(Alu_Apellido);
                        break;
                    case "Alu_Nombre":
                        msg_error = validar_atributo(Alu_Nombre);
                        break;
                    case "Alu_Email":
                        msg_error = validar_atributo(Alu_Email);
                        break;
                }
                return msg_error;
            }
        }
        private string validar_atributo(string atributo)
        {
            if (String.IsNullOrEmpty(atributo))
            {
                return "El valor del campo es obligatorio";
            }
            return null;
        }
    }
}
