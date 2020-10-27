using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

    [System.Serializable]
    public class TeclaEvento{
        [SerializeField]
        private Tecla tecla = new Tecla("Desconocido");
        [SerializeField]
        private Condicion []condiciones = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventodown = null;
        [SerializeField]
        private UnityEvent evento = null;
        [SerializeField]
        private UnityEvent eventoup = null;

        public void Update(){

            bool condicion = true;

            if (condiciones != null)
                for (int i = 0; i < condiciones.Length; i++)
                    condicion = condicion && condiciones[i].IsCondicion();
            
            if (condicion){
                if (tecla.IsClick())
                    evento.Invoke();
                if (tecla.IsClickDown())
                    eventodown.Invoke();
                if (tecla.IsClickUp())
                    eventoup.Invoke();
            }
            
        }
    }

}