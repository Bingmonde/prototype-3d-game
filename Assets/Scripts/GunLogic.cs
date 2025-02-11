using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GunLogic : MonoBehaviour
{

    Gun pistol;
    Gun submachine;
    Gun assault;
    List<Gun> gunList = new List<Gun>();
    bool submachineIsShooting = false;
    int assaultCounter;

    [SerializeField]
    AudioClip pistolSound;
    [SerializeField]
    AudioClip submachineSound;
    [SerializeField]
    AudioClip assaultSound;

    AudioSource[] shootingAudioSource;


    PlayerLogic playerLogic;
    //HudLogic hudLogic;

    bool isUsingAssault = false;


    // Start is called before the first frame update
    void Start()
    {
        initialiseGunParametres();
        playerLogic = GetComponent<PlayerLogic>();
        //hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
        shootingAudioSource = GetComponentsInChildren<AudioSource>(); // utilise index 0 pour l'effet sonore du tir
        Debug.Log(shootingAudioSource.Length);
        assaultCounter = assault.ShotByClick;
    }

    private void initialiseGunParametres()
    {
        pistol = new Gun(Guns.Pistol, 25, 10, 0, 1);
        submachine = new Gun(Guns.Submachine, 10, 30, 600, 1);
        assault = new Gun(Guns.Assault, 30, 20, 30, 3);
        gunList.Add(pistol);
        gunList.Add(submachine);
        gunList.Add(assault);
    }

    public int GetCurrentGunMaxBulletsInMagazine(int index) {
        return gunList[index].Magazine;
    }
    public int GetCurrentGunActualBulletsInMagazine(int index) {
        return gunList[index].AvailableBulletsInMagazine;
    }

    public bool isMagazineFull(int index) {
        return gunList[index].AvailableBulletsInMagazine == gunList[index].Magazine;
    }

    // Retourner le nombre de balles rechargées
    public int LoadBulletsForCurrentGun(int bulletsToLoad, int index) {
        Gun currentGun = gunList[index];
        int maxBulletToLoad = currentGun.Magazine - currentGun.AvailableBulletsInMagazine;
        if (currentGun.Magazine - currentGun.AvailableBulletsInMagazine < bulletsToLoad)
        {
            currentGun.AddBulletsInMagazine(maxBulletToLoad);
            return maxBulletToLoad;
        }
        else {
            currentGun.AddBulletsInMagazine(bulletsToLoad);
            return bulletsToLoad;
        }
    }
   

    public void TirPistol() {
        // vérifier si les balles sont suffisantes
        if (!playerLogic.CanShootEnemy(pistol.ShotByClick)) {
            return;
        }
        // gestion des bullets, déduire les balles dans le chargeur
        pistol.AddBulletsInMagazine(-pistol.ShotByClick);
        shootingAudioSource[0].PlayOneShot(pistolSound);
        shootingAudioSource[0].volume = AudioManager.instance.GetSoundVolumne();
        playerLogic.UpdateBulletInfoHUD();

        // tir et gestion des dégats sur ennemy
        playerLogic.ShootEnemy(pistol.Damage * pistol.ShotByClick);


    }
    private void TirSubmachineGun() {
        // vérifier si les balles sont suffisantes
        //TODO: Maybe....
        if (!playerLogic.CanShootEnemy(submachine.ShotByClick))
        {
            StopTirSubmachine();
            return;
        }
        shootingAudioSource[0].PlayOneShot(submachineSound);
        shootingAudioSource[0].volume = AudioManager.instance.GetSoundVolumne();
        // gestion des bullets
        submachine.AddBulletsInMagazine(-submachine.ShotByClick);
        playerLogic.UpdateBulletInfoHUD();

        // tir et gestion des dégats sur ennemy
        playerLogic.ShootEnemy(submachine.Damage * submachine.ShotByClick);

    }

    private void TirAssaultGun()
    {
        if (assaultCounter <= 0) {
            return;
        }

        assaultCounter--;
        shootingAudioSource[0].PlayOneShot(assaultSound);
        shootingAudioSource[0].volume = AudioManager.instance.GetSoundVolumne();
        // gestion des bullets, 3 balle /tir
        assault.AddBulletsInMagazine(-1);
        playerLogic.UpdateBulletInfoHUD();

        // tir et gestion des dégats sur ennemy
        playerLogic.ShootEnemy(assault.Damage);

    }


    public void StartTirSubmachine() {
        submachineIsShooting = true;
        TirSubmachineGun();
        StartCoroutine(Submachinetimer());
    }

    public void StopTirSubmachine() {
        StopCoroutine(Submachinetimer());
    }

    public void StartTirAssaultGun() {
        // vérifier si les balles sont suffisantes
        if (!playerLogic.CanShootEnemy(assault.ShotByClick))
        {
            return;
        }
        if (!isUsingAssault) {
            assaultCounter = assault.ShotByClick;
            TirAssaultGun();
            StartCoroutine(AssaultTimer());
            isUsingAssault = true;
        }
    }

    private IEnumerator AssaultTimer() {
        
        while (assaultCounter > 0) {
            yield return new WaitForSeconds(60 / assault.Speed); // 1 balle par 2 seconde
            TirAssaultGun();
        }
        StopCoroutine(AssaultTimer());
        Debug.Log("stop assault using");
        isUsingAssault = false;
    }
    private IEnumerator Submachinetimer() {
        // detecte si le clique "fire1" est toujours valide chaque 

        while (Input.GetMouseButton(0))
        {
            // 1 balle par 0.1 seconde, très vite, donc je fais ralentir 3 fois de vitesse, soit 1 balle / 0.3 seconde
            yield return new WaitForSeconds(1f * 60 / submachine.Speed);  
            // prochain tir
            TirSubmachineGun();
        }
    }


  
}

public class Gun
{
    public Guns Name { get; }
    public int Damage { get; }
    public int Magazine { get; }

    public int Speed { get; }

    public int ShotByClick { get; }

    public int AvailableBulletsInMagazine { get; set; }

    public Gun(Guns name, int damage, int magazine, int speed, int shotByClick)
    {
        Name = name;
        Damage = damage;
        Magazine = magazine;
        Speed = speed;
        ShotByClick = shotByClick;
    }

    public void AddBulletsInMagazine(int bullets){
        AvailableBulletsInMagazine += bullets;
    }
    

}

public enum Guns
{
    Pistol, Submachine, Assault
}


