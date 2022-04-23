using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildController : MonoBehaviour
{
    [SerializeField] private Camera buildCamera;
    private GameObject activeIndicator;
    [SerializeField] private GameObject mouseIndicatorHighlight;
    [SerializeField] private GameObject iceTowerIndicator;
    [SerializeField] private LayerMask placeableLayerMask; // set to Ground layer so can only place structures on ground
    [SerializeField] private LayerMask structureLayerMask;

    [SerializeField] private GameObject cameraControllerObj;    // reference to GameObject that holds CameraController script
    private CameraController cameraController;  // reference to CameraController script
    private MouseIndicatorController mouseCon;

    [SerializeField] private GameObject currencyContainer;
    private CurrencyController currencyController;

    [SerializeField] private int testCost;
    
    [Header("Structures")]
    public GameObject iceTower;
    public GameObject fireTower;
    public GameObject grassTower;
    private GameObject activeStructure;

    [Header("UI")]
    public Canvas buildCanvas;
    private int activeSlot;
    public Image Slot1;
    public Image Slot2;
    public Image Slot3;
    public TMP_Text modeText;
    public TMP_Text poorText;
    public TMP_Text towerNameText;
    public TMP_Text towerADText;
    public TMP_Text towerASText;
    public TMP_Text towerRangeText;
    public GameObject selectedTower;

    private Color32 defaultColor = new Color32(255, 255, 255, 255);
    private Color32 activeColor = new Color32(115, 255, 128, 255);

    [Header("Camera Controls")]
    public float camSpeed = 20f;
    public float scrollSpeed = 25f;
    public float panBorder = 10f;
    public Vector2 panLimit;
    private float scroll;
    public float minY = 20f;
    public float maxY = 80f;

    [Header("Animations")]
    public Animator towerStats;

    enum BuildMode
    {
        PLACE,
        DELETE
    }

    private bool inBuild = false;
    private BuildMode buildMode = BuildMode.PLACE;

    private void Start()
    {
        testCost = 200;

        activeStructure = iceTower; // default tower selected
        activeSlot = 1; // default tower highlighted on HUD
        activeIndicator = iceTowerIndicator;
        UpdateSlotsUI();
        mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
        cameraController = cameraControllerObj.GetComponent<CameraController>();
        currencyController = currencyContainer.GetComponent<CurrencyController>();
    }

    private void Update()
    {
        ToggleBuild(); // press tab to toggle build mode

        if (inBuild)
        {
            ToggleMode();
            MoveCam();
            BuildUIControl();
            buildCanvas.gameObject.SetActive(true);
            modeText.gameObject.SetActive(true);

            if (buildMode == BuildMode.PLACE)
            {
                activeIndicator.SetActive(true);
                MouseIndicator();
                StructurePlacement();
            }
            if (buildMode == BuildMode.DELETE)
            {
                StructureDeletion();
                activeIndicator.GetComponent<MeshRenderer>().enabled = false;
            }
        } else
        {
            activeIndicator.SetActive(false);
            buildCanvas.gameObject.SetActive(false);
            modeText.gameObject.SetActive(false);
            mouseCon.ClearCollisions();
        }
    }

    private void MoveCam()
    {
        Vector3 pos = buildCamera.transform.position; // current camera position
                                                      //Quaternion rot = buildCamera.transform.rotation;

        //if (Input.GetKey(KeyCode.Mouse1))
        //{
        //    if (Input.GetAxis("Mouse X") < 0)
        //    {
        //        print("Left");
        //        rot.y -= 1.0f * Time.deltaTime;
        //    }
        //    if (Input.GetAxis("Mouse X") > 0)
        //    {
        //        print("Right");
        //        rot.y += 1.0f * Time.deltaTime;
        //    }
        //    if (Input.GetAxis("Mouse Y") < 0)
        //    {
        //        print("Down");
        //        rot.x += 1.0f * Time.deltaTime;
        //    }
        //    if (Input.GetAxis("Mouse Y") > 0)
        //    {
        //        print("Up");
        //        rot.x -= 1.0f * Time.deltaTime;
        //    }
        //} else
        //{
        //    // use WASD or mouse to pan camera
        //    if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorder)
        //    {
        //        pos.z += camSpeed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorder)
        //    {
        //        pos.z -= camSpeed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorder)
        //    {
        //        pos.x += camSpeed * Time.deltaTime;
        //    }

        //    if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorder)
        //    {
        //        pos.x -= camSpeed * Time.deltaTime;
        //    }
        //}
        // use WASD or mouse to pan camera
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorder)
        {
            pos.z += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorder)
        {
            pos.z -= camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorder)
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorder)
        {
            pos.x -= camSpeed * Time.deltaTime;
        }

        scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x); // limit area you can pan to (can't move camera forever)
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        buildCamera.transform.position = pos; // update with new camera position
        //buildCamera.transform.rotation = rot;
    }

    private void ToggleBuild()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuild = !inBuild;
            cameraController.toggleCamera();
            cameraController.toggleMouseLock();
            cameraController.toggleCanvas();
        }
    }

    private void ToggleMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (buildMode == BuildMode.PLACE)
            {
                buildMode = BuildMode.DELETE;
                modeText.text = "REMOVING";
            } else if (buildMode == BuildMode.DELETE)
            {
                buildMode = BuildMode.PLACE;
                modeText.text = "PLACING";
            }
        }
    }

    private void MouseIndicator()
    {
        switch (activeSlot)
        {
            case 1:
                mouseIndicatorHighlight.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 2:
                iceTowerIndicator.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 3:
                iceTowerIndicator.GetComponent<MeshRenderer>().enabled = false;
                break;
            default:
                break;
        }

        activeIndicator.GetComponent<MeshRenderer>().enabled = true;
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, placeableLayerMask))
        {
            transform.position = raycastHit.point;
        }
        else
        {
            activeIndicator.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void StructurePlacement()
    {
        // use numbers to swap between towers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeStructure = iceTower;
            activeSlot = 1;
            UpdateSlotsUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeStructure = fireTower;
            activeSlot = 2;
            UpdateSlotsUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeStructure = grassTower;
            activeSlot = 3;
            UpdateSlotsUI();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseCon.canPlace)
        {
            if (currencyController.checkSufficientMoney(testCost))
            {
                Instantiate(activeStructure, transform.position, Quaternion.identity); // place tower at mouse location
                currencyController.removeMoney(testCost);
                poorText.enabled = false;
            } else {
                poorText.enabled = true;
                StartCoroutine(wait(3));
            }
        }
    }

    private void StructureDeletion()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, structureLayerMask))
            {
                mouseCon.RemoveStructureFromCollisions(raycastHit.collider.gameObject);
                if (raycastHit.collider.gameObject == selectedTower)
                {
                    selectedTower = null;
                    towerStats.SetBool("isSelected", false);
                    towerStats.SetTrigger("deselect");
                }
                Destroy(raycastHit.collider.gameObject);   
            }
        }
    }

    private void BuildUIControl()
    {
        // select and unselected towers
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, structureLayerMask))
            {
                if (selectedTower != null && selectedTower != raycastHit.transform.gameObject)
                {
                    selectedTower.GetComponent<TowerController>().SetSelected(false); // set previously selected tower to unselected
                    selectedTower = raycastHit.transform.gameObject;
                    selectedTower.GetComponent<TowerController>().SetSelected(true);
                    towerStats.SetBool("isSelected", true);
                } else
                {
                    selectedTower = raycastHit.transform.gameObject;
                    selectedTower.GetComponent<TowerController>().SetSelected(true);
                    towerStats.SetBool("isSelected", true);
                }
            } else
            {
                if (selectedTower != null)
                {
                    selectedTower.GetComponent<TowerController>().SetSelected(false);
                    selectedTower = null;
                    towerStats.SetBool("isSelected", false);
                    towerStats.SetTrigger("deselect");
                }
            }
        }

        if (selectedTower != null)
        {
            towerNameText.text = selectedTower.name;
            towerADText.text = "AD: " + selectedTower.GetComponent<TowerController>().damage.ToString();
            towerASText.text = "AS: " + selectedTower.GetComponent<TowerController>().fireRate.ToString();
            towerRangeText.text = "Range: " + selectedTower.GetComponent<TowerController>().range.ToString();
        } else
        {
            towerNameText.text = "";
            towerADText.text = "";
            towerASText.text = "";
            towerRangeText.text = "";
        }
    }

    private void UpdateSlotsUI()
    {
        switch (activeSlot)
        {
            case 1:
                activeIndicator = iceTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                Slot1.color = activeColor;
                Slot2.color = defaultColor;
                Slot3.color = defaultColor;
                break;
            case 2:
                activeIndicator = mouseIndicatorHighlight;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                Slot1.color = defaultColor;
                Slot2.color = activeColor;
                Slot3.color = defaultColor;
                break;
            case 3:
                activeIndicator = mouseIndicatorHighlight;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                Slot1.color = defaultColor;
                Slot2.color = defaultColor;
                Slot3.color = activeColor;
                break;
            default:
                break;
        }
    }

    private IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        poorText.enabled = false;
    }
    public bool getInBuild()
    {
        return inBuild;
    }
}
