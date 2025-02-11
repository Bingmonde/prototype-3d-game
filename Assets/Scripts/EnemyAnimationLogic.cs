using UnityEngine;

public class EnemyAnimationLogic : MonoBehaviour
{
    private Animator animator;
    private bool isInsideDistance = false;
    private bool isUnderView = false;

    [SerializeField]
    private EnemyLogic enemyLogic;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Activer le animation seulement la première fois que player entre dans une zone de détection
    public void ActiveAttackAnimation() {
        
        if ((isUnderView && !isInsideDistance) || (isInsideDistance && !isUnderView)) {
            animator.SetTrigger("Attack");
            enemyLogic.SetIsAttacking(true);
            enemyLogic.StartShootingPlayer();
            Debug.Log("start attacking player");
            enemyLogic.EnablePatrolScript(false);
        }
    }
    // Désactiver l'animation lorsque player sorte de toutes les 2 zone de détection
    public void StopAttackAnimation() {
        
        if (!isInsideDistance && !isUnderView) {
            animator.SetTrigger("Attack");
            enemyLogic.SetIsAttacking(false);
            enemyLogic.StopShootingPlayer();
            enemyLogic.EnablePatrolScript(true);
        }
    }

    public void SetIsInsideDistance(bool value) {
        isInsideDistance = value;
    }

    public void SetUnderView(bool value) {
        isUnderView = value;
    }


}
