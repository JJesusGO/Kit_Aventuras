using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Aventuras{


    [System.Serializable]
    public class PerfilEntrada{        

        [SerializeField]
        private string nombre = "Desconocido";
        [SerializeField]
        private bool enable = true;
        [SerializeField]
        private TeclaEvento []teclas = null;

        public void Update(){
            if (!enable)
                return;
            if (teclas != null)
                for (int i = 0; i < teclas.Length; i++)
                    teclas[i].Update();
        }

        public void SetEnable(bool enable){
            this.enable = enable;
        }
        public void ToogleEnable(){
            this.enable = !this.enable;
        }

        public bool IsNombre(string nombre){
            return this.nombre == nombre;
        }

    }
    public class ExtensionEntrada : Extension{

        [SerializeField]
        private PerfilEntrada []perfiles = null;
       
        private void Update(){
            if (perfiles != null)
                for (int i = 0; i < perfiles.Length; i++)
                    perfiles[i].Update();
        }

        private void SetEnable(string nombre,bool enable){
            if (perfiles != null)
                for (int i = 0; i < perfiles.Length; i++)
                    if (perfiles[i].IsNombre(nombre))
                        perfiles[i].SetEnable(enable);
        }
        private void ToogleEnable(string nombre){
            if (perfiles != null)
                for (int i = 0; i < perfiles.Length; i++)
                    if (perfiles[i].IsNombre(nombre))
                        perfiles[i].ToogleEnable();
        }
            
        public void AccionSetEnable(string comando){
            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                SetEnable(data[0],bool.Parse(data[1]));   
        }
        public void AccionToogleEnable(string nombre){
            ToogleEnable(nombre);
        }
      
    }

}

