using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Aventuras{

    public class ExtensionTemporizador : Extension{

        [SerializeField]
        private bool inicioautomatico = true;
        [Header("Configuraciones")]
        [SerializeField]
        private float tiempo = 2;
        [SerializeField]
        private float iteraciones = 1;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent evento = new UnityEvent();

        private Temporizador temporizador = null;
        private int ciclos = 0;

        protected override void Awake(){
            base.Awake();
            temporizador = new Temporizador(tiempo);
            ciclos = 0;
        }
        private void Start(){
            if (inicioautomatico)
                temporizador.Start();
        }

        private void Update(){
            temporizador.Update();
            if (temporizador.IsActivo()){                                
                if (iteraciones <= 0){
                    ciclos++;
                    evento.Invoke();
                    temporizador.Start();
                }
                else if(ciclos < iteraciones){
                    ciclos++;
                    evento.Invoke();
                    temporizador.Start();
                }
            }
        }

        public void AccionReiniciar(){
            ciclos = 0;
            temporizador.Start();
        }

       
    }

}
