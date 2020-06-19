using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadTable : MonoBehaviour
{
    [SerializeField]
    private Text tableTitle;

    [SerializeField]
    private Transform table;

    [SerializeField]
    private GameObject columnPrefab, rowPrefab;

    void Start()
    {
        loadFile();
    }

    private void loadFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "JsonChallenge.json");

        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd().Trim();
            reader.Close();

            JSONObject jsonObject = new JSONObject(json);
            JSONObject title = jsonObject["Title"];
            tableTitle.text = title.str;

            JSONObject columns = jsonObject["ColumnHeaders"];
            JSONObject data = jsonObject["Data"];

            GridLayoutGroup gridLayoutGroup = table.gameObject.GetComponent<GridLayoutGroup>();

            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = columns.list.Count;

            for (int j = 0; j < columns.list.Count; j++)
            {
                GameObject tableColumn = Instantiate(columnPrefab, table);

                GameObject columnHeader = Instantiate(rowPrefab, tableColumn.transform);
                Text columnTitle = columnHeader.GetComponent<Text>();
                columnTitle.text = columns[j].str;
                columnTitle.fontStyle = FontStyle.Bold;
                columnTitle.fontSize = 20;

                for (int i = 0; i < data.list.Count; i++)
                {
                    JSONObject obj = data[i];
                    JSONObject row = obj[j];
                    GameObject tableRow = Instantiate(rowPrefab, tableColumn.transform);
                    tableRow.GetComponent<Text>().text = row.str;
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load can't find JsonChallenge.json file");
        }
    }

    public void reloadFile()
    {
        foreach (Transform child in table)
        {
            GameObject.Destroy(child.gameObject);
        }

        loadFile();
    }
}
