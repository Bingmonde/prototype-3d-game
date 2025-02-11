using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    CharacterController _playerController;
    [SerializeField]
    private float walkingSpeed = 20f;

    private List<string> objectList;

    private GameObject startPostion;

    private float currentLifePoint;
    private bool isDead = false;
    private bool isPaused = false;

    // Où lancer la grenade
    private GameObject sprawPoint;
    private int numGrenade = 0;
    [SerializeField]
    GameObject grenadePrefab;
    [SerializeField]
    float forceAtMaxCharge = 50f;


    // Les armes
    [SerializeField]
    private int maxNumBulletPerRecharge = 10;
    private int numBullets = 10; // par défaut, le player a 10 balles au commencement du jeu
    private List<Guns> possessedArms = new List<Guns>();
    private float loadingBulletTime = 0.5f;
    private bool isLoading = false;
    private List<GameObject> weaponsIntances = new List<GameObject>();
    private int currentGunIndex = 0; // in possessedArms
    GunLogic gunLogic;

    [SerializeField]
    GameObject effect_blood;
    [SerializeField]
    GameObject effect_blood_enemy;
    GameObject bloodSprawPoint;
    [SerializeField]
    GameObject effect_dust;
    [SerializeField]
    GameObject effect_shot;

    List<GameObject> listFirePoint = new List<GameObject>();

    HudLogic hudLogic;

    // effet sonore
    [SerializeField]
    AudioClip changeGunSound;
    [SerializeField]
    AudioClip loadBulletSound;
    [SerializeField]
    AudioClip emptyBulletSound;
    [SerializeField]
    AudioClip shootOtherOjectSound;
    [SerializeField]
    AudioClip test;

    [SerializeField]
    AudioClip throwGrenadeSound;

    AudioSource[] audioSources;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        objectList = new List<string>();
        startPostion = GameObject.Find("StartPoint");
        currentLifePoint = 100f;
        Debug.Log("player point from start() : " + currentLifePoint);
        _playerController.transform.position = startPostion.transform.position;
        sprawPoint = GameObject.Find("SprawPoint");
        hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
        bloodSprawPoint = GameObject.Find("BloodEffectPoint");


        // guns
        possessedArms.Add(Guns.Pistol);
        //possessedArms.Add("Pistol");
        GameObject pistol = GameObject.Find("Pistol");
        GameObject submachine = GameObject.Find("SubmachineGun");
        GameObject assault = GameObject.Find("AssaultRifle");
        GameObject pistalFirePoint = GameObject.Find("PistolFirePoint");
        GameObject submachineFirePoint = GameObject.Find("SubmachineFirePoint");
        GameObject assaultFirePoint = GameObject.Find("AssaultFirePoint");
        listFirePoint.Add(pistalFirePoint);
        listFirePoint.Add(submachineFirePoint);
        listFirePoint.Add(assaultFirePoint);

        submachine.SetActive(false);
        assault.SetActive(false);

      
        weaponsIntances.Add(pistol);
        weaponsIntances.Add(submachine);
        weaponsIntances.Add(assault);
        gunLogic = GetComponent<GunLogic>();


        // recharger la premier fois les bullets par defaut
        if (numBullets >= maxNumBulletPerRecharge)
        {
            numBullets -= maxNumBulletPerRecharge;
            gunLogic.LoadBulletsForCurrentGun(maxNumBulletPerRecharge, GetCurrentGunIndexInGunList());
        }


        if (hudLogic != null)
        {
            int bulletsInMagazine = gunLogic.GetCurrentGunActualBulletsInMagazine(GetCurrentGunIndexInGunList());
            hudLogic.UpdateBulletsHUD(numBullets, bulletsInMagazine);
        }

        // 5 audio sources
        // 0 - tir, 1 - recharger, 2 - changer d'arme, 3 - bullet atteind sur autre objet, 4 - lancer grenade
        audioSources = GetComponentsInChildren<AudioSource>();
    }


    void Update()
    {
        if (isPaused || isDead)
        {
            return;
        }

        MovePlayer();

        if (Input.GetKeyDown(KeyCode.G) && numGrenade > 0)
        {
            LaunchGrenade();
        }
        //if (Input.GetKeyDown(KeyCode.R) && Mathf.Floor(numBullets / bulletsMaxMagazine) > 0)
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadBullets();
        }

        // changer de la prochaine arme
        if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Debug.Log("keydown E");
            ChangeGun(1);
        }

        // changer de la dernière arme
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ChangeGun(-1);
        }

        ManageShooting();


    }

    void MovePlayer()
    {

        // Rotation sur l’axe Y
        float h = Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);

        // deplacement X-Z
        Vector3 playerInput = Vector3.zero;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.z = Input.GetAxis("Vertical");

        Vector3 lateralMove = transform.right * playerInput.x * walkingSpeed * Time.deltaTime;
        Vector3 verticalMove = transform.forward * playerInput.z * walkingSpeed * Time.deltaTime;

        _playerController.Move(lateralMove + verticalMove);

    }


    public bool CanShootEnemy(int bulletsConsummed) {
        if (isDead || isLoading || isPaused) { return false; }

        if (NeedToLoadBullets(bulletsConsummed))
        {
            audioSources[(int) PlayerAudioManagement.Tir].PlayOneShot(emptyBulletSound);
            audioSources[(int)PlayerAudioManagement.Tir].volume = AudioManager.instance.GetSoundVolumne();
            hudLogic.PlayerLoadingBullets("Il faut recharger...");
            return false;
        }
        return true;
    }

    public void ShootEnemy(int pointsDamage)
    {
        // effet special de tir
        GameObject currentGunFirePoint = listFirePoint[GetCurrentGunIndexInGunList()];
        GameObject shootingFire = Instantiate(effect_shot, currentGunFirePoint.transform.position, Quaternion.identity);


        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            EnemyLogic enemyLogic = hit.collider.gameObject.GetComponentInParent<EnemyLogic>();
            if (enemyLogic != null)
            {
                float enemyLosingPoints = GameManager.instance.getDamagePercentage(hit.collider.tag) * pointsDamage;
                enemyLogic.IsHitOperation(enemyLosingPoints);
                GameObject blood = Instantiate(effect_blood_enemy, hit.point, Quaternion.identity);
                // Appliquer la force dans le dernier hit
                Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    enemyLogic.EnableAgent(false);
                    rb.AddExplosionForce(15f, hit.point, 1f, 1f, ForceMode.Impulse);
                }
            }
            else {
                audioSources[(int)PlayerAudioManagement.CibleManque].PlayOneShot(shootOtherOjectSound);
                audioSources[(int)PlayerAudioManagement.CibleManque].volume = AudioManager.instance.GetSoundVolumne();
                GameObject dust = Instantiate(effect_dust, hit.point, Quaternion.identity);
            }
        }

    }

    public List<string> GetObjectList()
    {
        return objectList;
    }

    public void CollectObject(string objectName)
    {
        objectList.Add(objectName);
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    public float GetPlayerLifePoint()
    {
        return currentLifePoint;
    }

    public void SetPlayerLifePoint(float damage)
    {
        if (damage > 0) {
            GameObject blood = Instantiate(effect_blood, bloodSprawPoint.transform.position, Quaternion.identity);
        }
        currentLifePoint -= damage;
        HudLogic.instance.SetLifepointHUD(currentLifePoint);
        if (currentLifePoint <= 0)
        {
            isDead = true;
            HudLogic.instance.ShowFailureInfo();
            GameManager.instance.StopGame();
        }
    }

    public void CollectGrenade()
    {
        numGrenade++;
        hudLogic.ShowArticleImageAndInfo("Grenade");
    }

    public void CollectBullets(int value)
    {
        numBullets += value;
        hudLogic.ShowArticleImageAndInfo("Bullets");
        UpdateBulletInfoHUD();

    }

    private void LaunchGrenade()
    {
        GameObject anotherGrenade = Instantiate(grenadePrefab, sprawPoint.transform.position, Quaternion.identity);
        Rigidbody rb = anotherGrenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 forwardDirection = Camera.main.transform.forward;
            rb.AddForce(forwardDirection * forceAtMaxCharge, ForceMode.Impulse);
        }
        hudLogic.UsingGrenade(true);
    }

    private void LoadBullets()
    { 
        if (gunLogic.isMagazineFull(GetCurrentGunIndexInGunList())) {
            hudLogic.PlayerLoadingBullets("Le chargeur est complet");
            return;
        }
        if (numBullets <= 0)
        {
            hudLogic.PlayerLoadingBullets("Il manque des balles ...");
            return;
        }
        isLoading = true;
        audioSources[(int)PlayerAudioManagement.Recharger].PlayOneShot(loadBulletSound);
        //audioSources[(int)PlayerAudioManagement.Recharger].PlayOneShot(test);
        audioSources[(int)PlayerAudioManagement.Recharger].volume = AudioManager.instance.GetSoundVolumne();
        StartCoroutine(LoadingTimerCoroutine());
        hudLogic.PlayerLoadingBullets("En train de recharger balles...");
    }


    /* chaque fois (en appuyer [R]), le player peut recharger 10 balles par défaut,
    ou recharger le chargeur au complet si le rechargeur reste moins de 10 balles pour atteindre son nombre maximal
     dans ce cas là, il peut recharger moins de 10 balles */
    private IEnumerator LoadingTimerCoroutine()
    {
        yield return new WaitForSeconds(loadingBulletTime);
        isLoading = false;
        int availableBulletsForOneLoad = numBullets >= maxNumBulletPerRecharge ? maxNumBulletPerRecharge : numBullets;
        int bulletConsommed = gunLogic.LoadBulletsForCurrentGun(availableBulletsForOneLoad, GetCurrentGunIndexInGunList());
        numBullets -= bulletConsommed;
        UpdateBulletInfoHUD();

    }

    public void UpdateBulletInfoHUD() {
        hudLogic.UpdateBulletsHUD(numBullets, getNumBulletInMagazin());
    }

    private int getNumBulletInMagazin() {
        return gunLogic.GetCurrentGunActualBulletsInMagazine(GetCurrentGunIndexInGunList());
    }
    private int GetCurrentGunIndexInGunList()
    {
        return (int) possessedArms[currentGunIndex];
    }

    private bool NeedToLoadBullets(int bulletToConsum)
    {
        return bulletToConsum > gunLogic.GetCurrentGunActualBulletsInMagazine(GetCurrentGunIndexInGunList());
    }

    public void CollectWeapons(Guns weaponName)
    {
        hudLogic.ShowArticleImageAndInfo(weaponName.ToString());
        if (!possessedArms.Contains(weaponName))
        {
            possessedArms.Add(weaponName);

            // mise a jour HUD
            hudLogic.UpdateCollectGunImageHUD((int)weaponName, true);
        }
    }



    private void ChangeGun(int index) // l'index dans le possessedArmes
    {
        if (possessedArms.Count <= 1)
        {
            return;
        }

        audioSources[(int)PlayerAudioManagement.ChangerDArme].PlayOneShot(changeGunSound);
        audioSources[(int)PlayerAudioManagement.ChangerDArme].volume = AudioManager.instance.GetSoundVolumne();
        Guns currentGun = possessedArms[currentGunIndex];
        currentGunIndex += index;
        if ((currentGunIndex) >= possessedArms.Count || (currentGunIndex) <= 0)
        {
            currentGunIndex = 0;
        }

        Guns selectedGun = possessedArms[currentGunIndex];


        // activer l'arme choisie, désactiver l'arme actuelle + HUD
        weaponsIntances[(int)currentGun].SetActive(false);
        hudLogic.UpdateChangeGunImageHUD((int)currentGun, false);

        weaponsIntances[(int)selectedGun].SetActive(true);
        hudLogic.UpdateChangeGunImageHUD((int)selectedGun, true);

        UpdateBulletInfoHUD();

    }


    // gestion des tirs 
    private void ManageShooting() {
        Guns currentGun = possessedArms[currentGunIndex];

        // effectuer tir par type d'arme
        if (Input.GetMouseButtonDown(0) && currentGun == Guns.Submachine)
        {
            gunLogic.StartTirSubmachine();
        }

        if (Input.GetMouseButtonUp(0) && currentGun == Guns.Submachine)
        {
            gunLogic.StopTirSubmachine();
        }

        if (Input.GetMouseButtonDown(0) && currentGun == Guns.Assault) {
            gunLogic.StartTirAssaultGun();
        }
        if (Input.GetMouseButtonDown(0) && currentGun == Guns.Pistol)
        {
            gunLogic.TirPistol();
        }
    }

    public void PlayerPauseOperation(bool pauseGame)
    {
        isPaused = pauseGame;
    }

}

//0 - tir, 1 - recharger, 2 - changer d'arme, 3 - bullet atteind sur autre objet, 4 - lancer grenade
public enum PlayerAudioManagement { 
    Tir, Recharger, ChangerDArme, CibleManque, Lancer
}


