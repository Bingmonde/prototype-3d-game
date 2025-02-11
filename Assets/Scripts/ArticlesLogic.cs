
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ArticlesLogic : MonoBehaviour
{
    HudLogic hudLogic;
    int numBulletsDeadEnemy = 10;

    [SerializeField]
    AudioClip objectFoundSound;

    AudioSource collectionAudioSource;
    private void Start()
    {
        hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
        collectionAudioSource = GameObject.Find("ArticleCollectionAudio").GetComponent<AudioSource>();
        collectionAudioSource.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //Debug.Log(gameObject.name);
        if (other.name.Equals("Player")){
           
            PlayerLogic playerLogic = other.gameObject.GetComponent<PlayerLogic>();
            if (playerLogic != null)
            {
                if ( collectionAudioSource != null)
                {
                    collectionAudioSource.enabled = true;
                    collectionAudioSource.Play();
                    collectionAudioSource.volume = AudioManager.instance.GetSoundVolumne();

                }


                // collecter des armes
                switch (gameObject.name) {
                    case "GrenadeCollectTrigger":
                        playerLogic.CollectGrenade();
                        break;
                    case "BulletCollectTrigger":
                        playerLogic.CollectBullets(numBulletsDeadEnemy);                       
                        break;
                    case "SubmachcineCollectionTrigger":
                        playerLogic.CollectWeapons(Guns.Submachine);
                        break;
                    case "AssaultCollectionTrigger":
                        playerLogic.CollectWeapons(Guns.Assault);
                        break;
                    default: 
                        // collecter des documents
                        string article = transform.parent.gameObject.name.Trim();
                        Debug.Log(article);
                        playerLogic.CollectObject(article);
                        hudLogic.ShowArticleImageAndInfo(transform.parent.gameObject.name.Trim());
                        break;
                }
                Destroy(transform.parent.gameObject);


            }
            
        }
/*        if (other.name.Equals("ExplosionTrigger")) {
            Debug.Log(gameObject.name + ": grenade");
        }*/
    }
}
