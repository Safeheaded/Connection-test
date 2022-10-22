using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;



public class Player : MonoBehaviour
{

    [SerializeField]
    float speed = .1f;

    public class Controls
    {
        public bool top { get; set; } = false;
        public bool left { get; set; } = false;
        public bool right { get; set; } = false;
        public bool bottom { get; set; } = false;
    }

    public int id { get; set; }
    public string userName { get; set; }
    public Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.right)
        {
            //transform.position += new Vector3(0, .05f, 0) * Time.deltaTime;
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            print("moving");
        }
    }
}
