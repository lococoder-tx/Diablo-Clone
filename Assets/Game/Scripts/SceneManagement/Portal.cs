using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//uses RPG.CORE
namespace RPG.SceneManagement
{
    
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        //[SerializeField] private CanvasGroup fadeCanvas;
        [SerializeField] private float fadeOutTime = 2f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float betweenFadeTime = 2f;
        
        
        //state var used for all portals
        private static bool isTransitioning = false;
        //private static IEnumerator transitionFunction;
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

       
        
        private void OnTriggerEnter(Collider other)
        {
            
            
            if (other.gameObject == MainPlayer.Instance.gameObject && !isTransitioning)
            {
                StartCoroutine(Transition());
                isTransitioning = true;
            }
        }

        private IEnumerator Transition()
        {
            //prevent any transitions from happening 
            DontDestroyOnLoad(this.gameObject);
            
            
            //fader will persist throughout scenes since it is part of persistentObjects 
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            
            yield return fader.FadeOut(fadeOutTime);
            
            //set player to inactive so he cannot be controlled during trans
            MainPlayer.Instance.gameObject.SetActive(false);
            wrapper.Save();
            
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            wrapper.Load();
           
            
            //change location of player to other portal's spawn point
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(betweenFadeTime);
            
            
            MainPlayer.Instance.gameObject.SetActive(true);
            yield return fader.FadeIn(fadeInTime);
            
            
            isTransitioning = false;
            Destroy(this.gameObject);
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
           
            foreach (var portal in portals)
            {
                if (portal != this && portal.destination == this.destination)
                {
                    return portal;
                }
            }

            
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            //change players location on load to pos of spawnPoint 
            MainPlayer.Instance.transform.position = otherPortal.spawnPoint.position;
            MainPlayer.Instance.transform.rotation = otherPortal.spawnPoint.rotation;
            
            
        }

       
        
    }
}