using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TowerManager : MonoBehaviour
{
    // Start is called before the first frame update

    //�ӽ� ������Ʈ��
    [SerializeField]
    public GameObject towerSpawner;
    [SerializeField]
    public Transform tiles;
    [SerializeField]
    public Sprite[] foodSprites;
    [SerializeField]
    public GameObject wait_tower;
    [SerializeField]
    public GameObject tower1;
    [SerializeField]
    public GameObject tower2;

    private RecipeList recipeList;

    [Serializable]
    public class RecipeList
    {
        public Recipe[] recipes;
    }

    [Serializable]
    public class Recipe
    {
        public string[] input;
        public string output;
    }

    private void Start()
    {
        Debug.Log("loadedJson");
        recipeList = JsonUtility.FromJson<RecipeList>(Resources.Load<TextAsset>("recipe").text);

    }


    //Ÿ�� ���׷��̵� �Լ�
    public bool TowerUpgrade(GameObject obj1, GameObject obj2) // ������ �ִ� Ÿ���� obj1. �������� Ÿ���� obj2  (1 <- 2 2�� ��� 1�� ���� ��Ȳ �� 1����ġ�� ���׷��̵� �� Ÿ���� ����) 
    {
        bool result = false;
        if (obj1.GetComponent<TowerWeapon>() == true && obj2.GetComponent<TowerWeapon>() == true)
        {
            Debug.Log("========== Ÿ������ ==========");
            string towerWeapon = obj1.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            Debug.Log(towerWeapon);
            towerWeapon = obj2.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            Debug.Log(towerWeapon);
            String tower1_name = obj1.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            String tower2_name = obj2.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            string new_tower = FindrRecipe(tower1_name,tower2_name);
            if (!new_tower.Equals("��"))
            {
                for (int i = 0; i < towerSpawner.GetComponent<TowerSpawner>().towerTemplate.Length; i++)
                {
                    TowerTemplate tp = towerSpawner.GetComponent<TowerSpawner>().towerTemplate[i];
                    Debug.Log(new_tower + "//" + tp.weapon[0].name);

                    if (new_tower.Equals(tp.weapon[0].name))
                    {
                        Tile temp_tile = obj1.GetComponent<TowerWeapon>().ownerTile;
                        obj2.GetComponent<TowerWeapon>().ownerTile.IsBuildTower = false;


                        //towerSpawner.GetComponent<TowerSpawner>().ReadyToSpawnTower(i);
                        //towerSpawner.GetComponent<TowerSpawner>().SpawnTower(temp_tile.transform);

                        if (new_tower.Split('_')[1].Equals("2"))
                        {
                            //tower2�� Food tower1�� Tool
                            if (tower1_name.Contains("tool_"))
                            {
                                Destroy(obj2);
                                obj1.transform.position = temp_tile.transform.position;
                                foreach (Sprite sp in foodSprites)
                                {
                                    if (tower2_name.Substring(8).Equals(sp.name.Substring(5)))
                                    {
                                        obj1.transform.Find("food").GetComponent<SpriteRenderer>().sprite = sp;
                                        obj1.GetComponent<Animator>().SetInteger("state", 1);
                                        break;
                                    }
                                }
                                //yield return new WaitForSeconds(tp.weapon[0].buildingTime);
                                StartCoroutine(spawnToewer(obj1, i, temp_tile, tp.weapon[0].buildingTime));
                            }
                            //tower1�� Food tower2�� Tool
                            else
                            {
                                Destroy(obj1);
                                obj2.transform.position = temp_tile.transform.position;
                                foreach (Sprite sp in foodSprites)
                                {
                                    if (tower1_name.Substring(8).Equals(sp.name.Substring(5)))
                                    {
                                        obj2.transform.Find("food").GetComponent<SpriteRenderer>().sprite = sp;
                                        obj2.GetComponent<Animator>().SetInteger("state", 1);
                                        break;
                                    }
                                }
                                //yield return new WaitForSeconds(tp.weapon[0].buildingTime);
                                StartCoroutine(spawnToewer(obj2, i, temp_tile, tp.weapon[0].buildingTime));
                                //Destroy(obj1);
                            }

                            //temp_tile.IsBuildTower = false;
                            //towerSpawner.GetComponent<TowerSpawner>().ReadyToSpawnTower(i);
                            //towerSpawner.GetComponent<TowerSpawner>().SpawnTower(temp_tile.transform);

                            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("UpgradeLV2");
                        }
                        
                        else if (new_tower.Split('_')[1].Equals("3"))
                        {

                            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("UpgradeLV3");
                            Destroy(obj1);
                            Destroy(obj2);
                            GameObject wait_obj = Instantiate(wait_tower, temp_tile.transform);
                            foreach (Sprite sp in foodSprites)
                            {
                                if (new_tower.Substring(8).Equals(sp.name.Substring(5)))
                                {
                                    wait_obj.transform.Find("food").GetComponent<SpriteRenderer>().sprite = sp;
                                    break;
                                }
                            }
                            wait_obj.GetComponent<Animator>().SetFloat("waitTime", 1/tp.weapon[0].buildingTime);
                            StartCoroutine(spawnToewer(wait_obj, i, temp_tile, tp.weapon[0].buildingTime));

                        }
                        

                        Debug.Log("Ÿ�� " + new_tower + "����!!");

                        result = true;
                        break;
                    }
                }
            }
            //

        }
        return result;
    }

    private IEnumerator spawnToewer(GameObject obj, int i, Tile temp_tile, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
        temp_tile.IsBuildTower = false;
        towerSpawner.GetComponent<TowerSpawner>().ReadyToSpawnTower(i);
        towerSpawner.GetComponent<TowerSpawner>().SpawnTower(temp_tile.transform);
        yield break;
    }

    public bool TowerUpgrade2(GameObject obj1, string tower2_name) 
    {
        bool result = false;
        if (obj1.GetComponent<TowerWeapon>() == true)
        {
            String tower1_name = obj1.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            string new_tower = FindrRecipe(tower1_name, tower2_name);
            if (!new_tower.Equals("��"))
            {
                for (int i = 0; i < towerSpawner.GetComponent<TowerSpawner>().towerTemplate.Length; i++)
                {
                    TowerTemplate tp = towerSpawner.GetComponent<TowerSpawner>().towerTemplate[i];
                    Debug.Log(new_tower + "//" + tp.weapon[0].name);
                    
                    if (new_tower.Equals(tp.weapon[0].name))
                    {
                        Tile temp_tile = obj1.GetComponent<TowerWeapon>().ownerTile;

                        //tower2�� Food tower1�� Tool
                        if (tower1_name.Contains("tool_"))
                        {
                            foreach (Sprite sp in foodSprites)
                            {
                                if (tower2_name.Substring(8).Equals(sp.name.Substring(5)))
                                {
                                    obj1.transform.Find("food").GetComponent<SpriteRenderer>().sprite = sp;
                                    obj1.GetComponent<Animator>().SetInteger("state", 1);
                                    StartCoroutine(spawnToewer(obj1, i, temp_tile, tp.weapon[0].buildingTime));
                                    break;
                                }
                            }

                        }
                        //tower1�� Food tower2�� Tool
                        else
                        {
                            Destroy(obj1);
                            foreach (Sprite sp in foodSprites)
                            {
                                if (tower1_name.Substring(8).Equals(sp.name.Substring(5)))
                                {
                                    GameObject obj2_ref = null;
                                    for (int j = 0; j < towerSpawner.GetComponent<TowerSpawner>().towerTemplate.Length; j++)
                                    {
                                        TowerTemplate temp_tp = towerSpawner.GetComponent<TowerSpawner>().towerTemplate[j];
                                        if (temp_tp.weapon[0].name.Equals(tower2_name))
                                        {
                                            obj2_ref = temp_tp.towerPrefab;
                                        }

                                    }
                                    GameObject obj2 = Instantiate(obj2_ref, temp_tile.transform);
                                    obj2.transform.Find("food").GetComponent<SpriteRenderer>().sprite = sp;
                                    obj2.GetComponent<Animator>().SetInteger("state", 1);
                                    StartCoroutine(spawnToewer(obj2, i, temp_tile, tp.weapon[0].buildingTime));
                                    break;
                                }
                            }
                        }


                        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("UpgradeLV2");
                        Debug.Log("Ÿ�� " + new_tower + "����!!");

                        result = true;
                        break;
                    }
                }
            }
            //

        }
        return result;
    }

    //������ Ȯ�� �Լ�
    private string FindrRecipe(string input0, string input1)
    {
        foreach (Recipe recipe in recipeList.recipes)
        {
            if ((Array.IndexOf(recipe.input,input0) == 0 && Array.IndexOf(recipe.input, input1) == 1) || (Array.IndexOf(recipe.input, input0) == 1 && Array.IndexOf(recipe.input, input1) == 0))
            {
                
                Debug.Log("�����ǰ� ���� �մϴ�. \n"+recipe.input[0] + " + " + recipe.input[1] + " = " + recipe.output);
                return recipe.output;
            }
        }
        return "��";
    }


}
