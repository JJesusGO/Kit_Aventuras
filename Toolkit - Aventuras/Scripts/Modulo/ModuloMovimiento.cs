using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{


    [System.Serializable]
    public class ModuloMovimiento : EntidadModulo{

        [Header("General")]
        [SerializeField]
        private float velocidad = 7.0f;
        [SerializeField]
        private float minimavelocidad = 0.2f;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventomovimiento = new UnityEvent();
        [SerializeField]
        private UnityEvent eventodetenerse = new UnityEvent();

        private ManagerGameplay gameplay = null;
        private Vector3 direccion = Vector3.forward;

        private bool estadodetenerse = false;
        private bool estadomovimiento = false;

        public override void Start(){
            gameplay = ManagerGameplay.GetInstancia();
            if (velocidad <= 0)
                velocidad = 0.0001f;
            SetVelocidad(velocidad);
        }
        public override void Update(){
            base.Update();
            ActualizarVelocidad();
        }
     
        public void  ActualizarVelocidad(){
            if (!IsEnable())
                return;
            if (gameplay == null)
                gameplay = ManagerGameplay.GetInstancia();

            Rigidbody r = GetEntidad().GetRigidbody();
            if (r == null)
                return;
            
            if (!gameplay.IsEstado(GameplayEstado.JUGANDO)){
                Detener();
            }
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

            if (Mathf.Abs(rvelocidad.x) > minimavelocidad  && 
                Mathf.Abs(rvelocidad.z) > minimavelocidad)
            {

                if (Mathf.Abs(rvelocidad.x) >= velocidad * 0.7072f)
                    rvelocidad.x = Mathf.Sign(rvelocidad.x) * (velocidad * 0.7072f);
                else
                    rvelocidad.x = rvelocidad.x*0.55f;
                if (Mathf.Abs(rvelocidad.z) >= velocidad * 0.7072f)
                    rvelocidad.z = Mathf.Sign(rvelocidad.z) * (velocidad * 0.7072f);
                else
                    rvelocidad.z = rvelocidad.z*0.55f;
            }
            else{

                if (Mathf.Abs(rvelocidad.x) >= velocidad)
                    rvelocidad.x = Mathf.Sign(rvelocidad.x) * (velocidad);
                if (Mathf.Abs(rvelocidad.z) >= velocidad)
                    rvelocidad.z = Mathf.Sign(rvelocidad.z) * (velocidad);

            }
            r.velocity = rvelocidad;

            if ( Mathf.Abs(r.velocity.x) > minimavelocidad || Mathf.Abs(r.velocity.z) > minimavelocidad)
            {
                if (!estadomovimiento)
                    eventomovimiento.Invoke();
                estadomovimiento = true;
                estadodetenerse = false;
            }
            else if(r.velocity.magnitude <= minimavelocidad){
                if (!estadodetenerse)
                    eventodetenerse.Invoke();
                estadomovimiento = false;
                estadodetenerse = true;
            }


        }

        public void Detener(){
            Rigidbody r = GetEntidad().GetRigidbody();
            if (r == null)
                return;
            Vector3 rvelocidad = r.velocity;
            rvelocidad.x = 0;
            rvelocidad.z = 0;

            AjustarVelocidad(rvelocidad);

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

