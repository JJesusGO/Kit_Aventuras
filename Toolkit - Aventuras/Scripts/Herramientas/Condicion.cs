using UnityEngine;
using System.Collections;

namespace Aventuras{

    public enum CondicionTipo{
        METADATO, 
        VIDA, 
        ATAQUE
    }
    public enum CondicionEstado{
        MENOR,
        MAYOR,
        MENORIGUAL,
        MAYORIGUAL,
        IGUAL,
        DIFERENTE,
        SIEMPRE
    }

    [System.Serializable]
    public class Condicion{

        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private CondicionTipo tipo = CondicionTipo.METADATO;
        [SerializeField]
        private CondicionEstado estado = CondicionEstado.IGUAL;
        [SerializeField]
        private string valor = "0";
        [Header("Metadato")]
        [SerializeField]
        private string nombre = "Desconocido";

        private bool IsCondicionValor(string valor){
            switch(estado){
                case CondicionEstado.DIFERENTE:
                    return this.valor != valor;
                case CondicionEstado.IGUAL:
                    return this.valor == valor;
                case CondicionEstado.MENOR:
                    return float.Parse(valor) < float.Parse(this.valor);
                case CondicionEstado.MENORIGUAL:
                    return float.Parse(valor) <= float.Parse(this.valor);
                case CondicionEstado.MAYOR:
                    return float.Parse(valor) > float.Parse(this.valor);
                case CondicionEstado.MAYORIGUAL:
                    return float.Parse(valor) >= float.Parse(this.valor);                    
                case CondicionEstado.SIEMPRE:
                    return true; 
            }
            return true;
        }
        private bool IsCondicionValor(float valor){
            switch(estado){
                case CondicionEstado.DIFERENTE:
                    return valor != float.Parse(this.valor);
                case CondicionEstado.IGUAL:
                    return valor == float.Parse(this.valor);
                case CondicionEstado.MENOR:
                    return valor < float.Parse(this.valor);
                case CondicionEstado.MENORIGUAL:
                    return valor <= float.Parse(this.valor);
                case CondicionEstado.MAYOR:
                    return valor > float.Parse(this.valor);
                case CondicionEstado.MAYORIGUAL:
                    return valor >= float.Parse(this.valor);                    
                case CondicionEstado.SIEMPRE:
                    return true; 
            }
            return true;
        }


        public bool  IsCondicion(){
            switch(tipo){
                case CondicionTipo.METADATO:

                    string valor = "";
                    if (entidad != null)
                        valor = entidad.GetMetadato(nombre);
                    else
                        valor = ManagerGameplay.GetInstancia().GetMetadato(nombre);
                    return IsCondicionValor(valor);                                   

                case CondicionTipo.ATAQUE:
                    if (entidad == null)
                        return false;
                    if (entidad.GetModuloAtaque() == null)
                        return false;
                    return IsCondicionValor(entidad.GetModuloAtaque().GetAtaqueBase());

                case CondicionTipo.VIDA:
                    if (entidad == null)
                        return false;
                    if (entidad.GetModuloVitalidad() == null)
                        return false;
                    return IsCondicionValor(entidad.GetModuloVitalidad().GetPerfilVitalidad().GetVida());


            }
            return true;
        }


    }


}

