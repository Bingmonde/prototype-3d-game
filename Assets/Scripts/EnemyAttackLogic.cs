using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackLogic : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    EnemyLogic enemyLogic;

    [SerializeField]
    private EnemyAnimationLogic enemyAnimationLogic;

    [SerializeField]
    private float detectionDiffiLevel = 1.3f;

    void Start()
    {
        enemyLogic = transform.parent.GetComponent<EnemyLogic>();
        if (enemyLogic == null ) {
            Debug.Log("enemy logic est abesent!");
        }

        // La portée de détection des ennemis augmente de 30% 
        if (LevelManager.instance.currentLevel == GameLevels.Difficle) {
            this.transform.localScale = this.transform.localScale * detectionDiffiLevel;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player")) {

            if (transform.gameObject.name.Equals("ViewTrigger"))
            {
                enemyAnimationLogic.SetUnderView(true);
                enemyAnimationLogic.ActiveAttackAnimation();
            }
            if (transform.gameObject.name.Equals("DistanceTrigger"))
            {
                enemyAnimationLogic.SetIsInsideDistance(true);
                enemyAnimationLogic.ActiveAttackAnimation();
            }
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (!GameManager.instance.IsGameRunning()) {
            return;
        }

        if (other.name.Equals("Player"))
        {
            // Suivre le player
            PlayerLogic playerLogic = other.gameObject.GetComponent<PlayerLogic>();
            if (playerLogic != null && enemyLogic != null)
            {
                Vector3 playerPosition = playerLogic.GetPlayerPosition();
               enemyLogic.SetAttackPosition(playerPosition);
            }

            // Toujour tourne vers le player
            transform.parent.transform.forward = new Vector3(-other.transform.forward.x, 0, -other.transform.forward.z);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player")) {
            if (transform.gameObject.name.Equals("ViewTrigger"))
            {
                enemyAnimationLogic.SetUnderView(false);
                enemyAnimationLogic.StopAttackAnimation();
            }
            if (transform.gameObject.name.Equals("DistanceTrigger"))
            {
                enemyAnimationLogic.SetIsInsideDistance(false);
                enemyAnimationLogic.StopAttackAnimation();
            }
            enemyLogic.BackToLastPosition();
        }
        
    }




}
