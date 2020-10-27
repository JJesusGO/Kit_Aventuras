using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

    [RequireComponent(typeof(Entidad))]
    public class ExtensionSalto : Extension{

        [Header("Variables")]
        [SerializeField]
        private float impulso = 5.0f;
        [SerializeField]
        private int saltosdisponibles = 1;
        [Header("Controles")]
        [SerializeField]
        private Tecla salto = new Tecla("Salto");
        [SerializeField]
        private Colision deteccionpiso = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventosalto = new UnityEvent();
        [SerializeField]
        private UnityEvent eventopiso  = new UnityEvent();

        private int saltos = 0; 
        private bool estadopiso = false;

        private Entidad entidad = null;

        protected override void Awake(){
            base.Awake();
            entidad = GetComponent<Entidad>();

            if (entidad.GetModuloMovimiento() == null)
                Debug.LogError("La entidad debe tener el modulo de movimiento.");
            if (deteccionpiso != null)
                deteccionpiso.AddColisionEvento(EventoPiso);
        }

        private void Update(){

            if (entidad.GetModuloMovimiento() == null)
                return;
            if (salto.IsClickDown() && (saltos < saltosdisponibles)){
                eventosalto.Invoke();
                entidad.GetModuloMovimiento().Impulso(new Vector3(0, impulso, 0));  
                saltos++;
            }

        }

        private void EventoPiso(ColisionInformacion info){        
            if (info.GetColisionTipo() == ColisionTipo.TRIGGER){
                if (info.GetColisionEstado() == ColisionEstado.ENTER){
                    saltos = 0;
                    eventopiso.Invoke();
                }
                estadopiso = info.GetColisionEstado() == ColisionEstado.ENTER;            
            }
        }

    }

}

