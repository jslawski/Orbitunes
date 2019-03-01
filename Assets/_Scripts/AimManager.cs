using UnityEngine;
using UnityEngine.EventSystems;

public class AimManager : MonoBehaviour, IPointerDownHandler 
{

	public static AimManager instance;

	private GameObject asteroidGeneratorPrefab;
    private GameObject slingShotPrefab;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
        this.slingShotPrefab = Resources.Load("SlingShot") as GameObject;
		this.asteroidGeneratorPrefab = Resources.Load("AsteroidGenerator") as GameObject;
	}

	public void OnPointerDown(PointerEventData data)
	{
        if (GameManager.instance.IsClickingAStar() == true)
        {
            return;
        }

        this.InstantiateSlingShot();
	}

    private void InstantiateSlingShot()
    {
        Vector2 instantiationPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        GameObject currentSlingShot = Instantiate(this.slingShotPrefab, instantiationPoint, new Quaternion()) as GameObject;
        SlingShot slingShot = currentSlingShot.GetComponent<SlingShot>();
        slingShot.OnSlingShotReleased += CreateAsteroidGenerator;
    }

	private void CreateAsteroidGenerator(LaunchValues launchValues)
	{
		GameObject newAsteroidGenerator = Instantiate(this.asteroidGeneratorPrefab, launchValues.startPoint, new Quaternion()) as GameObject;
		AsteroidGenerator generator = newAsteroidGenerator.GetComponent<AsteroidGenerator>();
		generator.SetupAsteroidGenerator(launchValues);
    }
}
