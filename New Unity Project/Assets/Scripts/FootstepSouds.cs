using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSouds : MonoBehaviour
{
    [SerializeField] AudioClip[] footsteps;
    AudioSource audioSource;
    float footstepTimer = .5f;
    [SerializeField] float footstepTime = .5f;
    [SerializeField] float footstepVolume = .3f;

    [SerializeField] float pitchBase = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateFootstep(float speed)
    {
        footstepTimer -= Time.deltaTime * speed * 5;
        if (footstepTimer <= 0)
        {
            audioSource.pitch =  pitchBase + Random.value * .1f;
            int rnd = Random.Range(0, footsteps.Length);
            audioSource.PlayOneShot(footsteps[rnd], .3f);
            footstepTimer = footstepTime;
        }
    }
}
