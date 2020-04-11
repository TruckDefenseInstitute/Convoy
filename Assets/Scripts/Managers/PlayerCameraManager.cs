using UnityEngine;

public class PlayerCameraManager : Manager<PlayerCameraManager> {
    static readonly string _defaultCameraName = "Player Camera";
    public string cameraName = "Player Camera";
    public GameObject CameraDolly;
    public GameObject CameraAngleAxis;
    public GameObject CameraRig;
    [HideInInspector]
    public GameObject CameraGameObject;
    [HideInInspector]
    public Camera Camera;
    
    public float _xMinimapPos = 0;
    public float _yMinimapPos = 0;
    public float _zMinimapPos = 0;
    [SerializeField]
    private float _miniMapSize = 50;

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit = new Vector2(40f, 30f); // To be replaced by map width and length

    public float scrollSpeed = 20f;
    public float minY = 0f;
    public float maxY = 60f;
    public float minPan = 45f;
    public float maxPan = 70f;

    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;

    bool _middleMousePanning;
    Vector3 _middleMousePanningStartCoords;
    GameObject _minimapBox;
    LineRenderer _minimapBoxRenderer;

    readonly Vector3[] _corners = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };

    void Start() {
        CameraGameObject = GameObject.Find(cameraName == null ? _defaultCameraName : cameraName);
        GameObject.Find("MinimapCamera")
            .GetComponent<MinimapCameraController>()
            .SetMinimapCameraLocation(_xMinimapPos, _yMinimapPos, _zMinimapPos, _miniMapSize);
        Camera = CameraGameObject.GetComponent<Camera>();

        _minimapBox = new GameObject();
        var renderer = _minimapBox.AddComponent<SpriteRenderer>();
        var material = renderer.material;
        Destroy(renderer);
        _minimapBoxRenderer = _minimapBox.AddComponent<LineRenderer>();
        _minimapBoxRenderer.positionCount = 4;
        _minimapBoxRenderer.loop = true;
        _minimapBoxRenderer.material = material;
        _minimapBox.layer = LayerMask.NameToLayer("Minimap");
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = CameraRig.transform.position;

        if (RTSUnitManager.Instance.InPannableControlState()) {
            if (Input.GetKeyDown(KeyCode.Mouse2)) {
                _middleMousePanning = true;
                _middleMousePanningStartCoords = Input.mousePosition;
            }
            if (Input.GetKeyUp(KeyCode.Mouse2)) {
                _middleMousePanning = false;
            }

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

            if (_middleMousePanning) {
                pos += CameraRig.transform.forward * Time.deltaTime * panSpeed * (Input.mousePosition - _middleMousePanningStartCoords).y / Screen.height * 5.0f;
                pos += CameraRig.transform.right * Time.deltaTime * panSpeed * (Input.mousePosition - _middleMousePanningStartCoords).x / Screen.width * 5.0f;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            var zoom = CameraDolly.transform.localPosition;
            zoom.z += scroll * scrollSpeed * 100f * Time.deltaTime;
            zoom.z = Mathf.Clamp(zoom.z, minY, maxY);
            CameraDolly.transform.localPosition = zoom;

            pos.x = Mathf.Clamp(pos.x, _xMinimapPos - panLimit.x, _xMinimapPos + panLimit.x);
            pos.z = Mathf.Clamp(pos.z, _zMinimapPos - panLimit.y, _zMinimapPos + panLimit.y);

            CameraRig.transform.position = pos;

            var rot = CameraRig.transform.localEulerAngles;
            rot.y += Input.GetKey(KeyCode.LeftBracket) ? Time.deltaTime * 180 : Input.GetKey(KeyCode.RightBracket) ? Time.deltaTime * -180 : 0;
            CameraRig.transform.localEulerAngles = rot;

            var angle = CameraAngleAxis.transform.localEulerAngles;
            angle.x += Input.GetKey(KeyCode.PageUp) ? Time.deltaTime * 60 : Input.GetKey(KeyCode.PageDown) ? Time.deltaTime * -60 : 0;
            angle.x = Mathf.Clamp(angle.x, minPan, maxPan);
            CameraAngleAxis.transform.localEulerAngles = angle;

            // draw minimap box
            int i = 0;
            foreach (var point in _corners) {
                Ray p = Camera.ViewportPointToRay(point);
                RaycastHit hit;
                Physics.Raycast(p, out hit, 10000f, LayerMask.GetMask("Ground"));
                Vector3 drawPoint = hit.point;
                drawPoint.y = 45;
                _minimapBoxRenderer.SetPosition(i++, drawPoint);
            }

        }

        if(Input.GetKey(KeyCode.Space)) {
            SetCameraPosition(RTSUnitManager.Instance.truck.transform.position);
        }
    }

    public void SetCameraPosition(Vector3 position) {
        CameraRig.transform.position = new Vector3(position.x, CameraRig.transform.position.y, position.z);
    }
}