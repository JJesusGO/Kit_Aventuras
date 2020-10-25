using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Aventuras{

    public enum GameplayEstado{
        JUGANDO,PERDER, GANAR
    }

    public class ManagerGameplay : MonoBehaviour {


        private static ManagerGameplay instancia = null;
        private GameplayEstado estado = GameplayEstado.JUGANDO;

        protected virtual void Awake() {
            instancia = this;
        }
        protected virtual void Start() {
            SetEstado(GameplayEstado.JUGANDO, true);
        }

        protected void Jugando() {
        }
        protected void Perder() {

        }
        protected void Ganar() {

        }

        public void ReiniciarNivel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetEstado(GameplayEstado estado, bool forzar = false) {
            if (this.estado == estado && !forzar)
                return;
            this.estado = estado;
            switch (this.estado) {
                case GameplayEstado.JUGANDO:
                    Jugando();
                    break;
                case GameplayEstado.GANAR:
                    Ganar();
                    break;
                case GameplayEstado.PERDER:
                    Perder();
                    break;
            }
        }

        public bool IsEstado(GameplayEstado estado) {
            return this.estado == estado;
        }


        public void AccionGanar() {
            if (IsEstado(GameplayEstado.JUGANDO))
                SetEstado(GameplayEstado.GANAR);
        }
        public void AccionPerder() {
            if (IsEstado(GameplayEstado.JUGANDO))
                SetEstado(GameplayEstado.PERDER);
        }

        public static ManagerGameplay GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ManagerGameplay>();
            return instancia;
        }
            

    }


}
