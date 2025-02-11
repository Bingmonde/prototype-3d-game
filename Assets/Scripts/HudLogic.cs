using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class HudLogic : MonoBehaviour
{
    public static HudLogic instance { get; private set; }
    [SerializeField]
    // est-ce que tu voulais infoShowTime comme Serialized ou c'est un accident causé par la mise en commentaire de ces 2 lignes?
    //private List<Sprite> articleImages;
    //private Image currentImage;
    private float infoShowTime = 3f;
    private TextMeshProUGUI articleInfo;
    private TextMeshProUGUI textInfo;
    private TextMeshProUGUI numBullets;
    private TextMeshProUGUI numBulletsMagazine;
    private TextMeshProUGUI customedInfo;

    private Image lifePointImage;
    private TextMeshProUGUI lifePointNum;
    private Image failureEffect;
    private GameObject infos;

    private GameObject buttonQuit;
    private GameObject buttonRestart;
    private GameObject buttonPause;
    private TextMeshProUGUI textPause;

    private Image imageFile1;
    private Image imageFile2;
    private Image imageKey;
    private Image imageGrenade;
    private Image imageGrenadeBackground;
   // private Image imageBullet;

    private Image iconFile1;
    private Image iconFile2;
    private Image iconKey;
   

    private GameObject missingDocus;


    private List<Image> weaponsImages;
    private List<Image> weaponsImagesBG;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
    }

    void Start()
    {
        //currentImage = GameObject.Find("ArticleImage").GetComponent<Image>();
        
        articleInfo = GameObject.Find("ArticleInfo").GetComponent<TextMeshProUGUI>();
        textInfo = GameObject.Find("GameStatus").GetComponent <TextMeshProUGUI>();
        buttonQuit = GameObject.Find("ButtonQuit");
        buttonRestart = GameObject.Find("ButtonRetry");
        buttonPause = GameObject.Find("ButtonContinue");
        textPause = GameObject.Find("TextPause").GetComponent<TextMeshProUGUI>();
        customedInfo = GameObject.Find("CustomedInfo").GetComponent<TextMeshProUGUI>();

        lifePointImage = GameObject.Find("LifeImage").GetComponent<Image>();
        lifePointNum = GameObject.Find("LifePoint").GetComponent<TextMeshProUGUI>();
        failureEffect = GameObject.Find("FailureEffect").GetComponent<Image>();

        missingDocus = GameObject.Find("MissingDocus");
        imageFile1 = GameObject.Find("File1Image").GetComponent<Image>();
        imageFile2 = GameObject.Find("File2Image").GetComponent<Image>();
        imageKey = GameObject.Find("KeyImage").GetComponent<Image>();
        iconFile1 = GameObject.Find("MissingFile1").GetComponent<Image>();
        iconFile2 = GameObject.Find("MissingFile2").GetComponent<Image>();
        iconKey = GameObject.Find("MissingKey").GetComponent<Image>();
        imageGrenade = GameObject.Find("GrenadeImage").GetComponent<Image>();
        imageGrenadeBackground = GameObject.Find("GrenadeBackground").GetComponent<Image>();    
        //imageBullet = GameObject.Find("BulletImage").GetComponent<Image>();
        numBullets = GameObject.Find("NumBullets").GetComponent<TextMeshProUGUI>();
        Debug.Log(numBullets == null);
        numBulletsMagazine = GameObject.Find("NumBulletInMagazin").GetComponent <TextMeshProUGUI>();
        infos = GameObject.Find("Infos");



        // Images des armes
        GameObject weaponsImagesParent = GameObject.Find("GunImages");
        if (weaponsImagesParent != null)
        {
            Image[] weaponsImagesArray = weaponsImagesParent.GetComponentsInChildren<Image>();
            weaponsImages = new List<Image>(weaponsImagesArray);
        }
        GameObject weaponsImagesBGParent = GameObject.Find("GunImagesBG");
        if (weaponsImagesBGParent != null)
        {
            Image[] weaponsImagesBGArray = weaponsImagesBGParent.GetComponentsInChildren<Image>();
            weaponsImagesBG = new List<Image>(weaponsImagesBGArray);
        }

        ResetAll();
    }


    public void ShowVictoryInfo() {
        infos.SetActive(true);
        textInfo.text = "Félicitation! Vous avez gagné!";
        buttonQuit.SetActive(true);
        buttonRestart.SetActive(true);

    }
    public void ShowFailureInfo() {
        infos.SetActive(true);
        textInfo.text = "Orz....Game Over";
        failureEffect.enabled = true;
        buttonQuit.SetActive(true);
        buttonRestart.SetActive(true);
    }


    public void ShowHint(string hint) {
        missingDocus.SetActive(true);
        ShowMessage(hint);
        
    }

    private void ShowMessage(string message)
    {
        
        textInfo.text = message;
        infos.SetActive(true);
        StartCoroutine(MessageTimerCoroutine());
        
    }


    void HideMessage() {
        //Debug.Log("Hiding message");
        textInfo.text = "";
        StopCoroutine(MessageTimerCoroutine());
        infos.SetActive(false);
        showIconFile1(false);
        showIconFile2(false);
        showIconKey(false);
        showGrenade(false);
        imageGrenadeBackground.enabled = false;
        missingDocus.SetActive(false);
    }

    public void ShowArticleImageAndInfo(string article) {
        //FindArticleImage(article);

        switch (article) {
            case "File1": 
                ShowFile1();
                break;
            case "File2":
                ShowFile2(); 
                break;
            case "Key":
                ShowKey(); 
                break;
            case "Grenade":
                showGrenade(true);
                break;
        }
        articleInfo.text = article + " trouvé(e)";

        

        StartCoroutine(ImageInfoTimerCoroutine());
    }
    
    void DisableImageInfo() {
        StopCoroutine(ImageInfoTimerCoroutine());
        articleInfo.text = "";
    }

    // Gestion du temps
    private IEnumerator ImageInfoTimerCoroutine()
    {
            yield return new WaitForSeconds(infoShowTime);
            DisableImageInfo();
    }

    private IEnumerator MessageTimerCoroutine()
    {
        yield return new WaitForSeconds(infoShowTime);
        HideMessage();
    }

    public void SetLifepointHUD(float point)
    {
        lifePointNum.text = point.ToString();
        float newScaleX = point / 100f;
        Vector3 newScale = new Vector3(newScaleX, lifePointImage.transform.localScale.y, lifePointImage.transform.localScale.z);
        lifePointImage.transform.localScale = newScale;

    }


    private void ResetAll() {
       // currentImage.enabled = false;
        buttonQuit.SetActive(false);
        buttonRestart.SetActive(false);
        buttonPause.SetActive(false);
        textPause.enabled = false;
        infos.SetActive(false);
        textInfo.text = "";
        failureEffect.enabled = false;
        imageFile1.enabled = false;
        imageFile2.enabled = false; 
        imageKey.enabled = false;
        iconFile1.enabled = false;
        iconFile2.enabled = false;  
        iconKey.enabled = false;
        imageGrenade.enabled = false;
        imageGrenadeBackground.enabled = false;
        missingDocus.SetActive(false);

        weaponsImages[1].enabled = false;
        weaponsImages[2].enabled = false;
        weaponsImagesBG[1].enabled = false;
        weaponsImagesBG[2].enabled = false;
    }

    public void ShowFile1() {
        imageFile1.enabled = true;
    }

    public void ShowFile2()
    {
        imageFile2.enabled = true;
    }
    public void ShowKey()
    {
        imageKey.enabled = true;
    }
    public void showGrenade(bool value)
    {
        imageGrenade.enabled = value;
    }
    public void showIconFile1(bool value) {
        iconFile1.enabled = value;
    }
    public void showIconFile2(bool value)
    {
        iconFile2.enabled = value;
    }
    public void showIconKey(bool value)
    {
        iconKey.enabled = value;
    }

    public void UsingGrenade(bool value) {
        imageGrenadeBackground.enabled = value;
        showGrenade(value);
        ShowMessage("Lancer la grenade...");
    }


    public void UpdateBulletsHUD(int totalNum, int numBM)
    {
        numBullets.text = " X " + totalNum;
        numBulletsMagazine.text = numBM + "";
    }


    // Afficher l'info lorsque le player recharge
    public void PlayerLoadingBullets(string text) {
        if (text.Equals(articleInfo.text)) {
            return;
        }
        articleInfo.text = text;
        StartCoroutine(LoadingTimerCoroutine());

    }
    private IEnumerator LoadingTimerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        articleInfo.text = "";
    }


    // mise a jour l'image d'arme actuel
    public void UpdateCollectGunImageHUD(int index, bool value) {
        weaponsImages[index].enabled = value;
    }

    public void UpdateChangeGunImageHUD(int index, bool value)
    {
        weaponsImagesBG[index].enabled = value;
    }

    public void PauseOperation(bool isPaused) {
        // afficher le bouton CONTINER, QUITTER, le text PAUSE
        buttonPause.SetActive(isPaused);
        buttonQuit.SetActive(isPaused);
        textPause.enabled = isPaused;
    }

    public void ShowCustomedInfo(string text, float duration) {
        StartCoroutine(CustomedInfoTimer(duration));
        customedInfo.text = text;
    }

    private IEnumerator CustomedInfoTimer(float duration) {
        yield return new WaitForSeconds(duration);
        customedInfo.text = string.Empty;

    
    }
    

}
