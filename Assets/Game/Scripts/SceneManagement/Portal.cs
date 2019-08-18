using System.Collections;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


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
            
            
            wrapper.Save();
            
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            wrapper.Load();
            
            //change location of player to other portal's spawn point
            Portal otherPortal = GetOtherPortal();
            UpdatePlayerLocation(otherPortal);
            
           
            
            //time to wait while screen is faded out
            yield return new WaitForSeconds(betweenFadeTime);
            wrapper.Save();
            
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

        private void UpdatePlayerLocation(Portal otherPortal)
        {
            MainPlayer.Instance.gameObject.GetComponent<NavMeshAgent>().enabled  = false;
            
            //change players location on load to pos of spawnPoint 
            MainPlayer.Instance.transform.position = otherPortal.spawnPoint.position;
            MainPlayer.Instance.transform.rotation = otherPortal.spawnPoint.rotation;
           
            MainPlayer.Instance.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            
        }

       
        
    }
}