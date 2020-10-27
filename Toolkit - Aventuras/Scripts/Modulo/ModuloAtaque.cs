using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace Aventuras{

    public delegate void AtaqueEvento(AtaqueInformacion info,ModuloAtaque ataque);

    public enum   AtaqueObjetivo{
        ALIADO, ENEMIGO, AMBOS
    }
    public struct AtaqueInformacion{
       
        private PerfilAtaque perfil;
        private float ataquebasico;
        private Entidad entidad,entidadatacada;

        public AtaqueInformacion(PerfilAtaque perfil,float ataquebasico, Entidad entidad,Entidad entidadatacada){
            this.perfil = perfil;
            this.ataquebasico = ataquebasico;
            this.entidad = entidad;
            this.entidadatacada = entidadatacada;
        }

        public PerfilAtaque GetPerfil(){
            return perfil;
        }
        public float        GetAtaqueBasico(){
            return ataquebasico;
        }
        public Entidad      GetEntidad(){
            return entidad;
        }
        public Entidad      GetEntidadAtacada(){
            return entidadatacada;
        }

    }

    [System.Serializable]
    public class ModuloAtaque : EntidadModulo{

        [Header("Configuracion")]
        [SerializeField]
        private float ataque = 0.0f;
        [SerializeField]
        private AtaqueObjetivo objetivo = AtaqueObjetivo.AMBOS;
        [Header("Perfiles de ataque")]
        [SerializeField]
        private PerfilAtaque[] perfiles = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventoataque = new UnityEvent();

        private ManagerGameplay game = null;
        private event AtaqueEvento ataqueevento;

        public override void Start(){
            base.Start();
            game = ManagerGameplay.GetInstancia();
            if(perfiles!=null)
                for (int i = 0; i < perfiles.Length; i++)
                    perfiles[i].SetEvento(EventoColision);
        }

        private void SolicitarEvento(AtaqueInformacion info){
            if (ataqueevento != null)
                ataqueevento(info,this);
            eventoataque.Invoke();
        }

        public void AddAtaqueEvento(AtaqueEvento evento) {
            RemoveAtaqueEvento(evento);
            ataqueevento += (evento);
        }
        public void RemoveAtaqueEvento(AtaqueEvento evento){
            try{
                ataqueevento -= (evento);            
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

        }

        public void  SetAtaque(float ataque){
            this.ataque = ataque;
            if (this.ataque < 0)
                this.ataque = 0;
        }
        public void  ModAtaque(float mod){
            SetAtaque(ataque + mod);
        }                             
       
        public float GetAtaque(PerfilAtaque perfil = null){
            float a = GetAtaqueBase();           
            if (perfil != null)
                a *= perfil.GetMultiplicador();
            return a;
        }
        public float GetAtaqueBase(){
            return ataque;
        }
            
        public PerfilAtaque GetPerfil(Colision colision){
            for (int i = 0; i < perfiles.Length; i++)
                if (perfiles[i].IsColision(colision))
                    return perfiles[i];
            return null;
        }
        public PerfilAtaque GetPerfiles(int i){
            return perfiles[i];
        }
        public int          GetPerfilesCount(){
            return perfiles.Length;
        }            

        public AtaqueObjetivo GetObjetivo(){
            return objetivo;
        }

        private void EventoColision(ColisionInformacion info){

            if (!IsEnable())
                return;

            Entidad entidad = info.GetEntidadImpacto();
            if (entidad == null || info.GetColisionEstado() != ColisionEstado.ENTER)
                return;
            ModuloVitalidad vitalidad = entidad.GetModuloVitalidad();
            if (vitalidad == null)
                return;
            PerfilAtaque perfil = GetPerfil(info.GetColision());
            if (perfil == null)
                return;
                         
            switch(GetObjetivo()){
                case AtaqueObjetivo.ALIADO:

                    if (entidad.GetTipo() == EntidadTipo.ALIADO){                        
                        SolicitarEvento(new AtaqueInformacion(
                                perfil,
                                GetAtaque(perfil),
                                GetEntidad(),
                                entidad
                            ));
                        vitalidad.AddDanio(GetAtaque(perfil),entidad, info.GetColisionImpacto());
                    }

                    break;
                case AtaqueObjetivo.ENEMIGO:

                    if (entidad.GetTipo() == EntidadTipo.ENEMIGO){
                        Debug.Log("ATAQUE");
                        SolicitarEvento(new AtaqueInformacion(
                            perfil,
                            GetAtaque(perfil),
                            GetEntidad(),
                            entidad
                        ));
                        vitalidad.AddDanio(GetAtaque(perfil),entidad, info.GetColisionImpacto());
                    }

                    break;
                case AtaqueObjetivo.AMBOS:

                    if (entidad.GetTipo() != EntidadTipo.DESCONOCIDO)
                    {
                        SolicitarEvento(new AtaqueInformacion(
                            perfil,
                            GetAtaque(perfil),
                            GetEntidad(),
                            entidad
                        ));
                        vitalidad.AddDanio(GetAtaque(perfil),entidad, info.GetColisionImpacto());

                    }

                    break;
            }

        }    

    }

}

