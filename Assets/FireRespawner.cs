using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireRespawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Collect collect;
    private BoxCollider2D collider;
    private bool wasStarted = false;
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    IEnumerator RespNewCollect()
    {
        while (true)
        {
            float randomX = Random.Range(-8.48f, 8.48f);
            float randomY = Random.Range(-4.6f, 4.6f);
            Instantiate(collect, new Vector3(randomX, randomY, -5), Quaternion.identity);
            float randomSeconds = Random.Range(4, 8);
            yield return new WaitForSeconds(randomSeconds);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !wasStarted)
        {
            StartCoroutine(RespNewCollect());
            wasStarted = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("Scores");
        }
    }
}
