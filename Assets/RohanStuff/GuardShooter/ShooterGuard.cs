using UnityEngine;
using UnityEngine.AI;

public class ShooterGuard : MonoBehaviour
{
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private LayerMask bulletLayer;
    // [SerializeField] private Sprite eggPrefab;
	private Vector3 playerPos = Vector3.zero;
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
				playerPos = behavior.GetPlayerPos();
				eggReloadTimer = 0;
                GameObject egg = new()
                {
                    layer = (int)Mathf.Log(bulletLayer, 2)
                };
                egg.transform.position = transform.position;
				egg.transform.rotation = Quaternion.Euler(0,0,
						Mathf.Atan2(
							transform.position.y - playerPos.y,
							transform.position.x - playerPos.x
							)*Mathf.Rad2Deg + 90 + Random.Range(-30,30)
						);
				egg.transform.localScale *= .9f;
				egg.AddComponent<Egg>();
				egg.AddComponent<SpriteRenderer>().sprite = bulletSprite;
				// egg.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
				// egg.GetComponent<Rigidbody2D>().gravityScale = 0;
				egg.AddComponent<BoxCollider2D>().size *= .4f;
				// egg.AddComponent<NavMeshObstacle>();
			}
		}
		else
		{
			eggReloadTimer = 0;
		}
    }
}
