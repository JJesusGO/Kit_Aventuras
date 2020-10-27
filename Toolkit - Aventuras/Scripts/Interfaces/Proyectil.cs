using UnityEngine;
using System.Collections;

namespace Aventuras{

    public abstract class Proyectil : Entidad{

        [Header("Proyectil")]
        [SerializeField]
        private EntidadTipo tipoproyectil = EntidadTipo.DESCONOCIDO;
        [SerializeField]
        private ModuloMovimiento movimiento = new ModuloMovimiento();

        protected override void Awake(){
            base.Awake();
            AddModulo(movimiento);
            SetTipo(tipoproyectil);
        }
     
        public abstract void Disparar(Vector3 direccion);

        public override ModuloMovimiento GetModuloMovimiento(){
            return movimiento;
        }

    }

}

