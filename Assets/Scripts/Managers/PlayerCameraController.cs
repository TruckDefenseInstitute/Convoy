using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    static readonly string _defaultCameraName = "Player Camera";
    public string cameraName = "Player Camera";
    GameObject _cameraGameObject;
    UnitControlAndSelectionManager _unitControlAndSelectionManager;

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit = new Vector2(40f, 30f); // To be replaced by map width and length

    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 60f;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;


    void Start()
    {
        _cameraGameObject = GameObject.Find(cameraName == null ? _defaultCameraName : cameraName);
        _unitControlAndSelectionManager = GetComponent<UnitControlAndSelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _cameraGameObject.transform.position;

        if (_unitControlAndSelectionManager.InPannableControlState())
        {
            if (Input.GetKey(up) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(left) || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(down) || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(right) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y - ((pos.y - minY) / Mathf.Sqrt(3f)), panLimit.y - ((pos.y - minY) / Mathf.Sqrt(3f)));

            _cameraGameObject.transform.position = pos;
        }
    }
}