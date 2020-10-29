using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Aventuras{

    [System.Serializable]
    public class EntidadGeneracion{
        
        [SerializeField]
        string nombre = "Desconocido";
        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private float probabilidad = 1.0f;
        [SerializeField]
        private  float enfriamiento = 1.0f;

        private Temporizador temporizador;

        public void Start(){
            temporizador = new Temporizador(enfriamiento);
        }
        public void Update(){
            temporizador.Update();           
        }    

        public void StartTemporizador(){
            temporizador.Start();
        }

        public Entidad GetEntidad(){
            return entidad;
        }
        public float   GetProbabilidad(){
            return probabilidad;
        }
        public float   GetEnfriamiento(){
            return  enfriamiento;
        }
               
        public string GetNombre(){
            return nombre;
        }

        public bool IsNombre(string nombre){
            return this.nombre == nombre;
        }

        public bool IsActivo(){
            return temporizador.IsActivo();
        }
        public bool IsEntidad(Entidad entidad){
            return this.entidad == entidad;
        }
    
    }

    public enum GeneradorTipo{
        RAFAGA, ENTIDAD
    }

    [RequireComponent(typeof(BoxCollider))]
    public class MapaGenerador : MonoBehaviour{
        
        [Header("General")]
        [SerializeField]
        private bool automatico = true;
        [Header("Generacion - General")]
        [SerializeField]
        private EntidadGeneracion []generacion = null;
        [SerializeField]
        private Transform carpeta = null;
        [Header("Generacion - Configuracion")]
        [SerializeField]
        private GeneradorTipo tipo = new GeneradorTipo();
        [SerializeField]
        private float tiempominimo = 1;
        [SerializeField]
        private float tiempomaximo = 15;
        [SerializeField]
        private int rafaga = 3;
        [Header("Generacion - Limite")]
        [SerializeField]
        private bool limitar = true;
        [SerializeField]
        private int entidadeslimite = 10;

        private Mapa mapa = null;
        private ManagerGameplay game = null;

        private List<Entidad> entidadesgeneracion  = new List<Entidad>();
        private Probabilidad  probabilidades        = new Probabilidad();

        private Temporizador temporizador;
        private BoxCollider  area;

        private List<Entidad> entidades = new List<Entidad>();

        private void Awake(){
        
            area = GetComponent<BoxCollider>();

        }
        private void Start(){
            mapa = Mapa.GetInstancia();
            game = ManagerGameplay.GetInstancia();

            for (int i = 0; i < generacion.Length; i++)
                generacion[i].Start();

            temporizador = new Temporizador();
            ActualizarTiempo();
        }
        private void Update(){    

            temporizador.Update();

            for (int i = 0; i < entidades.Count; i++)
                if (entidades[i] == null)
                    entidades.RemoveAt(i--);

            if (game.IsEstado(GameplayEstado.JUGANDO)) {               

                for (int i = 0; i < generacion.Length; i++)
                    generacion[i].Update();
                if (temporizador.IsActivo() && automatico){
                    if(Generar())
                        temporizador.Start();
                }                                
            }
        }
            
        public  void Generar(Entidad entidad){
            if (limitar && entidades.Count >= entidadeslimite)
                return;
            if (entidad == null)
                return;

            Entidad generada = entidad.Create(carpeta,GetPosicion() + GetPosicionAleatoria());                                           
            generada.Generacion();

            entidades.Add(generada);

            ActualizarTiempo();
        }
        private bool Generar(){            
                     
            if (limitar && entidades.Count >= entidadeslimite)
                return false;

            entidadesgeneracion.Clear();
            probabilidades.Clear();

            for (int i = 0; i < generacion.Length; i++)
                if (generacion[i].IsActivo() &&
                    generacion[i].GetProbabilidad() > 0.0f)
                {                    
                    entidadesgeneracion.Add(generacion[i].GetEntidad());
                    probabilidades.AddProbabilidad(generacion[i].GetProbabilidad());
                }

            if (probabilidades.GetProbabilidadCount() == 0)
                return false;

            int cantidad = 1;
            if (tipo == GeneradorTipo.RAFAGA)
                cantidad = rafaga;
            for (int k = 0; k < cantidad; k++){
                
                int n = probabilidades.NextProbabilidad();          
                Generar(entidadesgeneracion[n]);

                for (int i = 0; i < generacion.Length; i++)
                    if (generacion[i].IsEntidad(entidadesgeneracion[n]))
                    {                    
                        generacion[i].StartTemporizador();
                        break;
                    }

            }
                    
            return true;

        }
          
        private void ActualizarTiempo(){
            float tiempo = Random.Range(tiempominimo, tiempomaximo);
            temporizador.SetTiempoTarget(tiempo);
        }

        private void SetAutomatico(bool automatico){
            this.automatico = automatico;
        }
            
        public Vector3 GetPosicionAleatoria(){
            Vector3 posicion = new Vector3(Random.Range(0, area.size.x), Random.Range(0, area.size.y), Random.Range(0, area.size.z)) 
                               - area.size/2 
                               + area.center;
            return posicion;
        }    
        public Vector3 GetPosicion(){            
             return transform.position;                        
        }

        public void AccionSetAutomatico(bool automatico){
            SetAutomatico(automatico);
        }
        public void AccionGenerar(){
            Generar();
        }
        public void AccionGenerar(string nombre){
            for (int i = 0; i < generacion.Length; i++)
                if (generacion[i].IsNombre(nombre))
                {                    
                    generacion[i].StartTemporizador();
                    Generar(generacion[i].GetEntidad());
                    break;
                }
        }


    }

}