using UnityEngine;
using System.Collections;

namespace Aventuras{

    public abstract class Aliado : Entidad{

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.ALIADO);
        }
    
    }

}
