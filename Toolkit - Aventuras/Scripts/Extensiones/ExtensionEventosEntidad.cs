using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

    public enum EventoEntidad{
        VIVIR,MORIR,DESTRUIR,PERDER,GANAR
    }

    [RequireComponent(typeof(Entidad))]
    public class ExtensionEventosEntidad : Extension{

        [Header("Eventos")]
        [SerializeField]
        private UnityEvent vivir = new UnityEvent();
        [SerializeField]
        private UnityEvent morir = new UnityEvent();
        [SerializeField]
        private UnityEvent destruir = new UnityEvent();
        [SerializeField]
        private UnityEvent perder = new UnityEvent();
        [SerializeField]
        private UnityEvent ganar = new UnityEvent();

        public void Update(){

            if(ManagerGameplay.GetInstancia().IsEstado(GameplayEstado.GANAR)){
                EntidadEvento(EventoEntidad.GANAR);
            }
            if(ManagerGameplay.GetInstancia().IsEstado(GameplayEstado.GANAR)){
                EntidadEvento(EventoEntidad.PERDER);
            }

        }

        public void EntidadEvento(EventoEntidad evento){            
            switch(evento){
                case EventoEntidad.VIVIR:
                    vivir.Invoke();
                    break;
                case EventoEntidad.MORIR:
                    morir.Invoke();
                    break;
                case EventoEntidad.DESTRUIR:
                    destruir.Invoke();
                    break;
                case EventoEntidad.PERDER:
                    perder.Invoke();
                    break;
                case EventoEntidad.GANAR:
                    ganar.Invoke();
                    break;
            }
        }

       
    }

}
