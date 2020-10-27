using UnityEngine;
using System.Collections;

namespace Aventuras{

    [RequireComponent(typeof(BoxCollider))]
    public class Mapa : MonoBehaviour{

        [SerializeField]
        private MapaSpawn spawn = null;

        private static Mapa instancia = null;

        private BoxCollider mapa = null;
        private Vector3 posicion = Vector3.zero;


        private void Awake(){            
            instancia = null;
            mapa     = GetComponent<BoxCollider>();
        }
            
        public void SetSpawn(MapaSpawn spawn){
            this.spawn = spawn;
        }

        public Vector3   GetPosicion(){
            return transform.position;
        }
        public MapaSpawn GetSpawn(){
            return spawn;
        }

        public bool IsMapa(Entidad entidad){            
            return mapa.bounds.Contains(entidad.transform.position);
        }
        public static Mapa GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<Mapa>();
            return instancia;
        }
            
    }

}
