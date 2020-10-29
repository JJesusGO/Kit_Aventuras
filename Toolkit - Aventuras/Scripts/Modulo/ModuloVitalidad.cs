using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace Aventuras{

    public delegate void VitalidadEvento(VitalidadInformacion info,ModuloVitalidad vitalidad);

    public enum   VitalidadTipo{
        DAÑO, CARGAS, MODMETA, SETMETA, MODMETAATAQUE, SETMETAATAQUE
    }
    public enum   VitalidadEventoTipo{
        PREEFECTO,EFECTO
    }
    public struct VitalidadInformacion{

        private PerfilVitalidad perfil;
        private float efecto;
        private VitalidadEventoTipo tipo;
        private Entidad entidadatacante;

        public VitalidadInformacion(PerfilVitalidad perfil,float efecto,Entidad entidadatacante,VitalidadEventoTipo tipo){
            this.perfil = perfil;
            this.efecto = efecto;
            this.entidadatacante = entidadatacante;
            this.tipo = tipo;
        }

        public PerfilVitalidad GetPerfil(){
            return perfil;
        }
        public float GetEfecto(){
            return efecto;
        }
        public VitalidadEventoTipo GetTipo(){
            return tipo;
        }
        public Entidad GetEntidadAtacante(){
            return entidadatacante;
        }

    }
        
    [System.Serializable]
    public class PerfilVitalidad{
       
        [SerializeField]
        private Colision []colisiones = null;
        [SerializeField]
        private VitalidadTipo tipo = VitalidadTipo.DAÑO; 
        [SerializeField]
        private float vida = 100.0f;
        [SerializeField]
        private float vidamaxima = 100.0f;
        [Header("Seccion - Daño")]
        [Range(0.0f,1.0f)]
        [SerializeField]
        private float reducciondaño = 0.0f;
        [Header("Seccion - Meta")]
        [SerializeField]
        private string metanombre = "Desconocido";
        [SerializeField]
        private string metavalor = "-1";
        [SerializeField]
        private float metamultiplicador = 1.0f;
        [Header("Seccion - Cargas")]
        [SerializeField]
        private int cargasporimpacto = 1;

        public void Start(){
            SetVida(vida);
            SetVidaMaxima(vidamaxima);
        }           
       
        public void ModVida(float valor){
            SetVida(vida + valor);
        }
        public void SetVida(float valor){
            vida = valor;
            if (tipo == VitalidadTipo.CARGAS)
                vida = (float)Math.Floor(vida);                        
            vida = Mathf.Clamp(vida, 0, vidamaxima);
        }
        public void ResetVida(){
            SetVida(GetVidaMaxima());
        }

        public void ModVidaMaxima(float valor){
            SetVidaMaxima(vida + valor);
        }
        public void SetVidaMaxima(float valor){            
            vidamaxima = valor;
            if (tipo == VitalidadTipo.CARGAS)
                vidamaxima = (float)Math.Floor(vidamaxima);    
            if (vidamaxima <= 0)
                vidamaxima = 0;
            if (vida >= vidamaxima)
                vida = vidamaxima;
        }
            
        public void ModReduccionDaño(float valor){
            SetReduccionDaño(reducciondaño + valor);
        }
        public void SetReduccionDaño(float valor){
            reducciondaño = valor;
            reducciondaño = Mathf.Clamp(reducciondaño, 0, 1);
        }
         
        public string GetMetaNombre(){
            return metanombre;
        }
        public string GetMetaValor(){
            return metavalor;
        }
        public float  GetMetaMultiplicador(){
            return metamultiplicador;
        }
            
        public float GetVida(bool relativa = false){
            if (relativa)
                return vida / vidamaxima;
            return vida;
        }
        public float GetVidaMaxima(){
            return vidamaxima;
        }
        public float GetReduccionDaño(){
            return reducciondaño;
        }
        public int   GetCargas(){
            return cargasporimpacto;
        }           
            
        public VitalidadTipo GetTipo(){
            return tipo;
        }

        public bool IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colisiones[i].Equals(colision))
                    return true;
            return false;
        }


    }
    [System.Serializable]
    public class ModuloVitalidad : EntidadModulo{

        [SerializeField]
        private PerfilVitalidad perfilvitalidad =null;
        [Header("Eventos")]
        private UnityEvent eventoefecto = new UnityEvent();

        private event VitalidadEvento vitalidadevento = null;

        public override void Start(){
            base.Start();
            perfilvitalidad.Start();
        }

        public void AddDanio(float ataquebasico,Entidad entidad,Colision colision){
            if (!IsEnable())
                return;                        

            if (perfilvitalidad.IsColision(colision)){

                float efecto = ataquebasico;
                    efecto -= efecto * perfilvitalidad.GetReduccionDaño(); 
                    if (perfilvitalidad.GetTipo() == VitalidadTipo.CARGAS)
                        efecto = perfilvitalidad.GetCargas();


                    if (IsEnable()){
                        switch(perfilvitalidad.GetTipo()){
                        case VitalidadTipo.DAÑO:
                        case VitalidadTipo.CARGAS:
                            SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                                efecto,
                                entidad,
                                VitalidadEventoTipo.PREEFECTO));        
                            perfilvitalidad.ModVida(-efecto);                                
                            break;
                        case VitalidadTipo.MODMETA:
                            efecto = float.Parse(perfilvitalidad.GetMetaValor());
                            SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                                    efecto,
                                    entidad,
                                    VitalidadEventoTipo.PREEFECTO));                              
                            GetEntidad().ModMetadato(perfilvitalidad.GetMetaNombre(), float.Parse(perfilvitalidad.GetMetaValor()));
                            break;
                        case VitalidadTipo.SETMETA:
                            efecto = -1;
                            SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                                    efecto,
                                    entidad,
                                    VitalidadEventoTipo.PREEFECTO));                             
                            GetEntidad().SetMetadato(perfilvitalidad.GetMetaNombre(), perfilvitalidad.GetMetaValor());
                            break;
                        case VitalidadTipo.MODMETAATAQUE:
                            efecto *= perfilvitalidad.GetMetaMultiplicador();
                            SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                                efecto,
                                entidad,
                                VitalidadEventoTipo.PREEFECTO)); 
                            GetEntidad().ModMetadato(perfilvitalidad.GetMetaNombre(), efecto*perfilvitalidad.GetMetaMultiplicador());
                            break;
                        case VitalidadTipo.SETMETAATAQUE:
                            efecto *= perfilvitalidad.GetMetaMultiplicador();
                            SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                                efecto,
                                entidad,
                                VitalidadEventoTipo.PREEFECTO)); 
                            GetEntidad().SetMetadato(perfilvitalidad.GetMetaNombre(), (efecto*perfilvitalidad.GetMetaMultiplicador()).ToString());
                            break;
                        }
                        eventoefecto.Invoke();
                    }
                    else
                        efecto = 0;

                    SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                        efecto,
                        entidad,
                        VitalidadEventoTipo.EFECTO));                                    

                }
                    
        }
      
        public void AddVitalidadEvento(VitalidadEvento evento) {
            RemoveColisionEvento(evento);
            this.vitalidadevento += (evento);
        }
        public void RemoveColisionEvento(VitalidadEvento evento){
            try{
                this.vitalidadevento -= (evento);            
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

        }

        private void SolicitarVitalidadEvento(VitalidadInformacion info){
            if (vitalidadevento != null)
                vitalidadevento(info,this);
        }

        public void ModReduccionDaño(float reduccion){
            perfilvitalidad.ModReduccionDaño(reduccion);
        }
            
        public PerfilVitalidad GetPerfilVitalidad(){
            return perfilvitalidad;
        }
            

    }


}
