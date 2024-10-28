using UnityEngine;

namespace Project.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip failed;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip lose;
        [SerializeField] private AudioClip win;
        [SerializeField] private AudioClip pop;


        public void PlayClick() => audioSource.PlayOneShot(click);
        public void PlayLose() => audioSource.PlayOneShot(lose);
        public void PlayWin() => audioSource.PlayOneShot(win);
        public void PlayFailed() => PlayRandomPitch(failed);
        public void PlayPop() => PlayRandomPitch(pop);


        private void PlayRandomPitch(AudioClip audioClip)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioClip);
            audioSource.pitch = 1f;
        }
    }
}