﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{

    public enum AreaObjetivo{
        ENTIDAD, JUGADOR, METADATA,TODOS
    }

public class Area : MonoBehaviour{

    [Header("Deteccion")]
    [SerializeField]
    private AreaObjetivo objetivo = AreaObjetivo.TODOS;    
    [Header("Variables")]
    [SerializeField]
    private Entidad entidadobjetivo = null;    
    [SerializeField]
    private string metanombre = "";
    [SerializeField]
    private string metaclase = ""; 
    [Header("Eventos")]
    [SerializeField]
    private UnityEvent eventoentrar = new UnityEvent();
    [SerializeField]
    private UnityEvent eventosalir = new UnityEvent();

    private Entidad entidad = null;

    private void Awake(){

        Colision[] colisiones = GetComponentsInChildren<Colision>();
        if (colisiones != null)
            for (int i = 0; i < colisiones.Length; i++)
                colisiones[i].AddColisionEvento(EventoColision);

    }
    private void EventoColision(ColisionInformacion info){

        Entidad entidad = info.GetEntidadImpacto();
        if (entidad == null)
            return;
                          
        switch(objetivo){
            case AreaObjetivo.JUGADOR:
                    Jugador jugador = entidad as Jugador;
                if (jugador != null){
                    this.entidad = entidad;
                    SolicitarEvento(info.GetColisionEstado() == ColisionEstado.ENTER);               
                }
                break;
            case AreaObjetivo.ENTIDAD:               
                if (entidad==entidadobjetivo){
                    this.entidad = entidad;
                    SolicitarEvento(info.GetColisionEstado() == ColisionEstado.ENTER);               
                }
                break;
            case AreaObjetivo.METADATA: 
                string valor = entidad.GetMetadato(metanombre);
                string[] clases = valor.Split(' ');
                if(clases!=null)
                    for(int i=0;i<clases.Length;i++)
                        if (clases[i] == metaclase){
                            this.entidad = entidad;
                            SolicitarEvento(info.GetColisionEstado() == ColisionEstado.ENTER);               
                        }
                break;
            case AreaObjetivo.TODOS:

                this.entidad = entidad;
                SolicitarEvento(info.GetColisionEstado() == ColisionEstado.ENTER);

                break;
        }

    }    

    private void SolicitarEvento(bool entrar){
        if (entrar)
            eventoentrar.Invoke();
        else
            eventosalir.Invoke();
    }


    public void AccionMatarEntidad(){
        if (entidad == null)
            return;
        entidad.Muerte();
    }
    public void AccionDestruirEntidad(){
        if (entidad == null)
            return;
        entidad.Destruir();
    }
    public void AccionActivarTrigger(string trigger){
        if (entidad == null)
            return;
        if(entidad.GetAnimador()!=null)
            entidad.GetAnimador().ActivarTrigger(trigger);
    }

}

}
