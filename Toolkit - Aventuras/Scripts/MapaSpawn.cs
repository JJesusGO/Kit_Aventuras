using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class MapaSpawn : MonoBehaviour{

        public void    Seleccionar(){
            Mapa.GetInstancia().SetSpawn(this);
        }
        public Vector3 GetPosicion(){
            return transform.position;
        }

        public void AccionSeleccionar(){
            Seleccionar();
        }

    }

}
