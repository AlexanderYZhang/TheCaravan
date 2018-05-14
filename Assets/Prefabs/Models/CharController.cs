using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharController : MonoBehaviour {
    public GameObject car;
    public GameObject canvas;
    public GameObject marker;
    public NavMeshAgent agent;
    public Inventory inventory;

    private Interactable focus;
    private CarController carController;
    private CameraController cameraController;

    // Character Parameter
    private Animator anim;
    private bool placed;
    private Text[] texts;
    private GameObject message;

    // Vehicle Check
    public bool closeToVehicle;
    private bool inVehicle;
    private bool placeMode;
    private int selectedTurretCode = 0;
    private GameObject turretCreation;

    private int groundLayer = 1 << 8;

    new Camera camera;
    public PlayerMotor motor;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");

        carController = car.GetComponent<CarController>();
        camera = Camera.main;
        cameraController = camera.GetComponent<CameraController>();
        motor = GetComponent<PlayerMotor>();

        placed = false;
        texts[0].text = "";
        message.SetActive(false);

        closeToVehicle = false;
        inVehicle = false;

        marker.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == marker.name) {
            marker.SetActive(false);
        }
    }

    void setEnterText() {
        texts[0].text = "Press E to Enter";
        message.SetActive(true);
        closeToVehicle = true;
    }

    void removeEnterText() {
        texts[0].text = "";
        message.SetActive(false);
        closeToVehicle = false;

    }

    public void EnterVehicle() {
        texts[0].text = "Press E to Exit";
        inVehicle = true;
        cameraController.target = car.transform;
        marker.transform.position = new Vector3(car.transform.position.x, 0, car.transform.position.z);
        carController.setOffset(transform.position);
        carController.agent.enabled = true;
        gameObject.SetActive(false);
    }

    public void ExitVehicle(Vector3 newPos) {
        inVehicle = false;
        transform.position = newPos;
        cameraController.target = transform;
        marker.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        carController.agent.enabled = false;
        gameObject.SetActive(true);
    }

    public bool InsideVehicle() {
        return inVehicle;
    }

    private void ChangeObjectMaterial(GameObject obj, Material mat) {
        obj.GetComponent<Renderer>().material = mat;
        Renderer[] children = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children) {
            rend.material = mat;
        }
    }

    // Update is called once per frame
    void Update () {
        if (closeToVehicle) {
            setEnterText();
            if (Input.GetKeyDown(KeyCode.E)) {
                EnterVehicle();
            }
        } else {
            removeEnterText();
        }
        if (!inVehicle) {
            bool running = Input.GetKey(KeyCode.LeftShift);
            if (!placeMode && Input.GetKeyDown(KeyCode.Alpha1)) {
                placeMode = true;
                turretCreation = null;
                motor.StopMoveToPoint();
                marker.SetActive(false);
            } else if (placeMode && Input.GetKeyDown(KeyCode.Escape)) {
                placeMode = false;
                DestroyObject(turretCreation);
                turretCreation = null;
                marker.SetActive(true);
            }
            if (placeMode) {
                Inventory.TurretData data = inventory.GetTurretData(selectedTurretCode);
                if (turretCreation == null) {
                    Quaternion rot = new Quaternion {
                        eulerAngles = new Vector3(-90, 0, 0)
                    };
                    turretCreation = Instantiate(
                        data.prefab,
                        marker.transform.position,
                        rot
                    );
                    ChangeObjectMaterial(turretCreation, data.seeThrough);
                    turretCreation.GetComponent<TurretController>().notPlaced = true;
                    turretCreation.GetComponent<SphereCollider>().enabled = false;
                }

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                int layerMask = groundLayer;
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                    turretCreation.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                    if (turretCreation.GetComponent<TurretController>().Overlapping() && 
                        turretCreation.GetComponent<Renderer>().material != data.seeThroughError) {
                        ChangeObjectMaterial(turretCreation, data.seeThroughError);
                    } else if (turretCreation.GetComponent<Renderer>().material != data.seeThrough) {
                        ChangeObjectMaterial(turretCreation, data.seeThrough);
                    }
                }

                if (Input.GetMouseButtonDown(0) && inventory.EnoughForTurret(selectedTurretCode) &&
                    !turretCreation.GetComponent<TurretController>().Overlapping()) {
                    ChangeObjectMaterial(turretCreation, data.primary);
                    turretCreation.GetComponent<NavMeshObstacle>().enabled = true;
                    turretCreation.GetComponent<SphereCollider>().enabled = true;
                    turretCreation.GetComponent<TurretController>().notPlaced = false;
                    turretCreation.GetComponent<SphereCollider>().enabled = true;
                    inventory.AddTurret(1);
                    placeMode = false;
                } else if (!inventory.EnoughForTurret(0)) {
                    placeMode = false;
                }
                SetFocus(null);
            } else {
                if (focus != null && focus.IsInteracting()) {
                    GetComponent<PlayerMotor>().StopMoveToPoint();
                    GetComponent<PlayerMotor>().FaceTarget();
                }

                if (Input.GetMouseButton(0)) {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    int layerMask = groundLayer;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                        motor.MoveToPoint(hit.point);
                        marker.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                        marker.SetActive(true);
                        SetFocus(null);
                    }
                }
                if (Input.GetMouseButtonDown(1)) {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null) {
                            SetFocus(interactable);
                            marker.SetActive(false);
                        }
                    }

                }
            }

            agent.speed = running ? 15f : 5f;

            float animSpeedPct = 0;
            if (!motor.IsMoving() && focus != null && focus.IsInteracting() && focus is Resource) {
                animSpeedPct = 0.32f;
            } else if (motor.IsMoving() && running) {
                animSpeedPct = 1;
            } else if (motor.IsMoving()) {
                animSpeedPct = 0.67f;
            } else {
                animSpeedPct = 0;
            }

            anim.SetFloat("speedPct", animSpeedPct);
        }
    }

    public bool IsPlaced() {
        return placed;
    }

    public void SetPlaced() {
        if (!placed) {
            placed = true;
        }
    }

    public void SetFocus(Interactable newFocus)
    {
        if (focus != newFocus && focus != null)
        {
            focus.OnDefocused();
        }

        focus = newFocus;

        if (focus != null)
        {
            focus.OnFocused(transform);
        }
        //Debug.Log(newFocus);
        motor.FollowTarget(newFocus);
    }


    void OnDrawGizmos()
    {
        NavMeshPath path = agent.path;
        if (path != null)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

        }
    }
}
