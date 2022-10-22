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
    void FixedUpdate()
    {
        var movement = Vector3.zero;

        if (controls.top)
        {
            movement.y = 1;
        }

        if (controls.bottom)
        {
            movement.y = -1;
        }

        if (controls.left)
        {
            movement.x = -1;
        }

        if (controls.right)
        {
            movement.x = 1;
        }

        transform.Translate(movement * speed * Time.deltaTime);
    }
}
