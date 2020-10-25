using UnityEngine;
using System.Collections;

namespace Aventuras{


    [System.Serializable]
    public class ModuloMovimiento : EntidadModulo{

        [Header("General")]
        [SerializeField]
        private float velocidad = 7.0f;

        private ManagerGameplay gameplay = null;
        private Vector3 direccion = Vector3.forward;

        public override void Start(){
            gameplay = ManagerGameplay.GetInstancia();
            SetVelocidad(velocidad);
        }
        public override void Update(){
            base.Update();
            ActualizarVelocidad();
        }
     
        public void  ActualizarVelocidad(){
            if (!IsEnable())
                return;

            Rigidbody r = GetEntidad().GetRigidbody();
            if (r == null)
                return;
            if (!gameplay.IsEstado(GameplayEstado.JUGANDO))
                r.velocity = Vector3.zero;            
            else {
                                       
                direccion = direccion.normalized;
                AjustarVelocidad(r.velocity + (velocidad * direccion));                       

                /*
                Vector3 target = posiciondeseada;
                target.y = 0;
                Vector3 origen = r.position;
                origen.y = 0;

                float distancia = (target - origen).magnitude; 
                direccion = (target - origen).normalized;

                float k = 1.0f;
                if (distancia < distanciaactivacion)
                    k = distancia / distanciaactivacion * velocidadajuste;

                AjustarVelocidad(r,r.velocity + ((k * velocidad) * direccion));                       
                        */                                  

            }

        }            
        private void AjustarVelocidad(Vector3 rvelocidad){

            Rigidbody r = GetEntidad().GetRigidbody();

            if (rvelocidad.x != 0 && rvelocidad.z != 0)
            {

                if (Mathf.Abs(rvelocidad.x) >= velocidad*0.75f)
                    rvelocidad.x = Mathf.Sign(rvelocidad.x) * (velocidad*0.75f);
                if (Mathf.Abs(rvelocidad.z) >= velocidad*0.75f)
                    rvelocidad.z = Mathf.Sign(rvelocidad.z) * (velocidad*0.75f);
            }
            r.velocity = rvelocidad;

        }

        public void Detener(){
            Rigidbody r = GetEntidad().GetRigidbody();
            if (r == null)
                return;
            Vector3 rvelocidad = r.velocity;
            rvelocidad.x = 0;
            rvelocidad.z = 0;
            r.velocity = rvelocidad;
        }
        public void Impulso(Vector3 impulso){
            Rigidbody r = GetEntidad().GetRigidbody();
            AjustarVelocidad(r.velocity+impulso);
        }

        public void SetVelocidad(float velocidad){
            this.velocidad = Mathf.Abs(velocidad);
            ActualizarVelocidad();
        }       
        public void SetDireccion(Vector3 direccion){
            this.direccion = direccion;
        }     

        public Vector3 GetDireccion(){
            return direccion;
        }

    }

}

