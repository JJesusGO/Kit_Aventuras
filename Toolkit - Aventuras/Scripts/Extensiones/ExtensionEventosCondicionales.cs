using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Aventuras{


    public enum EventoTipo{
        REPETITIVO, UNICO
    }
    public enum EventoVariable{
        METADATO,VIDA,ATAQUE
    }

   
    [System.Serializable]
    public class EventoCondicional{

        [SerializeField]
        private string nombre = "Desconocido";
        [SerializeField]
        private Condicion []condiciones = null;
        [SerializeField]
        private EventoTipo tipo = EventoTipo.REPETITIVO;
        [SerializeField]
        private UnityEvent evento = new UnityEvent();

        private bool eventoejecutado = false;

        public void Update(){
            if (!eventoejecutado || tipo == EventoTipo.REPETITIVO){
                if (IsCondicion()){
                    evento.Invoke();
                    eventoejecutado = true;
                }
            }
        }

        public void ResetEvento(){
            eventoejecutado = false;
        }

        private bool IsCondicion(){
            bool condicion = true;
            if(condiciones!=null)
                for(int i=0;i<condiciones.Length;i++)
                    condicion = condicion && condiciones[i].IsCondicion();
            return condicion;
        }
        public bool  IsNombre(string nombre){
            return this.nombre == nombre;
        }
   
    }

    public class ExtensionEventosCondicionales : Extension{        

        [SerializeField]
        private EventoCondicional[] eventos = null;

        private void Update(){

            if (eventos != null)
                for (int i = 0; i < eventos.Length; i++)
                    eventos[i].Update();

        }

        private void ResetearEvento(string nombre){
            if (eventos != null)
                for (int i = 0; i < eventos.Length; i++)
                    if (eventos[i].IsNombre(nombre))
                        eventos[i].ResetEvento();                    
        }
      
        public void AccionResetearEvento(string nombre){
            this.ResetearEvento(nombre);
        }
    }

}
