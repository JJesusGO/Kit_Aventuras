using UnityEngine;
using System.Collections;

namespace Aventuras{



    [System.Serializable]
    public class JugadorHabilidad{
    
        [SerializeField]
        private string nombre = "";
        [SerializeField]
        private bool enable = true;
        [SerializeField]
        private TeclaEvento []teclas = null;

        public void Update(){
            if (!enable)
                return;
            if(teclas!=null)
                for(int i=0;i<teclas.Length;i++)
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

    public class Jugador : Aliado{

        [SerializeField]
        private ModuloMovimiento movimiento = new ModuloMovimiento();
        [SerializeField]
        private ModuloVitalidad vitalidad = new ModuloVitalidad();
        [SerializeField]
        private ModuloAtaque    ataque = new ModuloAtaque();
        [Header("Configuración")]
        [SerializeField]
        private bool revivirautomatico = true;
        [Header("Movimiento")]    
        [SerializeField]
        private Eje ejex = new Eje("EjeX");
        [SerializeField]
        private Eje ejey = new Eje("EjeZ");

        [Header("Habilidades")]
        [SerializeField]
        private JugadorHabilidad []habilidades = null;

        private Vector3 direccion = Vector3.zero;

        protected override void Awake(){
            base.Awake();
            AddModulo(movimiento);
            AddModulo(ataque);
            AddModulo(vitalidad);
        }
        protected override void Update(){
            base.Update();

            Vector3 direccion = new Vector3(-ejex.GetValor(), 0,ejey.GetValor());
            GetModuloMovimiento().SetDireccion(direccion);

            if (direccion == Vector3.zero)
                GetModuloMovimiento().Detener();     

            if (habilidades != null)
                for (int i = 0; i < habilidades.Length; i++)
                    habilidades[i].Update();

            if (IsMuerto() && revivirautomatico)
                Revivir();            

        }

        public override void Revivir(){
            base.Revivir();
            if (!IsMuerto())
                return;
            MapaSpawn spawn = Mapa.GetInstancia().GetSpawn();
            if (spawn != null)
                SetPosicion(spawn.GetPosicion());
        }
        public override void Generacion(){
            
        }
           
        private void SetHabilidadEnable(string nombre,bool enable){
            if (habilidades != null)
                for (int i = 0; i < habilidades.Length; i++)
                    if (habilidades[i].IsNombre(nombre))
                        habilidades[i].SetEnable(enable);
        }
        private void ToogleHabilidadEnable(string nombre){
            if (habilidades != null)
                for (int i = 0; i < habilidades.Length; i++)
                    if (habilidades[i].IsNombre(nombre))
                        habilidades[i].ToogleEnable();
        }
            
        public override ModuloMovimiento GetModuloMovimiento(){
            return movimiento;
        }
        public override ModuloAtaque     GetModuloAtaque(){
            return ataque;
        }
        public override ModuloVitalidad  GetModuloVitalidad(){
            return vitalidad;
        }

        public void AccionSetHabilidadEnable(string comando){
            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                SetHabilidadEnable(data[0],bool.Parse(data[1]));   
        }
        public void AccionToogleHabilidadEnable(string nombre){
            ToogleHabilidadEnable(nombre);
        }

    }

}

