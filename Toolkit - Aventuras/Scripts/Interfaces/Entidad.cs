using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


namespace Aventuras{

    public enum EntidadTipo{
        DESCONOCIDO, ALIADO, ENEMIGO
    }
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entidad : MonoBehaviour{
           
        [Header("Entidad - General")]
        [SerializeField]
        private bool destruir = true;
        [SerializeField]
        private bool destruirautomatico = true;

        private List<EntidadModulo> modulos = new List<EntidadModulo>();

        private   EntidadTipo tipo      = EntidadTipo.DESCONOCIDO;
        
        protected Rigidbody   cuerporigido = null;
        protected Animador    animador = null;

        private   Vector3     rotacion  = Vector3.zero;            
        private   Mapa        mapa      = null;

        private   bool        muerto = false;


        public Entidad Create(Transform padre,Vector3 posicion){            
            Entidad instancia = GameObject.Instantiate(gameObject,posicion,Quaternion.identity,padre).GetComponent<Entidad>();
            return instancia;
        }        
        public void    Destruir(){
            if (!destruir)
                return;
            GameObject.Destroy(gameObject);
        }


        protected virtual void Awake(){
            cuerporigido = GetComponent<Rigidbody>();
            animador     = GetComponentInChildren<Animador>();
        }
        protected virtual void Start(){
            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Start();            
            mapa = Mapa.GetInstancia();
        }
        protected virtual void Update(){            

            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Update();

            if (mapa != null && destruir)
                if (!mapa.IsMapa(this))
                    Destruir();
            UpdateRotacion();
        }     

        public abstract void Generacion();
        public virtual  void Muerte(){
            SetMuerto(true);
            if(destruirautomatico)
                Destruir();
        }

        private void UpdateRotacion(){
            if (GetModuloMovimiento() == null)
                return;

            if (GetModuloMovimiento().GetDireccion().magnitude > 0){

                rotacion = GetRigidbody().velocity;
                rotacion.y = 0;
                rotacion = rotacion.normalized;

                float angulo = Mathf.Atan2(rotacion.x,rotacion.z);

                if (GetAnimador() != null){
                    GetAnimador().transform.rotation = Quaternion.Euler(0, angulo*180/Mathf.PI, 0);
                }

            }
        }

        protected void AddModulo(EntidadModulo modulo){
            if (!modulos.Contains(modulo)){
                modulo.SetEntidad(this);       
                modulos.Add(modulo);
            }
        }

        protected void     SetMuerto(bool muerto){
            this.muerto = muerto;
        }
        protected void     SetTipo(EntidadTipo tipo){
            this.tipo = tipo;
        }
        public    void     SetPosicion(Vector3 posicion){
            if (GetRigidbody() != null)
                GetRigidbody().position = posicion;
            else
                transform.position = posicion;
        }
        public    void     SetPosicion(Vector2 posicion){
            if (GetRigidbody() != null)
                GetRigidbody().position = new Vector3(posicion.x,posicion.y,GetRigidbody().position.z);
            else
                transform.position = new Vector3(posicion.x,posicion.y,transform.position.z);;
        }
            
        public EntidadTipo      GetTipo(){
            return tipo;
        }

        public Rigidbody        GetRigidbody(){
            return cuerporigido;
        }
        public Animador         GetAnimador(){
            return animador;
        }
        public Vector3          GetPosicion(){
            return transform.position;
        }
       
        public virtual ModuloMovimiento GetModuloMovimiento(){
            return null;
        }            
        public virtual ModuloDeteccion        GetModuloDeteccion(){
            return null;
        }
        public virtual ModuloAtaque           GetModuloAtaque(){
            return null;
        }
        public virtual ModuloVitalidad        GetModuloVitalidad(){
            return null;
        }

        public bool IsMuerto(){
            return muerto;
        }

        public void AccionMuerte(){
            Muerte();
        }
        public void AccionDestruir(){
            Destruir();
        }
        public void AccionPlayAudio(string codigo){
            if (ManagerAplicacion.GetInstanciaBase() != null)
                ManagerAplicacion.GetInstanciaBase().AccionPlayAudio(codigo);
        }


    }
        
}
