using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Aventuras{

    [System.Serializable]
    public struct AudioPerfil{
        [SerializeField]
        public string nombre;
        [SerializeField]
        public Transform carpeta;
        [SerializeField]
        public Audio  prefab;
    }
    [System.Serializable]
    public struct ClipPerfil{
        [SerializeField]
        public string nombre;
        [SerializeField]
        public AudioClip audio;
    }

    public class ManagerAplicacion : MonoBehaviour{

        [Header("Audio")]
        [SerializeField]
        private AudioPerfil []perfiles = null;
        [SerializeField]
        private ClipPerfil[] clips = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventoinicio = null;

        public static ManagerAplicacion instancia;

        private void Awake(){
            instancia = this;        
        
        }
        private void Start(){
            eventoinicio.Invoke();
        }

            
        public void PlayAudio(string perfil, AudioClip clip, Vector3 posicion)
        {
            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre) { 
                    PlayAudio(i, clip, posicion);
                    break;
                }   
        }
        public void PlayAudio(int perfil,AudioClip clip, Vector3 posicion){
            Audio audio = perfiles[perfil].prefab;
            audio.Create(clip,perfiles[perfil].carpeta,posicion);
        }

        public static ManagerAplicacion GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ManagerAplicacion>();
            return instancia;
        }


        public void AccionLog(string mensaje){
            Debug.Log(mensaje);
        }
        public void AccionPlayAudio(string codigo){

            string[] data = codigo.Split('_');

            if (data.Length < 2)
                return;

            string perfil = data[0],
            clip = data[1];


            AudioClip sonido = null;
            for (int i = 0; i < clips.Length; i++)
                if (clip == clips[i].nombre) {
                    sonido = clips[i].audio;
                    break;
                }            

            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre)
                {
                    PlayAudio(i, sonido, transform.position);
                    break;
                }
        }

        public void AccionCargarEscena(int escena){
            SceneManager.LoadScene(escena);
        }
        public void AccionCargarEscena(string escena){
            SceneManager.LoadScene(escena);
        }


    }

}