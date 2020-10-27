using UnityEngine;
using System.Collections;

namespace Aventuras{

    public abstract class Item : Entidad{

     
        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.OBJETO);
        }
             
      
    }

}

