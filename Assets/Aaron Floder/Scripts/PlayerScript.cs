using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    [SerializeField] private string playerName;

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] protected uint health = 100;
    private uint currentHealth;

    private Vector2 InputVector = Vector2.zero;

    public static Action InitTakeDamage;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = health;
    }

    //Subscribes the GetInput function to DoMove event
    private void OnEnable()
    {
        InputScript.DoMove += GetInput;
    }

    //UnSubscribes the GetInput function
    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Movement(); //If using Rigidbody
    }

    private void GetInput()
    {
        InputVector = InputScript.MoveCtx().ReadValue<Vector2>();
    }

    //The method takes care of player movement
    private void Movement()
    {
        rig2d.velocity = InputVector * moveSpeed;
    }

    //Call this method to reduce the amount health
    private void TakeDamage()
    {
        currentHealth -= 10;
        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die() => Destroy(gameObject);
}