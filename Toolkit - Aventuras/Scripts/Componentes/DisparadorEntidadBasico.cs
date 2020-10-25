using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

public class DisparadorEntidadBasico : MonoBehaviour{

    [SerializeField]
    private Entidad entidadprefab = null;
    [SerializeField]
    private Transform carpeta = null;

    private Entidad entidad = null;

    private void Awake(){
        Transform padre = transform.parent;
        while (padre != null)
        {
            entidad = padre.gameObject.GetComponent<Entidad>();
            if (entidad != null)
                break;
            padre = padre.transform.parent;
        }
    }

    public void AccionDisparar(){  
         
        if (entidadprefab == null){
            Debug.LogError("No contiene una entidad valida.");
            return;
        }

        if (carpeta != null)
            entidadprefab.Create(carpeta, transform.position);
        else if (entidad != null)
            entidadprefab.Create(entidad.transform.parent, transform.position);
        else if(Mapa.GetInstancia() != null)
            entidadprefab.Create(Mapa.GetInstancia().transform, transform.position);

    }
  
}


}