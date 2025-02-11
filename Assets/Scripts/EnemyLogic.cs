using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyLogic : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private float _lifePoint = 100f;
    private float _originalLifePoint = 100f;
    bool isDead = false;
    [SerializeField]
    GameObject lifePointsBar;
    [SerializeField]
    GameObject originalLifeBar;
    public TextMeshPro lifePointText;
    [SerializeField]
    GameObject bulletsPrefab;

    private Rigidbody _rigidbody;


    private PlayerLogic playerLogic;
    [SerializeField]
    GameObject effect_shot;
    [SerializeField]
    GameObject firePoint;
    
    private Vector3 originalPosition;

    private int hitChance = 3;
    private int hitChanceDiffiLevel = 2; // niveau difficile
    private float increasedSpeedPercentageDiffiLevel = 0.25f;

    private float hitDamage = 25f;
    private int bulletMagazine = 10;
    private int bulletMagazineOriginal = 10;
    private float loadingBulletTime = 3f;
    private float shootInterval = 2f;
    private bool isLoading = false;
    private bool isAttacking = false;

    private bool isPatrolEnemy;
    EnemyPatrolLogic enemyPatrolLogic;
    HudLogic hudLogic;

    // effect sonore
    [SerializeField]
    AudioClip enemyShootSound;
    [SerializeField]
    AudioClip enemyGetShotSound;
    [SerializeField]
    AudioClip agonySound;
    [SerializeField]
    AudioClip bulletShellSound;
    [SerializeField]
    AudioClip rechargeSound;
    AudioSource[] audioSources;
  
    

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody != null) {
            _rigidbody.isKinematic = true;
        }
        
        agent = GetComponent<NavMeshAgent>();
        originalPosition = agent.transform.position;
        playerLogic = GameObject.Find("Player").GetComponent<PlayerLogic>();
        hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
        //firePoint = GameObject.Find("EnemyGunFirePoint");
        if (playerLogic == null)
        {
            Debug.Log("player logic est abesent!");
        }

        enemyPatrolLogic = GetComponent<EnemyPatrolLogic>();
        // identifier le type d'ennemi
        if (agent.tag.Equals("Patrol"))
        {
            isPatrolEnemy = true;
        }
        else {
            isPatrolEnemy= false;
            enemyPatrolLogic.enabled = false;
        }

        audioSources = GetComponentsInChildren<AudioSource>();

        if (LevelManager.instance.currentLevel == GameLevels.Difficle) {
            agent.speed *= 1 + increasedSpeedPercentageDiffiLevel;
            Debug.Log( "Agent speed"+ agent.speed);
        }

        lifePointText = GetComponentInChildren<TextMeshPro>();
        lifePointText.text = _lifePoint.ToString();
    }

    private void Update()
    {
        if (GameManager.instance.IsGameRunning())
        {
            agent.enabled = true;
            
        }
        else {
            agent.enabled = false;
        }
    }

    public float GetEnemyLifePoint() {
        return _lifePoint;
    }

    public void IsHitOperation(float damagePoint) {
        
        _lifePoint = _lifePoint - damagePoint < 0 ? 0 : _lifePoint - damagePoint;
        lifePointText.text = _lifePoint.ToString();
        // gestion de la bar de vie 
        float lifePercentage = _lifePoint / _originalLifePoint;
        lifePointsBar.transform.localScale = new Vector3(lifePointsBar.transform.localScale.x * lifePercentage, lifePointsBar.transform.localScale.y, lifePointsBar.transform.localScale.z);

        if (_lifePoint <= 0 && !isDead)
        {
            agent.enabled = false;
            EnemyDeadOperation();
        }
        else {
            audioSources[(int)EnemyAudioManagement.GetShot].PlayOneShot(enemyGetShotSound);
            audioSources[(int)EnemyAudioManagement.GetShot].volume = AudioManager.instance.GetSoundVolumne();
            agent.enabled = true;
        }
    }

    private void EnemyDeadOperation() {
        audioSources[(int)EnemyAudioManagement.GetShot].PlayOneShot(agonySound);
        audioSources[(int)EnemyAudioManagement.GetShot].volume = AudioManager.instance.GetSoundVolumne();
        _rigidbody.isKinematic = false;
        if (isPatrolEnemy) {
            enemyPatrolLogic.enabled=false;
        }

        // laisser 10 bullets à sa mort
        Instantiate(bulletsPrefab, agent.transform.position, Quaternion.identity);
        audioSources[(int)EnemyAudioManagement.BulletShell].PlayOneShot(bulletShellSound);
        audioSources[(int)EnemyAudioManagement.BulletShell].volume = AudioManager.instance.GetSoundVolumne();

        Destroy(gameObject, 1f);
    }

    public void SetAttackPosition(Vector3 destination)
    {
        if (agent.enabled) {
            agent.SetDestination(destination);
        }
    }

    public void BackToLastPosition() {
        if (agent.enabled ) {
            agent.SetDestination(originalPosition);
        }
    }

    public void EnableAgent(bool value) {
        agent.enabled = value;
    }

    public void SetIsAttacking(bool attack) {
        isAttacking = attack;
    }

    // tirer sur player par 2 seconde
    public void StartShootingPlayer()
    {
        if (!GameManager.instance.IsGameRunning()) { return; }
        if (isAttacking) {
            StartCoroutine(ShootTimerCoroutine());
            ShootPlayerOperation();
        }
    }
    public void StopShootingPlayer()
    {
        StopCoroutine(ShootTimerCoroutine());
;
    }
    private IEnumerator ShootTimerCoroutine()
    {
        while (!isLoading && isAttacking && GameManager.instance.IsGameRunning()) {
            yield return new WaitForSeconds(shootInterval);
            ShootPlayerOperation();
        }
    }
    private void ShootPlayerOperation() {
        GameObject shootingFire = Instantiate(effect_shot, firePoint.transform.position, Quaternion.identity);
        audioSources[(int)EnemyAudioManagement.Shoot].PlayOneShot(enemyShootSound);
        audioSources[(int)EnemyAudioManagement.Shoot].volume = AudioManager.instance.GetSoundVolumne();

        if (playerLogic.GetPlayerLifePoint() <= 0) {
            return;
        }

        if (bulletMagazine > 0 && !isLoading)
        {
            bulletMagazine--;
            float damage = PlayerDamage();
            playerLogic.SetPlayerLifePoint(damage);
        }
        // recharger des balles
        else {
            isLoading = true;
            StopShootingPlayer();
            hudLogic.ShowCustomedInfo("Ennemi est en train de recharger !", loadingBulletTime);
            StartCoroutine(LoadingBulletTimerCoroutine());
        }
    }

    private float PlayerDamage()
    {
        int aNumber;
        if (LevelManager.instance.currentLevel == GameLevels.Normal) {
            aNumber = Random.Range(0, hitChance);
            //Debug.Log("hit change: " + hitChance);
        }
        else { aNumber = Random.Range(0, hitChanceDiffiLevel);
           // Debug.Log("hit change: " + hitChanceDiffiLevel);
        }

        if (aNumber == 0)
        {
            return hitDamage;
        }
        else { return 0; }
    }


    // Gestion de la recharge des balles
    private IEnumerator LoadingBulletTimerCoroutine()
    {
        audioSources[(int)EnemyAudioManagement.Recharge].PlayOneShot(rechargeSound);
        audioSources[(int)EnemyAudioManagement.Recharge].volume = AudioManager.instance.GetSoundVolumne();
        yield return new WaitForSeconds(loadingBulletTime);
        bulletMagazine = bulletMagazineOriginal;
        isLoading = false;
        StopCoroutine(LoadingBulletTimerCoroutine());
        StartShootingPlayer();
    }

    // ennemi en patrouille
    public void EnablePatrolScript(bool value) {
        if (isPatrolEnemy) {
            enemyPatrolLogic.enabled = value;
        }
        
    }

}

public enum EnemyAudioManagement
{
    Shoot, GetShot, Agony, BulletShell, Recharge
}