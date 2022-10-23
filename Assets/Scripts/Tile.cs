using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Property
    public bool IsBuildTower { set; get; }
    private void Awake()
    {
        IsBuildTower = false;
    }
}
/*
 * File: Tile.cs
 * Desc: 타워 배치가 가능한 TileWall 오브젝트에 부착
 * -자동구현 프로퍼티로 제작
 * 프로퍼티의 set, get에 추가적인 내용이 없는 기본형, 코드 길이를 줄이기 위해 사용됨
 * 변수를 따로 선언할 필요 없으며 set, get의 기본형이 자동 작성됨
 */
