using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    Transform grenadeCenter;

    // Start is called before the first frame update
    void Start()
    {
        grenadeCenter = transform;
        Destroy(gameObject.transform.parent.gameObject, 0.1f);
    }



    private void OnTriggerEnter(Collider other)  
    {
        //Debug.Log(other.name);


        if (other.name.Equals("EnemyCollider") || other.name.Equals("Player")) {
            Debug.Log("EnemyCollider detected");
            float distance = Vector3.Distance(grenadeCenter.position, other.transform.position);
            Debug.Log("distance" + distance);
            float damage = - 20 * distance + 200;
            if (damage < 0) {
                damage = 0;
            }
            if (damage > 200)
            {
                damage = 200;
            }

            if (other.name.Equals("EnemyCollider")) {
                EnemyLogic enemyLogic = other.GetComponentInParent<EnemyLogic>();
                Debug.Log(damage);
                enemyLogic.IsHitOperation(damage);

            }
            if (other.tag.Equals("Player"))
            {
                PlayerLogic playerLogic = other.GetComponent<PlayerLogic>();
                playerLogic.SetPlayerLifePoint(damage);
            }


        }

    }

    




}
