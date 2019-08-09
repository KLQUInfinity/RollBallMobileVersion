using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData
{
    public LevelData(int levelNum)
    {
        string data = PlayerPrefs.GetString(levelNum.ToString());
        if (data.Equals(""))
        {
            return;
        }
        string[] allData = data.Split('&');
        BestTime = float.Parse(allData[0]);
        SilverTime = float.Parse(allData[1]);
        GoldTime = float.Parse(allData[2]);
    }

    public float BestTime{ set; get; }

    public float GoldTime{ set; get; }

    public float SilverTime{ set; get; }
}

public class MainMenu : MonoBehaviour
{
    public Sprite[] borders;
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;
    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;
    public Text currencyText;

    public Material playerMaterial;

    private Transform cameraTransform;
    private Transform cameraDesierdLookAt;

    private bool nextLevelLocked = false;

    private int[] costs =
        {0, 150, 150, 150,
            300, 300, 300, 300,
            500, 500, 500, 500,
            1000, 1250, 1500, 2000
        };

    private void Start()
    {
        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
        cameraTransform = Camera.main.transform;

        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
        foreach (Sprite i in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab)as GameObject;
            container.GetComponent<Image>().sprite = i;
            container.transform.SetParent(levelButtonContainer.transform, false);
            LevelData level = new LevelData(int.Parse(i.name[0].ToString()));

            string minutes = ((int)level.BestTime / 60).ToString("00");
            string seconds = (level.BestTime % 60).ToString("00.00");

            GameObject bottomPanel = container.transform.GetChild(0).GetChild(0).gameObject;

            bottomPanel.GetComponent<Text>().text = (level.BestTime != 0.0f) ? minutes + ":" + seconds : "Not Completed";

            container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked;
            container.GetComponent<Button>().interactable = !nextLevelLocked;

            if (level.BestTime == 0.0f)
            {
                nextLevelLocked = true;
            }
            else if (level.BestTime < level.GoldTime)
            {
                bottomPanel.GetComponentInParent<Image>().sprite = borders[2];
            }
            else if (level.BestTime < level.SilverTime)
            {
                bottomPanel.GetComponentInParent<Image>().sprite = borders[1];
            }
            else
            {
                bottomPanel.GetComponentInParent<Image>().sprite = borders[0];
            }

            int sceneName = int.Parse(i.name[0].ToString());
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }
        Sprite[] textures = Resources.LoadAll<Sprite>("Player");
        foreach (Sprite i in textures)
        {
            GameObject container = Instantiate(shopButtonPrefab)as GameObject;
            container.GetComponent<Image>().sprite = i;
            container.transform.SetParent(shopButtonContainer.transform, false);
            string[] s = i.name.Split('_');
            int index = int.Parse(s[1]);
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[index].ToString();
            if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
            {
                container.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (cameraDesierdLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesierdLookAt.rotation, 3 * Time.deltaTime);
        }
    }

    private void LoadLevel(int sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LockAtMenu(Transform menuTransform)
    {
        cameraDesierdLookAt = menuTransform;
        //Camera.main.transform.LookAt (menuTransform.position);
    }

    private void ChangePlayerSkin(int index)
    {
        if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
        {
            float x = (index % 4) * .25f;
            float y = ((int)index / 4) * .25f;

            if (y == .0f)
            {
                y = .75f;
            }
            else if (y == .25f)
            {
                y = .5f;
            }
            else if (y == .5f)
            {
                y = .25f;
            }
            else if (y == .75f)
            {
                y = .0f;
            }
            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));

            GameManager.Instance.currentSkinIndex = index;
            GameManager.Instance.Save();
        }
        else
        {
            //you don't have the skin, do u to but it ?
            int cost = costs[index];

            if (GameManager.Instance.currency >= cost)
            {
                GameManager.Instance.currency -= cost;
                currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
                GameManager.Instance.skinAvailability += 1 << index;
                GameManager.Instance.Save();
                shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
                ChangePlayerSkin(index);
            }
        }
    }
}
