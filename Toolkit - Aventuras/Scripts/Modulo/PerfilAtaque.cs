using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class PerfilAtaque : MonoBehaviour{

        [SerializeField]
        private bool   enable = true; 
        [SerializeField]
        private string nombre = "Desconocido";
        [Range(0,1)]
        [SerializeField]
        private float multiplicador = 1.0f;

        private Colision []colisiones = null;

        public void Awake(){            
            colisiones = GetComponentsInChildren<Colision>();
            SetEnable(enable);
        }       

        public void SetEvento(ColisionEvento evento){
            if(colisiones!=null)
                for(int i=0;i<colisiones.Length;i++)
                    colisiones[i].AddColisionEvento(evento);
        }

        public void SetMultiplicador(float multiplicador){
            this.multiplicador = multiplicador;
        }
        public void ModMultiplicador(float multiplicador){
            this.multiplicador += multiplicador;
        }

        public void SetEnable(bool enable){
            this.enable = enable;
            if (colisiones != null)
                for (int i = 0; i < colisiones.Length; i++)
                    colisiones[i].GetComponent<Collider>().enabled = enable;
        }
        private void ToogleEnable(){
            this.enable = !this.enable;
        }

        public float          GetMultiplicador(){
            return multiplicador;
        }
       
        public string   GetNombre(){
            return nombre;
        }
                   
        public bool     IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colision == colisiones[i])
                    return true;
            return false;

        }
        public bool     IsEnable(){
            return enable;
        }
            
        public void AccionSetEnable(bool enable){
            SetEnable(enable);
        }
        public void AccionToogleEnable(){
            ToogleEnable();
        }

        public void AccionSetMultiplicador(float multiplicador){
            this.SetMultiplicador(multiplicador);
        }
        public void AccionModMultiplicador(float multiplicador){
            this.ModMultiplicador(multiplicador);
        }


    }

}

