using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class PropBasico : Prop{

        [SerializeField]
        private ModuloVitalidad vitalidad = new ModuloVitalidad();

        protected override void Awake(){
            base.Awake();
            AddModulo(vitalidad);
        }

        public override void Generacion(){
            
        }
        public override ModuloVitalidad GetModuloVitalidad(){
            return vitalidad;
        }

    }

}

