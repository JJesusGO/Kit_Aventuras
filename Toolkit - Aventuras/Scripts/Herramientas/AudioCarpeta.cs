using UnityEngine;
using System.Collections;

namespace Aventuras{

    public class AudioCarpeta : MonoBehaviour{

        public void EventoStopAudios(){
            Audio []audios = GetComponentsInChildren<Audio>();
            if (audios != null)
                for (int i = 0; i < audios.Length; i++)
                    audios[i].Stop();
        }
       
    }

}
