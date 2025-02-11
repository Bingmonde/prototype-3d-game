
using UnityEngine;

public class GrenadeLogic : MonoBehaviour
{

    [SerializeField]
    GameObject grenadeTrigger;

    [SerializeField]
    GameObject effectExplosion;

    [SerializeField]
    AudioClip exlposionSound;
    AudioSource exlposionSoundSource;
    

    bool isFirstInstanceGrenadeTrigger = true;

    private void Start()
    {

        exlposionSoundSource = GameObject.Find("GrenadeExplosionAudio").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // instancier le grenadeTrigger seulement le premier grenade trigger
        if (isFirstInstanceGrenadeTrigger) {
            Instantiate(grenadeTrigger, collision.GetContact(0).point, Quaternion.identity);

            // appliquer l'effet d'exploision
            GameObject explosion = Instantiate(effectExplosion, collision.GetContact(0).point, Quaternion.identity);

            exlposionSoundSource.PlayOneShot(exlposionSound);
            exlposionSoundSource.volume = AudioManager.instance.GetSoundVolumne();
            isFirstInstanceGrenadeTrigger =false;
        }
        // détruire la grenade
        Destroy(gameObject);

    }

}
