using UnityEngine;

public class ShooterGuard : MonoBehaviour
{
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private GameObject playerObject;
    private float eggReloadTimer = 0;
	private GuardBehavior behavior = null;

	void Start()
	{
		behavior = GetComponent<GuardBehavior>();
	}

    void Update()
    {
		if (behavior.currentState == GuardBehavior.GuardState.Chase)
		{
			eggReloadTimer += Time.deltaTime;
			if (eggReloadTimer > 0.2f)
			{
				eggReloadTimer = 0;
				Instantiate(eggPrefab,
						transform.position,
						// Quaternion.AxisAngle(Mathf.Atan2(transform.position.x, transform.position.y))
						new Quaternion(0,0,Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f))
						// 0
						// transform.position.angleTo playerObject.position

					).AddComponent<Egg>();
			}
		}

		else
		{
			eggReloadTimer = 0;
		}
    }
}
