using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelParserStarter : MonoBehaviour
{
    public string filename;

    public GameObject Rock;

    public GameObject Brick;

    public GameObject QuestionBox;

    public GameObject Stone;

    public Text Score;

    public Text coinTracker;

    public Text TimeText;

    public int score;

    public int coins;

    public float time;

    public Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        RefreshParse();
        time = 400;
    }


    private void FileParser()
    {
        string fileToParse = string.Format("{0}{1}{2}.txt", Application.dataPath, "/Resources/", filename);

        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            int row = 0;

            while ((line = sr.ReadLine()) != null)
            {
                int column = 0;
                char[] letters = line.ToCharArray();
                foreach (var letter in letters)
                {
                    SpawnPrefab(letter, new Vector3 (column, -row, 0));
                    column++;
                }
                row++;

            }

            sr.Close();
        }
    }

    private void SpawnPrefab(char spot, Vector3 positionToSpawn)
    {
        GameObject ToSpawn;

        switch (spot)
        {
            case 'b': ToSpawn = Brick; break;
            case '?': ToSpawn = QuestionBox; break;
            case 'x': ToSpawn = Rock; break;
            case 's': ToSpawn = Stone; break;
            //default: Debug.Log("Default Entered"); break;
            default: return;
                //ToSpawn = //Brick;       break;
        }

        ToSpawn = GameObject.Instantiate(ToSpawn, parentTransform);
        ToSpawn.transform.localPosition = positionToSpawn;
    }

    public void RefreshParse()
    {
        GameObject newParent = new GameObject();
        newParent.name = "Environment";
        newParent.transform.position = parentTransform.position;
        newParent.transform.parent = this.transform;
        
        if (parentTransform) Destroy(parentTransform.gameObject);

        parentTransform = newParent.transform;
        FileParser();
    }



    public void Update()
    {
        time -= Time.deltaTime;
        TimeText.text = time.ToString("#");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider BC = hit.collider as BoxCollider;
                if (BC != null)
                {
                    if (BC.gameObject.name == "Brick(Clone)")
                    {
                        Destroy(BC.gameObject);
                    }
                    else if (BC.gameObject.name == "Question(Clone)")
                    {
                        coins++;
                        coinTracker.text = "" + coins;
                        Destroy(BC.gameObject);
                    }
                    
                }
            }
        }
    }
}
