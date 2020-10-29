using UnityEngine;
using System.Collections;
using TMPro;

namespace Aventuras{

    public enum UINumeroTipo{
        FLOAT,INT
    }
        
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UINumero : MonoBehaviour{

        [Header("Metadato")]
        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private CondicionTipo variable = CondicionTipo.VIDA;
        [SerializeField]
        private string metanombre = "";
        [Header("Condiguración")]
        [SerializeField]
        private UINumeroTipo tipo = UINumeroTipo.FLOAT;
        [SerializeField]
        private string formato = "{0:0.00}";
        [Tooltip("Si la velocidad es menor o igual a cero, se actualiza inmediatamente")]
        [SerializeField]
        private float  velocidad = 0.0f;
       
        private float numero = 0, 
                    uinumero = 0;
        private TextMeshProUGUI uitexto = null;

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();
            SetUINumero();
        }
        private void Update(){
                   
            SetNumero();
            if(numero!=uinumero){
                if(velocidad!=0.0f)
                    uinumero = Mathf.MoveTowards(uinumero,numero,velocidad);
                Actualizar();
            }


        }

        private void Actualizar(){
            if (tipo == UINumeroTipo.INT)
                uitexto.text = string.Format(formato, (int)uinumero);
            else
                uitexto.text = string.Format(formato, uinumero);
        }
            
        private void SetNumero(){     

            float valor = 0;

            if (entidad != null)
            {
                switch (variable)
                {
                    case CondicionTipo.ATAQUE:
                        if (entidad.GetModuloAtaque() != null)
                            valor = entidad.GetModuloAtaque().GetAtaqueBase();                        
                        break;
                    case CondicionTipo.VIDA:
                        if (entidad.GetModuloVitalidad() != null)
                            valor = entidad.GetModuloVitalidad().GetPerfilVitalidad().GetVida(true);                                                
                        break;
                    case CondicionTipo.METADATO:
                            valor = float.Parse(entidad.GetMetadato(metanombre));
                        break;
                }
            }
            else if(variable == CondicionTipo.METADATO)
                valor = float.Parse(ManagerGameplay.GetInstancia().GetMetadato(metanombre));

            numero = valor;
            if (velocidad <= 0){
                uinumero = numero;
                Actualizar();
            }
        }
        private void SetUINumero(){
            SetNumero();  
            uinumero = numero;
            Actualizar();
        }

    }

}