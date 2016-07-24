using UnityEngine;

namespace Game
{
    class UFOEnergyBlast : MonoBehaviour
    {
        private const float SPEED = 13.0f;
        private Vector3 _target;

        public GameObject DestroyedPartPrefab;

        private void Start()
        {
            // Grab the player position immediately; let's hope they move!
            _target = GameController.instance.GameEnv.PlayerShip.transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards( transform.position, _target, SPEED * Time.deltaTime );

            if ( transform.position.y <= GameConstants.SCREEN_Y_FLOOR ) {
                Destroy( this.gameObject );
            }
        }

        private void OnTriggerEnter( Collider col )
        {
            if ( col.CompareTag( "Player" ) ) {
                GameController.instance.KillPlayer();
                Destroy( this.gameObject );
            } else if (col.CompareTag("PlayerShot")) {
                Instantiate( DestroyedPartPrefab, this.transform.position, Quaternion.identity );
                Destroy( this.gameObject );
                col.gameObject.GetComponent<Laserbeam>().Die();
            }
        }
    }
}
