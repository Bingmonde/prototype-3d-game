using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GateLogic : MonoBehaviour
{
    HudLogic hudLogic;
    private List<string> documentsToPass = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        documentsToPass.Add("File1");
        documentsToPass.Add("File2");
        documentsToPass.Add("Key");
        hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            PlayerLogic playerLogic = other.gameObject.GetComponent<PlayerLogic>();
            if (playerLogic != null) {
                List<string> docuMissing = VerifyDocuments(playerLogic.GetObjectList());
                if (docuMissing.Count == 0)
                {
                    Transform doorTransform = transform.parent.Find("Door");
                    Vector3 angles = new Vector3(0, 270, 0);
                    doorTransform.rotation = Quaternion.Euler(angles);
                    HudLogic.instance.ShowVictoryInfo();
                    GameManager.instance.StopGame();
                }
                else
                {
                    string hint = "Vous manquez " + docuMissing.Count + " ducuments: ";
                    foreach (var item in docuMissing)
                    {
                        hint += item.ToString() + " ";
                    }
                    HudLogic.instance.ShowHint(hint);
                    return;

                }
            }
            
        }
    }


    List<string> VerifyDocuments(List<string> docus) {
        List<string> docuMissing = new List<string>();
        foreach (var item in documentsToPass)
        {
            if (!docus.Contains(item)) {
                Debug.Log("missing:" + item);
                docuMissing.Add(item);
                switch (item)
                {
                    case "File1":
                        hudLogic.showIconFile1(true);
                        break;
                    case "File2":
                        hudLogic.showIconFile2(true);
                        break;
                    case "Key":
                        hudLogic.showIconKey(true);
                        break;
                }
            }                 
        }
        Debug.Log("missing" + docuMissing.Count + "docus");
        return docuMissing;
    }
}
