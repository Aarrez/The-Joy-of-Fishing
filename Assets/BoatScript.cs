using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [SerializeField] private float boatSpeed = 3f;

    private Rigidbody2D rig2d;

    private float inputValueX = 0f;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputScript.MoveLine += GetInput;
    }

    private void OnDisable()
    {
        InputScript.MoveLine -= GetInput;
    }

    private void FixedUpdate()
    {
        MoveLeftRight();
    }

    private void GetInput()
    {
        inputValueX = InputScript.LineCtx().ReadValue<float>();
        Debug.Log(inputValueX);
    }

    private void MoveLeftRight()
    {
        rig2d.velocity = new Vector2((inputValueX * boatSpeed * Time.fixedDeltaTime), 0f);
    }
}