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
 * Desc: Ÿ�� ��ġ�� ������ TileWall ������Ʈ�� ����
 * -�ڵ����� ������Ƽ�� ����
 * ������Ƽ�� set, get�� �߰����� ������ ���� �⺻��, �ڵ� ���̸� ���̱� ���� ����
 * ������ ���� ������ �ʿ� ������ set, get�� �⺻���� �ڵ� �ۼ���
 */
