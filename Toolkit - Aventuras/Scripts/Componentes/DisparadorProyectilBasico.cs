using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

public class DisparadorProyectilBasico : MonoBehaviour{

    [Header("Disparador - General")]
    [SerializeField]
    private float      tiempo = 0.5f;
    [SerializeField]
    private Proyectil  disparoprefab = null;

    private Temporizador temporizador = null;

    private Entidad entidad = null;

    private void Awake(){

        temporizador = new Temporizador(tiempo);

        Transform padre = transform.parent;
        while (padre != null)
        {
            entidad = padre.gameObject.GetComponent<Entidad>();
            if (entidad != null)
                break;
            padre = padre.transform.parent;
        }
    }
    private void Update(){
        temporizador.Update();
    }

    public void Disparar(){      
        if (disparoprefab == null){
            Debug.LogError("No contiene un proyectil valido.");
            return;
        }
        if (IsActivo()){

            Proyectil p = (Proyectil)disparoprefab.Create(entidad.transform.parent, transform.position);
            p.Disparar(transform.forward);
            temporizador.Start();
        }
    }
    public void AccionDisparar(){
        Disparar();
    }

    public bool IsActivo(){
        return temporizador.IsActivo();      
    }
}

}
