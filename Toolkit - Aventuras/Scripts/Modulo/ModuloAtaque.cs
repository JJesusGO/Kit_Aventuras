using UnityEngine;
using System;
using System.Collections;

namespace Aventuras{


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

    public delegate void AtaqueEvento(AtaqueInformacion info,ModuloAtaque ataque);

    [System.Serializable]
    public class PerfilAtaque{
        
        [SerializeField]
        private string nombre = "Desconocido";
        [SerializeField]
        private Colision []colisiones = null;
        [SerializeField]
        private AtaqueObjetivo objetivo = AtaqueObjetivo.AMBOS;
        [Range(0,1)]
        [SerializeField]
        private float ataquebasico = 1.0f;

        public void Start(ColisionEvento evento){
            for(int i=0;i<colisiones.Length;i++)
                colisiones[i].AddColisionEvento(evento);
        }
                                        
        public float GetAtaqueBasico(){
            return ataquebasico;
        }
        public AtaqueObjetivo GetObjetivo(){
            return objetivo;
        }
        public string GetNombre(){
            return nombre;
        }

        public bool IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colision == colisiones[i])
                    return true;
            return false;
            
        }
    }
   
    [System.Serializable]
    public class ModuloAtaque : EntidadModulo{

        [Header("Ataque - Base")]
        [SerializeField]
        private float ataque = 0.0f;
        [Header("Perfiles de ataque")]
        [SerializeField]
        private PerfilAtaque[] perfiles = null;

        private ManagerGameplay game = null;
        private event AtaqueEvento ataqueevento;

        public override void Start(){
            base.Start();
            game = ManagerGameplay.GetInstancia();
            for (int i = 0; i < perfiles.Length; i++)
                perfiles[i].Start(EventoColision);

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
            
        private void SolicitarEvento(AtaqueInformacion info){
            if (ataqueevento != null)
                ataqueevento(info,this);
        }

        public void  ModAtaque(float mod){
            ataque += mod;
            if (ataque < 0)
                ataque = 0;
        }                             
        public float GetAtaque(PerfilAtaque perfil = null){
            float a = GetAtaqueBase();           
            if (perfil != null)
                a *= perfil.GetAtaqueBasico();
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

            switch(perfil.GetObjetivo()){
                case AtaqueObjetivo.ALIADO:

                    if (entidad.GetTipo() == EntidadTipo.ALIADO)
                    {

 
                        SolicitarEvento(new AtaqueInformacion(
                                perfil,
                                GetAtaque(perfil),
                                GetEntidad(),
                                entidad
                            ));
                        vitalidad.AddDaño(GetAtaque(perfil),entidad, info.GetColisionImpacto());
                    }

                    break;
                case AtaqueObjetivo.ENEMIGO:

                    if (entidad.GetTipo() == EntidadTipo.ENEMIGO){
                        
                        SolicitarEvento(new AtaqueInformacion(
                            perfil,
                            GetAtaque(perfil),
                            GetEntidad(),
                            entidad
                        ));
                        vitalidad.AddDaño(GetAtaque(perfil),entidad, info.GetColisionImpacto());
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
                        vitalidad.AddDaño(GetAtaque(perfil),entidad, info.GetColisionImpacto());

                    }

                    break;
            }

        }    

    }

}

