using UnityEngine;
using System.Collections;
using TMPro;

namespace Aventuras{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIMetaDato : MonoBehaviour{

        [SerializeField]
        private string nombre = "";
        [SerializeField]
        private string uiformato = "{0}";

        private TextMeshProUGUI uitexto = null;

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();        
        }
              
      
    }

}

