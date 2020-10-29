using UnityEngine;
using System.Collections.Generic;

namespace Aventuras{

    public class EnemigoTirador : Enemigo{

        [Header("IA - Tirador")]
        [SerializeField]
        private DisparadorProyectilBasico []disparadores = null;
        [Header("IA - Rastreo")]
        [SerializeField]
        private EnemigoEstrategia estrategia = EnemigoEstrategia.JUGADOR;
        [SerializeField]
        private Colision arearastreo = null;
        [SerializeField]
        private float radiorastreo = 10;
        [SerializeField]
        private float distanciadeseada = 5.0f;
        [Header("IA - Comportamiento")]
        [SerializeField]
        private bool regresarorigen = false;
        [SerializeField]
        private bool poseorigen = true;
        [SerializeField]
        private float distanciaminimaorigen = 0.25f;

        private List<Entidad> entidades = new List<Entidad>();
        private Entidad objetivo = null;
        private Vector3 origen = Vector3.zero;

        protected override void Awake(){
            base.Awake();
            origen = GetPosicion();
            origen.y = 0;
            arearastreo.AddColisionEvento(EventoColision);
        }
        protected override void Update(){
            base.Update();

            for (int i = 0; i < entidades.Count; i++)
                if (entidades[i] == null)
                    entidades.RemoveAt(i--);

            RastreoObjetivo();
            SeguimientoObjetivo();
            DisparoObjetivo();

        }

        public void RastreoObjetivo(){
            float menor = 100000;
            float aux = 0;
            int index = -1;
            switch (estrategia){
                case EnemigoEstrategia.INSISTENTE:

                    if (objetivo == null && entidades.Count > 0)
                        objetivo = entidades[0];                    

                    break;
                case EnemigoEstrategia.JUGADOR:

                    if (objetivo == null)
                        for (int i = 0; i < entidades.Count; i++)
                            if ((entidades[i] as Jugador) != null){
                                objetivo = entidades[i];          
                                break;
                            }

                    break;
                case EnemigoEstrategia.MASCERCANO:
                    for (int i = 0; i < entidades.Count; i++)
                    {
                        aux = GetDistanciaPlano(entidades[i]).magnitude;
                        if (aux < menor)
                        {
                            menor = aux;
                            index = i;
                        }
                    }
                    break;
                case EnemigoEstrategia.MENORVIDA:
                    for (int i = 0; i < entidades.Count; i++)
                    {
                        if (entidades[i].GetModuloVitalidad() == null)
                            continue;

                        aux = entidades[i].GetModuloVitalidad().GetPerfilVitalidad().GetVida();
                        if (aux < menor)
                        {
                            menor = aux;
                            index = i;
                        }
                    }
                    break;    
            }

            if (index != -1)
                objetivo = entidades[index];


        }
        public void SeguimientoObjetivo(){
            if (!GetModuloMovimiento().IsEnable())
                return;

            Vector3 direccion = Vector3.zero;

            if (objetivo == null)
            {

                if (regresarorigen)
                {
                    direccion = origen - GetPosicion();  
                    direccion.y = 0;

                    if (direccion.magnitude < distanciaminimaorigen)
                    {
                        if (poseorigen)
                            GetModuloMovimiento().SetDireccion(GetDireccionOriginal());
                        GetModuloMovimiento().Detener();
                    }
                    else
                        GetModuloMovimiento().SetDireccion(direccion.normalized);
                }
                else
                    GetModuloMovimiento().Detener();            

            }
            else{

                direccion = objetivo.GetDistanciaPlano(this);
                if (GetDistanciaOrigen(objetivo) > radiorastreo)
                {
                    GetModuloMovimiento().Detener();
                    GetModuloMovimiento().SetDireccion(direccion);
                }
                else if (direccion.magnitude < distanciadeseada)
                {
                    GetModuloMovimiento().Detener();
                    GetModuloMovimiento().SetDireccion(direccion);
                }
                else{
                    direccion = direccion.normalized;               
                    GetModuloMovimiento().SetDireccion(direccion);
                }


            }



        }    
        public void DisparoObjetivo(){
            if (objetivo == null || disparadores == null)
                return;
            for(int i=0;i<disparadores.Length;i++)
                if (disparadores[i].IsActivo())
                    disparadores[i].Disparar();
            

        }

        public override void Generacion(){

        }

        public float GetDistanciaOrigen(Entidad entidad){
            Vector3 distancia = origen - entidad.GetPosicion();
            distancia.y = 0;
            return distancia.magnitude;
        }           

        private void EventoColision(ColisionInformacion info){

            Entidad entidad = info.GetEntidadImpacto();

            if (entidad != null){
                if (entidad.GetModuloVitalidad() == null)
                    return;
                if (!entidad.GetModuloVitalidad().GetPerfilVitalidad().IsColision(info.GetColisionImpacto()))
                    return;
                if (entidad.GetTipo() != EntidadTipo.ALIADO)
                    return;
                if (info.IsColisionEstado(ColisionEstado.ENTER)){
                    if (!entidades.Contains(entidad))
                        entidades.Add(entidad);
                }
                else if (info.IsColisionEstado(ColisionEstado.EXIT)){
                    if (entidades.Contains(entidad)){
                        if (entidad == objetivo)
                            objetivo = null;
                        entidades.Remove(entidad);
                    }
                }
            }

        }            

    }


}