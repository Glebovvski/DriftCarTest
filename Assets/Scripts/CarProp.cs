using UnityEngine;

public class CarProp : MonoBehaviour
{
    [field: SerializeField] public CarKey Car;
    public void Toggle(bool value) => gameObject.SetActive(value);
    
    public float rotationSpeed = 200f;  // Speed of rotation
    private Camera mainCamera;
    private bool isDragging = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime);
        }

        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    isDragging = true;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                float touchX = touch.deltaPosition.x;
                transform.Rotate(Vector3.up, -touchX * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
