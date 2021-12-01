using System;
using UnityEngine;

public class ExampleUnityScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    [SerializeField] private string playerName;

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private uint health = 100;
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

    private void OnEnable()
    {
        InputScript.DoMove += GetInput;
    }

    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
    }

    private void GetInput()
    {
        InputVector = InputScript.MoveCtx().ReadValue<Vector2>();
    }

    private void Movement()
    {
        rig2d.velocity = InputVector * moveSpeed;
    }

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