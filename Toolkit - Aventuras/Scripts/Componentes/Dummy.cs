using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class Dummy : Enemigo{

        protected override void Awake(){
            base.Awake();
            GetModuloVitalidad().AddVitalidadEvento(EventoVitalidad);
        }
        public override void Generacion(){
            
        }   
            
        public void EventoVitalidad(VitalidadInformacion info,ModuloVitalidad vitalidad){
            if (info.GetTipo() == VitalidadEventoTipo.EFECTO)
                Debug.Log(info.GetEfecto()+" "+vitalidad.GetPerfilVitalidad().GetTipo());
        }

       
    }


}