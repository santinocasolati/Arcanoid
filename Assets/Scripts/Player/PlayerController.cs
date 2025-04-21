using UnityEngine;

public class PlayerController : ManagedUpdateBehaviour
{
    [SerializeField] private float moveSpeed, minX, maxX;

    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
    }

    public override void CustomUpdate(float deltaTime)
    {
        float moveDirection = inputs.Player.Move.ReadValue<float>();

        if (moveDirection != 0)
        {
            Vector3 pos = transform.position;

            pos.x += moveSpeed * moveDirection * deltaTime;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);

            transform.position = pos;
        }
    }
}
