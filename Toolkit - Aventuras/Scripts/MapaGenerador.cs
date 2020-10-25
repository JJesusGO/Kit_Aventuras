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

    [RequireComponent(typeof(BoxCollider))]
    public class MapaGenerador : MonoBehaviour{
        
        [Header("General")]
        [SerializeField]
        private bool enable = true;
        [Header("Generacion - General")]
        [SerializeField]
        private EntidadGeneracion []generacion = null;
        [SerializeField]
        private Transform carpeta = null;
        [Header("Generacion - Configuracion")]
        [SerializeField]
        private float tiempominimo = 1;
        [SerializeField]
        private float tiempomaximo = 15;
        [SerializeField]
        private int entidadesmaximas = 3;

        private Mapa mapa = null;
        private ManagerGameplay game = null;

        private List<Entidad> entidadesdisponibles  = new List<Entidad>();
        private Probabilidad  probabilidades        = new Probabilidad();

        private Temporizador temporizador;
        private BoxCollider  area;

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
            if (!enable)
                return;

            if (game.IsEstado(GameplayEstado.JUGANDO)) {               

                for (int i = 0; i < generacion.Length; i++)
                    generacion[i].Update();

                if (temporizador.IsActivo()){
                    if(Generar())
                        temporizador.Start();
                }                                
            }
        }
            
        public  void Generar(Entidad entidad){
            if (entidad == null)
                return;
            Entidad generada = entidad.Create(carpeta,GetPosicion() + GetPosicionAleatoria());                                           
            generada.Generacion();
            ActualizarTiempo();
        }
        private bool Generar(){        
            if (!enable)
                return false;
                     
            entidadesdisponibles.Clear();
            probabilidades.Clear();

            for (int i = 0; i < generacion.Length; i++)
                if (generacion[i].IsActivo() &&
                    generacion[i].GetProbabilidad() > 0.0f)
                {                    
                    entidadesdisponibles.Add(generacion[i].GetEntidad());
                    probabilidades.AddProbabilidad(generacion[i].GetProbabilidad());
                }

            if (probabilidades.GetProbabilidadCount() == 0)
                return false;
            
            int n = probabilidades.NextProbabilidad();          
            Generar(entidadesdisponibles[n]);

            for (int i = 0; i < generacion.Length; i++)
                if (generacion[i].IsEntidad(entidadesdisponibles[n])){                    
                    generacion[i].StartTemporizador();
                    break;
                }
                    
            return true;

        }
          
        private void ActualizarTiempo(){
            float tiempo = Random.Range(tiempominimo, tiempomaximo);
            temporizador.SetTiempoTarget(tiempo);
        }
          
        public Vector3 GetPosicionAleatoria(){
            Vector3 posicion = new Vector3(Random.Range(0, area.size.x), Random.Range(0, area.size.y), Random.Range(0, area.size.z)) + area.center;
            return posicion;
        }    
        public Vector3 GetPosicion(){            
             return transform.position;                        
        }

    }

}