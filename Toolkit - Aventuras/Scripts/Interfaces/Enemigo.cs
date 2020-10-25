using UnityEngine;
using System.Collections;

namespace Aventuras{

    public abstract class Enemigo : Entidad{

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.ENEMIGO);
        }

    }

}

