using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


[System.Serializable]
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
    public Animator animator;
    private SpriteRenderer renderer;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
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
            renderer.flipX= true;
        }

        if (controls.right)
        {
            movement.x = 1;
            renderer.flipX= false;
        }

        if(movement == Vector3.zero)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }

        transform.Translate(movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score += 10;
        Destroy(collision.gameObject);
        GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0} - {1}", userName, score);
    }
}
