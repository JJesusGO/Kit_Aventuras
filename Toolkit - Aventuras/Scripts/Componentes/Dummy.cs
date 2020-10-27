using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class Dummy : Enemigo{

        [SerializeField]
        private ModuloVitalidad vitalidad = new ModuloVitalidad();

        protected override void Awake(){
            base.Awake();
            AddModulo(vitalidad);
            vitalidad.AddVitalidadEvento(EventoVitalidad);
        }
        public override void Generacion(){
            
        }   


        public override ModuloVitalidad GetModuloVitalidad(){
            return vitalidad;
        }

        public void EventoVitalidad(VitalidadInformacion info,ModuloVitalidad vitalidad){
            if (info.GetTipo() == VitalidadEventoTipo.EFECTO)
                Debug.Log(info.GetEfecto()+" "+vitalidad.GetPerfilVitalidad().GetTipo());
        }

       
    }


}