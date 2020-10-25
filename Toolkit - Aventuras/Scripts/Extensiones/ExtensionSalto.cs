using UnityEngine;
using System.Collections;

namespace Aventuras{


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

        private int saltos = 0; 
        private bool estadopiso = false;

        protected override void Awake(){
            base.Awake();
            if (GetEntidad().GetModuloMovimiento() == null)
                Debug.LogError("La entidad debe tener el modulo de movimiento.");
            if (deteccionpiso != null)
                deteccionpiso.AddColisionEvento(EventoPiso);
        }

        private void Update(){

            if (GetEntidad().GetModuloMovimiento() == null)
                return;
            if (salto.IsClickDown() && (saltos < saltosdisponibles)){
                GetEntidad().GetModuloMovimiento().Impulso(new Vector3(0, impulso, 0));  
                saltos++;
            }

        }

        private void EventoPiso(ColisionInformacion info){        
            if (info.GetColisionTipo() == ColisionTipo.TRIGGER){
                if (info.GetColisionEstado() == ColisionEstado.ENTER)
                    saltos = 0;
                estadopiso = info.GetColisionEstado() == ColisionEstado.ENTER;            
            }
        }

    }

}

