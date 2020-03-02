using UnityEngine;

public class PlayerCameraManager : Manager<PlayerCameraManager> {
    static readonly string _defaultCameraName = "Player Camera";
    public string cameraName = "Player Camera";
    public GameObject CameraDolly;
    public GameObject CameraAngleAxis;
    public GameObject CameraRig;
    GameObject _cameraGameObject;

    [SerializeField]
    private float _xMinimapPos = 0;
    [SerializeField]
    private float _yMinimapPos = 0;
    [SerializeField]
    private float _zMinimapPos = 0;
    [SerializeField]
    private float _miniMapSize = 50;

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit = new Vector2(40f, 30f); // To be replaced by map width and length

    public float scrollSpeed = 20f;
    public float minY = 0f;
    public float maxY = 60f;

    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;


    void Start() {
        _cameraGameObject = GameObject.Find(cameraName == null ? _defaultCameraName : cameraName);
        GameObject.Find("MinimapCamera")
            .GetComponent<MinimapCameraController>()
            .SetMinimapCameraLocation(_xMinimapPos, _yMinimapPos, _zMinimapPos, _miniMapSize);
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = CameraRig.transform.position;

        if (UnitControlAndSelectionManager.Instance.InPannableControlState()) {
            if (Input.GetKey(up) || Input.mousePosition.y >= Screen.height - panBorderThickness) {
                pos += CameraRig.transform.forward * Time.deltaTime * panSpeed;
            }

            if (Input.GetKey(left) || Input.mousePosition.x <= panBorderThickness) {
                pos += -CameraRig.transform.right * Time.deltaTime * panSpeed;
            }

            if (Input.GetKey(down) || Input.mousePosition.y <= panBorderThickness) {
                pos += -CameraRig.transform.forward * Time.deltaTime * panSpeed;
            }

            if (Input.GetKey(right) || Input.mousePosition.x >= Screen.width - panBorderThickness) {
                pos += CameraRig.transform.right * Time.deltaTime * panSpeed;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            var zoom = CameraDolly.transform.localPosition;
            zoom.z += scroll * scrollSpeed * 100f * Time.deltaTime;
            zoom.z = Mathf.Clamp(zoom.z, minY, maxY);
            CameraDolly.transform.localPosition = zoom;

            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            CameraRig.transform.position = pos;

            var rot = CameraRig.transform.localEulerAngles;
            rot.y += Input.GetKey(KeyCode.O) ? Time.deltaTime * 180 : Input.GetKey(KeyCode.P) ? Time.deltaTime * -180 : 0;
            CameraRig.transform.localEulerAngles = rot;

            var angle = CameraAngleAxis.transform.localEulerAngles;
            angle.x += Input.GetKey(KeyCode.PageUp) ? Time.deltaTime * 60 : Input.GetKey(KeyCode.PageDown) ? Time.deltaTime * -60 : 0;
            angle.x = Mathf.Clamp(angle.x, 10, 70);
            CameraAngleAxis.transform.localEulerAngles = angle;
        }
    }

    public void SetCameraPosition(Vector3 position) {
        CameraRig.transform.position = new Vector3(position.x, CameraRig.transform.position.y, position.z);
    }
}