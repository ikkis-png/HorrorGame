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
    [SerializeField] bool hasAIListener = false;
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
    

    public void UpdateFootstep(float speed, bool running = false)
    {
        footstepTimer -= Time.deltaTime * speed * 5;
        if (footstepTimer <= 0)
        {
            if (hasAIListener && running) AlertWithSound();
            audioSource.pitch =  pitchBase + Random.value * .1f;
            int rnd = Random.Range(0, footsteps.Length);
            audioSource.PlayOneShot(footsteps[rnd], footstepVolume);
            footstepTimer = footstepTime;
        }
    }
    private void AlertWithSound()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3, Vector3.up);
        foreach (RaycastHit h in hits)
        {
            IListener l = h.transform.GetComponent<IListener>();
            if (l != null) l.HearSound(1, transform.position);
        }
    }
}
