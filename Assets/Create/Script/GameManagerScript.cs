using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    private int[,] map;
    public TextAsset stageCSV;

    enum Stage
    {
        Space,
        Block,
    }

    public GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        CreateStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadCSV()
    {
        if (stageCSV != null)
        {
            // CSV�t�@�C���̃e�L�X�g���擾
            string fileText = stageCSV.text;

            // ���s�ŕ������Ĕz��Ɋi�[
            string[] lines = fileText.Split('\n');

            // �s���Ɨ񐔂��擾
            int rowCount = lines.Length;
            int colCount = lines[0].Split(',').Length;

            // �}�b�v�z��̏�����
            map = new int[rowCount, colCount];

            // �e�s�ɂ��ď���
            for (int i = 0; i < rowCount; i++)
            {
                // �J���}�ŕ������ăf�[�^���擾
                string[] fields = lines[i].Split(',');
                for (int j = 0; j < colCount; j++)
                {
                    int value;
                    // ������𐮐��ɕϊ����ă}�b�v�z��Ɋi�[
                    if (int.TryParse(fields[j], out value))
                    {
                        map[i, j] = value;
                    }
                    else
                    {
                        Debug.LogError("CSV�t�@�C�����ɕs���ȃf�[�^���܂܂�Ă��܂��I");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV�t�@�C�����w�肳��Ă��܂���I");
        }
    }

    void CreateStage()
    {
        Vector3 position = Vector3.zero;
        LoadCSV();
        int lenY = map.GetLength(0);
        int lenX = map.GetLength(1);
        for (int x = 0; x < lenX; x++)
        {
            position.x = x;
            for (int y = 0; y < lenY; y++)
            {
                position.y = -y + 5;
                if (map[y, x] == (int)Stage.Block)
                {
                    Instantiate(block, position, Quaternion.identity);
                }
            }
        }
    }
}
