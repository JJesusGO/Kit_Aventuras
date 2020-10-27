using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


namespace Aventuras{

    public enum EntidadTipo{
        DESCONOCIDO, ALIADO, ENEMIGO, OBJETO
    }
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entidad : MonoBehaviour{
           
        [Header("Entidad - General")]
        [SerializeField]
        private bool destruir = true;
        [SerializeField]
        private bool muerteautomatica = true;
        [SerializeField]
        private bool destruirautomatico = true;
        [SerializeField]
        private MetaData []metadatos = null;

        private List<EntidadModulo> modulos = new List<EntidadModulo>();

        private   EntidadTipo tipo      = EntidadTipo.DESCONOCIDO;
        
        protected Rigidbody   cuerporigido = null;
        protected Animador    animador = null;

        private   Vector3     rotacion  = Vector3.zero;            
        private   Mapa        mapa      = null;

        private   bool        muerto = false;

        private ExtensionEventosEntidad eventos = null;

        public Entidad Create(Transform padre,Vector3 posicion){            
            Entidad instancia = GameObject.Instantiate(gameObject,posicion,Quaternion.identity,padre).GetComponent<Entidad>();
            return instancia;
        }        
        public void    Destruir(){
            if (!destruir)
                return;
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.DESTRUIR);
            GameObject.Destroy(gameObject);
        }

        protected virtual void Awake(){
            cuerporigido = GetComponent<Rigidbody>();
            animador     = GetComponentInChildren<Animador>();
            eventos = GetComponentInChildren<ExtensionEventosEntidad>();
        }
        protected virtual void Start(){
            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Start();            
            mapa = Mapa.GetInstancia();
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.VIVIR);
        }
        protected virtual void Update(){            

            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Update();

            if (mapa != null && destruir)
                if (!mapa.IsMapa(this))
                    Destruir();
            if (GetModuloVitalidad() != null)
                if ((GetModuloVitalidad().GetPerfilVitalidad().GetVida() <= 0) && muerteautomatica)
                    Muerte();

            UpdateRotacion();

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

        public abstract void Generacion();
        public virtual  void Muerte(){
            SetMuerto(true);
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.MORIR);
            if(destruirautomatico)
                Destruir();
        }
        public virtual  void Revivir(){
            if (!IsMuerto())
                return;

            if (GetModuloVitalidad() != null)
                GetModuloVitalidad().GetPerfilVitalidad().ResetVida();
            if (GetModuloMovimiento() != null)
                GetModuloMovimiento().Detener();
            SetMuerto(true);
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.VIVIR);            
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
        public    void     SetMetadato(string nombre,string valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].SetValor(valor);
            }
        }
        public    void     ModMetadato(string nombre,float valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].ModValor(valor);
            }
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

        public string           GetMetadato(string nombre){
            for (int i = 0; i < metadatos.Length; i++)
                if (metadatos[i].IsNombre(nombre))
                    return metadatos[i].GetValor();
            return "";
        }

                      
        public virtual ModuloMovimiento       GetModuloMovimiento(){
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
        public void AccionRevivir(){
            Revivir();
        }
        public void AccionDestruir(){
            Destruir();
        }
        public void AccionPlayAudio(string codigo){
            if (ManagerAplicacion.GetInstancia() != null)
                ManagerAplicacion.GetInstancia().AccionPlayAudio(codigo);
        }

        public void AccionSetMetadato(string comando){

            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                SetMetadato(data[0],data[1]);            

        }
        public void AccionModMetadato(string comando){

            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                ModMetadato(data[0],float.Parse(data[1]));            

        }
            
        public void AccionLog(string mensaje){
            Debug.Log(mensaje);
        }
            
        public void AccionSetGameplayMetadato(string comando){
            if (ManagerGameplay.GetInstancia() != null)
                ManagerGameplay.GetInstancia().AccionSetMetadato(comando);
        }
        public void AccionModGameplayMetadato(string comando){
            if (ManagerGameplay.GetInstancia() != null)
                ManagerGameplay.GetInstancia().AccionModMetadato(comando);               
        }
            
        public void AccionSetEnableMovimiento(bool enable){
            if (GetModuloMovimiento() != null)
                GetModuloMovimiento().SetEnable(enable);
        }
        public void AccionSetEnableAtaque(bool enable){
            if (GetModuloAtaque() != null)
                GetModuloAtaque().SetEnable(enable);
        }
        public void AccionSetEnableVitalidad(bool enable){
            if (GetModuloVitalidad() != null)
                GetModuloVitalidad().SetEnable(enable);
        }

        public void AccionSetAtaque(float ataque){            
            if (GetModuloAtaque() == null)
                return;
            GetModuloAtaque().SetAtaque(ataque);
        }
        public void AccionModAtaque(float ataque){            
            if (GetModuloAtaque() == null)
                return;
            GetModuloAtaque().ModAtaque(ataque);
        }

        public void AccionSetVida(float vida){
            if (GetModuloVitalidad() == null)
                return;
            GetModuloVitalidad().GetPerfilVitalidad().SetVida(vida);
        }
        public void AccionModVida(float vida){
            if (GetModuloVitalidad() == null)
                return;
            GetModuloVitalidad().GetPerfilVitalidad().ModVida(vida);
        }

        public void AccionSetVidaMax(float vida){
            if (GetModuloVitalidad() == null)
                return;
            GetModuloVitalidad().GetPerfilVitalidad().SetVidaMaxima(vida);
        }
        public void AccionModVidaMax(float vida){
            if (GetModuloVitalidad() == null)
                return;
            GetModuloVitalidad().GetPerfilVitalidad().ModVidaMaxima(vida);
        }


    }
        
}
