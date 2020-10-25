using UnityEngine;
using System.Collections;

namespace Aventuras{

    [RequireComponent(typeof(Entidad))]
    public abstract class Extension : MonoBehaviour{

        private Entidad entidad = null;

        protected virtual void Awake(){
            entidad = GetComponent<Entidad>();

        }


        protected Entidad GetEntidad(){
            return entidad;
        }
       
    }

}
