using UnityEngine;

namespace Project.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip deselect;
        [SerializeField] private AudioClip win;
        [SerializeField] private AudioClip lose;
        [SerializeField] private AudioClip woosh;
        [SerializeField] private AudioClip pop;
        [SerializeField] private AudioClip failed;


        public void PlayClick() => audioSource.PlayOneShot(click);
        public void PlayDeselect() => audioSource.PlayOneShot(deselect);
        public void PlayWin() => audioSource.PlayOneShot(win);
        public void PlayLose() => audioSource.PlayOneShot(lose);
        public void PlayWoosh() => PlayRandomPitch(woosh);
        public void PlayPop() => PlayRandomPitch(pop);

        public void PlayFailed() => PlayRandomPitch(failed);

        private void PlayRandomPitch(AudioClip audioClip)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioClip);
            audioSource.pitch = 1f;
        }
    }
}