using UnityEngine;
using UnityEditor;
using System;

namespace Aventuras
{


    public enum ModoAtaque { 
        ITERABLE, DISPARO
    }
    public enum AtaqueEstado { 
        DELAY,ATAQUE    
    }

    [RequireComponent(typeof(PerfilAtaque))]
    public class ExtensionPerfilAtaque : Extension {

        [SerializeField]
        private bool enable = true;
        [SerializeField]
        private ModoAtaque modo = ModoAtaque.ITERABLE;
        [Header("Configuracion")]
        [SerializeField]
        private float delay = 0.8f;
        [SerializeField]
        private float duracion = 0.2f;

        private AtaqueEstado estado = AtaqueEstado.DELAY;
        private Temporizador temporizador = null;
        private PerfilAtaque perfil = null;

        protected override void Awake(){
            base.Awake();
            temporizador = new Temporizador(delay);
            perfil = GetComponent<PerfilAtaque>();
            estado = AtaqueEstado.DELAY;
        }
        private void Start(){
            if(modo == ModoAtaque.ITERABLE)
                temporizador.Start();
        }

        private void Update(){
            temporizador.Update();
            if (temporizador.IsActivo()) {
                if (estado == AtaqueEstado.DELAY && modo == ModoAtaque.ITERABLE && enable) {
                    temporizador.SetTiempoTarget(duracion);
                    estado = AtaqueEstado.ATAQUE;
                    perfil.SetEnable(true);
                    temporizador.Start();
                }
                else if (estado == AtaqueEstado.ATAQUE){
                    temporizador.SetTiempoTarget(delay);
                    estado = AtaqueEstado.DELAY;
                    perfil.SetEnable(false);
                    temporizador.Start();
                }
            }
        }
        private void Disparo() {
            if (!enable)
                return;
            temporizador.SetTiempoTarget(duracion);
            estado = AtaqueEstado.ATAQUE;

            perfil.SetEnable(true);
        }


        public void SetEnable(bool enable)
        {
            this.enable = enable;
        }
        private void ToogleEnable()
        {
            this.enable = !this.enable;
        }

        public void AccionDisparo() {
            Disparo();        
        }

        public void AccionSetEnable(bool enable)
        {
            SetEnable(enable);
        }
        public void AccionToogleEnable()
        {
            ToogleEnable();
        }

    }
}