using UnityEngine;
using System.Collections;

namespace Aventuras
{
    public class ExtensionJugador : Extension{

        private Jugador jugador = null;
        protected override void Awake(){
            base.Awake();
            jugador = GameObject.FindObjectOfType<Jugador>();
        }


        public void AccionMuerte()
        {
            if (jugador == null)
                return;
            jugador.AccionMuerte();
        }
        public void AccionRevivir()
        {
            if (jugador == null)
                return;
            jugador.AccionRevivir();
        }
        public void AccionDestruir()
        {
            if (jugador == null)
                return;
            jugador.AccionDestruir();
        }

        public void AccionSetMetadato(string comando)
        {

            if (jugador == null)
                return;
            jugador.AccionSetMetadato(comando);

        }
        public void AccionModMetadato(string comando)
        {

            if (jugador == null)
                return;
            jugador.AccionModMetadato(comando);

        }

        public void AccionSetGameplayMetadato(string comando)
        {
            if (jugador == null)
                return;
            jugador.AccionSetGameplayMetadato(comando);
        }
        public void AccionModGameplayMetadato(string comando)
        {
            if (jugador == null)
                return;
            jugador.AccionModGameplayMetadato(comando);
        }

        public void AccionSetEnableMovimiento(bool enable)
        {
            if (jugador == null)
                return;
            jugador.AccionSetEnableMovimiento(enable);
        }
        public void AccionSetEnableAtaque(bool enable)
        {
            if (jugador == null)
                return;
            jugador.AccionSetEnableAtaque(enable);
        }
        public void AccionSetEnableVitalidad(bool enable)
        {
            if (jugador == null)
                return;
            jugador.AccionSetEnableVitalidad(enable);
        }

        public void AccionSetAtaque(float ataque)
        {
            if (jugador == null)
                return;
            jugador.AccionSetAtaque(ataque);
        }
        public void AccionModAtaque(float ataque)
        {
            if (jugador == null)
                return;
            jugador.AccionModAtaque(ataque);
        }

        public void AccionSetVida(float vida)
        {
            if (jugador == null)
                return;
            jugador.AccionSetVida(vida);
        }
        public void AccionModVida(float vida)
        {
            if(jugador == null)
                return;
            jugador.AccionModVida(vida);
        }

        public void AccionSetVidaMax(float vida)
        {
            if (jugador == null)
                return;
            jugador.AccionSetVidaMax(vida);
        }
        public void AccionModVidaMax(float vida)
        {
            if (jugador == null)
                return;
            jugador.AccionModVidaMax(vida);
        }


        public void AccionSetReduccionDaño(float reduccion)
        {
            if (jugador == null)
                return;
            jugador.AccionSetReduccionDaño(reduccion);
        }
        public void AccionModVReduccionDaño(float reduccion)
        {
            if (jugador == null)
                return;
            jugador.AccionModVReduccionDaño(reduccion);
        }

        public void AccionSetHabilidadEnable(string comando){
            if (jugador == null)
                return;
            jugador.AccionSetHabilidadEnable(comando);
        }
        public void AccionToogleHabilidadEnable(string nombre){
            if (jugador == null)
                return;
            jugador.AccionToogleHabilidadEnable(nombre);
        }


    }
}