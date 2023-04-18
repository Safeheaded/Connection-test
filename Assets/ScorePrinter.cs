using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScorePrinter : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI winner;
    // Start is called before the first frame update
    void Start()
    {
        winner.text = string.Format("{0} won!", PlayerPrefs.GetString("Winner"));
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
