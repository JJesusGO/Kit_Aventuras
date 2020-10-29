using UnityEngine;
using System.Collections;

namespace Aventuras{

    public enum EnemigoEstrategia{
        INSISTENTE, MENORVIDA, MASCERCANO, JUGADOR
    }

    public abstract class Enemigo : Entidad{

        [SerializeField]
        private ModuloMovimiento movimiento = new ModuloMovimiento();    
        [SerializeField]
        private ModuloVitalidad vitalidad = new ModuloVitalidad();
        [Header("Variables - Enemigo")]
        [SerializeField]
        private bool target = true;

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.ENEMIGO);
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

