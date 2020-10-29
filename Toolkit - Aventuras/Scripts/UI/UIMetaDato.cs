using UnityEngine;
using System.Collections;
using TMPro;

namespace Aventuras{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIMetaDato : MonoBehaviour{

        [Header("Configuración")]
        [SerializeField]
        private string metanombre = "";
        [SerializeField]
        private string formato = "{0}";
        [Header("Entidad")]
        private Entidad entidad = null;

        private TextMeshProUGUI uitexto = null;

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();        
        }

        private void Update(){
            if (entidad == null){
                string valor = ManagerGameplay.GetInstancia().GetMetadato(metanombre);
                uitexto.text = string.Format(formato, valor);
            }
            else{
                string valor = entidad.GetMetadato(metanombre);
                uitexto.text = string.Format(formato, valor);
            }
                
        }
              
      
    }

}

