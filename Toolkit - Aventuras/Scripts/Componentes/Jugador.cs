using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class Jugador : Aliado{

        [SerializeField]
        ModuloMovimiento modulomovimiento = new ModuloMovimiento();

        [Header("Movimiento - Jugador")]    
        [SerializeField]
        private Eje ejex = new Eje("EjeX");
        [SerializeField]
        private Eje ejey = new Eje("EjeZ");

        private Vector3 direccion = Vector3.zero;

        protected override void Awake(){
            base.Awake();
            AddModulo(modulomovimiento);

        }
        protected override void Update(){
            base.Update();

            Vector3 direccion = new Vector3(-ejex.GetValor(), 0,ejey.GetValor());
            GetModuloMovimiento().SetDireccion(direccion);

            if (direccion == Vector3.zero)
                GetModuloMovimiento().Detener();          

        }

        public override void Generacion(){
            
        }
            
        public override ModuloMovimiento GetModuloMovimiento(){
            return modulomovimiento;
        }



    }

}

