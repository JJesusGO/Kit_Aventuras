using UnityEngine;
using System.Collections;

namespace Aventuras{

    public enum AliadoEstrategia{
        INSISTENTE, MENORVIDA, MASCERCANO
    }


    public abstract class Aliado : Entidad{

        [SerializeField]
        private ModuloMovimiento movimiento = new ModuloMovimiento();    
        [SerializeField]
        private ModuloVitalidad vitalidad = new ModuloVitalidad();
        [Header("Variables - Aliado")]
        [Tooltip("Deprecated")]
        [SerializeField]
        private bool target = true;

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.ALIADO);
            AddModulo(movimiento);
            AddModulo(vitalidad);
        }

        public override ModuloMovimiento GetModuloMovimiento(){
            return movimiento;
        }

        public override ModuloVitalidad GetModuloVitalidad(){
            return vitalidad;
        }

        public bool IsTarget(){
            return target;
        }
    }

}
