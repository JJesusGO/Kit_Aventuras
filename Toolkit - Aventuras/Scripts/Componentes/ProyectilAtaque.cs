using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class ProyectilAtaque : Proyectil{

        [SerializeField]
        protected ModuloAtaque ataque = new ModuloAtaque(); 
        [Header("Proyectil - Configuración")]
        [SerializeField]
        private bool destruirimpacto = true;

        protected override void Awake(){                        
            base.Awake();
            ataque.AddAtaqueEvento(EventoAtaque);
            AddModulo(ataque);
        }
        public override void Generacion(){
            
        }
        public override void Disparar(Vector3 direccion){
            GetModuloMovimiento().SetDireccion(direccion);
        }                              

        public override ModuloAtaque GetModuloAtaque(){
            return ataque;
        }

        /*----------EVENTOS----------*/

        private void EventoAtaque(AtaqueInformacion info,ModuloAtaque modulo){
            if (destruirimpacto)
                Muerte();
        }
      
    }

}
