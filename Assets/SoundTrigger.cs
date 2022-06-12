using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public string id = "birds";

    // bird ambient forest entrance

    // water sound lake entry
    // stone steps on bridge
    public AudioSource ambientSource;
    public AudioClip birdAmbient;
    public AudioClip waterAmbient;
    public AudioClip mountainAmbient;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(id == "stonesounds")
            {
                FindObjectOfType<Movement>().playStoneSounds = true;
            }
            else if(id == "birds")
            {
                ambientSource.loop = true;
                ambientSource.clip = birdAmbient;
                ambientSource.volume = 0.75f;
                ambientSource.Play();
            }
            else if(id == "water")
            {
                ambientSource.loop = true;
                ambientSource.clip = waterAmbient;
                ambientSource.volume = 0.6f;
                ambientSource.Play();
            }
            else if(id == "mountain")
            {
                ambientSource.loop = true;
                ambientSource.clip = mountainAmbient;
                ambientSource.volume = 0.4f;
                ambientSource.Play();
            }
        }
    }
}
