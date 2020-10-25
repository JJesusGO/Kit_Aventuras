using UnityEngine;
using System.Collections;

namespace Aventuras{

    [System.Serializable]
    public class Eje{

        [SerializeField]
        private string nombre = "";
        [SerializeField]
        private string positivo = "";
        [SerializeField]
        private string negativo = "";

        public Eje(string nombre){
            this.nombre = nombre;
            this.positivo = "";
            this.negativo = "";
        }

        private bool IsClick(bool tipo){
            if(tipo)
                return Input.GetKey(positivo);
            return Input.GetKey(negativo);
        }
        private bool IsClickDown(bool tipo){
            if(tipo)
                return Input.GetKeyDown(positivo);
            return Input.GetKeyDown(negativo);
        }
        private bool IsClickUp(bool tipo){
            if(tipo)
                return Input.GetKeyUp(positivo);
            return Input.GetKeyUp(negativo);
        }
            
        public int  GetValor(){
            if (IsClick(true))
                return 1;
            if (IsClick(false))
                return -1;
            return 0;
        }
       
        public bool IsClick(){
            return IsClick(true) || IsClick(false);
        }
        public bool IsClickDown(){
            return IsClickDown(true) || IsClickDown(false);
        }
        public bool IsClickUp(){
            return IsClickUp(true) || IsClickUp(false);
        }


    }

}
