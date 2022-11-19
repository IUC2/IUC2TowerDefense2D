using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TowerManager : MonoBehaviour
{
    // Start is called before the first frame update

    //임시 오브젝트들
    [SerializeField]
    public GameObject towerSpawner;
    [SerializeField]
    public Transform tiles;
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

    [CustomEditor(typeof(TowerManager))]
    public class PrepareSceneEditor : Editor
    {
        //임시테스트 버튼
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TowerManager myScript = (TowerManager)target;
            if (GUILayout.Button("TowerUpgrade Test"))
            {
                myScript.TowerUpgrade(myScript.tower1, myScript.tower2);
            }
            if (GUILayout.Button("FindrRecipe Test"))
            {

            }
        }
    }

    //타워 업그레이드 함수
    public bool TowerUpgrade(GameObject obj1, GameObject obj2) // 가만히 있는 타워가 obj1. 옯겨지는 타워가 obj2  (1 <- 2 2를 끌어서 1에 놓는 상황 즉 1번위치에 업그레이드 된 타워가 생성) 
    {
        bool result = false;
        if (obj1.GetComponent<TowerWeapon>() == true && obj2.GetComponent<TowerWeapon>() == true)
        {
            Debug.Log("========== 타워정보 ==========");
            string towerWeapon = obj1.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            Debug.Log(towerWeapon);
            towerWeapon = obj2.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name;
            Debug.Log(towerWeapon);
            string new_tower = FindrRecipe(obj1.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name, obj2.GetComponent<TowerWeapon>().towerTemplate.weapon[0].name);
            if (!new_tower.Equals("똥"))
            {
                for (int i = 0; i < towerSpawner.GetComponent<TowerSpawner>().towerTemplate.Length; i++)
                {
                    TowerTemplate tp = towerSpawner.GetComponent<TowerSpawner>().towerTemplate[i];
                    Debug.Log(new_tower + "//" + tp.weapon[0].name);

                    if (new_tower.Equals(tp.weapon[0].name))
                    {
                        Tile temp_tile = obj1.GetComponent<TowerWeapon>().ownerTile;
                        temp_tile.IsBuildTower = false;
                        obj2.GetComponent<TowerWeapon>().ownerTile.IsBuildTower = false;
                        Destroy(obj1);
                        Destroy(obj2);
                        towerSpawner.GetComponent<TowerSpawner>().ReadyToSpawnTower(i);
                        towerSpawner.GetComponent<TowerSpawner>().SpawnTower(temp_tile.transform);
                        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("UpgradeLV1");
                        Debug.Log("타워 " + new_tower + "생성!!");

                        result = true;
                        break;
                    }
                }
            }
            //

        }
        return result;
    }

    //레시피 확인 함수
    private string FindrRecipe(string input0, string input1)
    {
        foreach (Recipe recipe in recipeList.recipes)
        {
            if (Array.IndexOf(recipe.input,input0) != -1 && Array.IndexOf(recipe.input, input1) != -1)
            {
                
                Debug.Log("레시피가 존재 합니다. \n"+recipe.input[0] + " + " + recipe.input[1] + " = " + recipe.output);
                return recipe.output;
            }
        }
        return "똥";
    }


}
