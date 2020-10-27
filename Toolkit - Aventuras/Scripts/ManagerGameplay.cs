using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;


namespace Aventuras{

    public enum GameplayEstado{
        JUGANDO,PERDER, GANAR
    }

    [System.Serializable]
    public class MetaData{
        
        [SerializeField]
        public string nombre = "Desconocido";
        [SerializeField]
        public string valor = "";

        public string GetValor(){
            return valor;
        }
        public string GetNombre(){
            return nombre;
        }

        public void SetValor(string valor){
            this.valor = valor;
        }
        public void ModValor(float valor){
            float numero = float.Parse(this.valor);
            numero += valor;
            this.valor = numero.ToString();
        }


        public bool IsNombre(string nombre){
            return this.nombre == nombre;
        }            

    }

    public class ManagerGameplay : MonoBehaviour {

        [Header("General")]
        [SerializeField]
        private MetaData []metadatos = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventojugando = new UnityEvent();
        [SerializeField]
        private UnityEvent eventoperder = new UnityEvent();
        [SerializeField]
        private UnityEvent eventoganar = new UnityEvent();

        private static ManagerGameplay instancia = null;
        private GameplayEstado estado = GameplayEstado.JUGANDO;

        protected virtual void Awake() {
            instancia = this;
        }
        protected virtual void Start() {
            SetEstado(GameplayEstado.JUGANDO, true);
        }

        protected void Jugando() {
            eventojugando.Invoke();
        }
        protected void Perder() {
            eventoperder.Invoke();
        }
        protected void Ganar() {
            eventoganar.Invoke();
        }

        public void ReiniciarNivel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetEstado(GameplayEstado estado, bool forzar = false) {
            if (this.estado == estado && !forzar)
                return;
            this.estado = estado;
            switch (this.estado) {
                case GameplayEstado.JUGANDO:
                    Jugando();
                    break;
                case GameplayEstado.GANAR:
                    Ganar();
                    break;
                case GameplayEstado.PERDER:
                    Perder();
                    break;
            }
        }

        public void SetMetadato(string nombre,string valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].SetValor(valor);
            }
        }
        public void ModMetadato(string nombre,float valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].ModValor(valor);
            }
        }

        public string GetMetadato(string nombre){
            for (int i = 0; i < metadatos.Length; i++)
                if (metadatos[i].IsNombre(nombre))
                    return metadatos[i].GetValor();
            return "";
        }

        public bool IsEstado(GameplayEstado estado) {
            return this.estado == estado;
        }

        public void AccionGanar() {
            if (IsEstado(GameplayEstado.JUGANDO))
                SetEstado(GameplayEstado.GANAR);
        }
        public void AccionPerder() {
            if (IsEstado(GameplayEstado.JUGANDO))
                SetEstado(GameplayEstado.PERDER);
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
            
        public static ManagerGameplay GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ManagerGameplay>();
            return instancia;
        }
            
    }


}
