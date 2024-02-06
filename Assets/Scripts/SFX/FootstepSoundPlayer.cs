using System;
using UnityEngine;

namespace SFX
{
    public class FootstepSoundPlayer : MonoBehaviour
    {
        public AudioClip grass;
        public AudioClip sand;
        public AudioClip bridge;
        public float volume;
        public float rayLength;
        public LayerMask groundLayer;

        private RaycastHit hit;

        public void Footstep()
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, rayLength, groundLayer))
            {
                if (hit.collider.CompareTag("Grass"))
                {
                    AudioManager.instance.PlaySound(grass, volume);
                }
                if (hit.collider.CompareTag("Sand"))
                {
                    AudioManager.instance.PlaySound(sand, volume);
                }
                if (hit.collider.CompareTag("Bridge"))
                {
                    AudioManager.instance.PlaySound(bridge, volume);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + rayLength * -transform.up);
        }
    }
}
