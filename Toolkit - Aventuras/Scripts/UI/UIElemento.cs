using UnityEngine;
using System.Collections;

namespace Aventuras{

    [ExecuteInEditMode]
    public class UIElemento : MonoBehaviour{

        [SerializeField]
        private bool enable = true;
        [SerializeField]
        private Transform elemento = null;

        private void Update(){

            if (enable != elemento.gameObject.activeSelf)
                elemento.gameObject.SetActive(enable);            

        }

        public void SetEnable(bool enable,bool forzar = false){

            if (this.enable == enable && !forzar)
                return;
            this.enable = enable;

        }
            
        public bool IsEnable(){
            return enable;
        }

        public void AccionSetEnable(bool enable){
            this.enable = enable;
        }

    }


}

