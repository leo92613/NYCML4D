using UnityEngine;
using System.Collections;
using FRL.IO;
using System;
using UnityEngine.SceneManagement;

namespace FRL.IO.FourD
{
    public class scenemanager : GlobalReceiver, IGlobalApplicationMenuPressDownHandler
    {
        public void OnGlobalApplicationMenuPressDown(ViveControllerModule.EventData eventData)
        {
            int scene_index;
            scene_index = SceneManager.GetActiveScene().buildIndex;
            scene_index = (scene_index + 1) % 3;
            SceneManager.LoadScene(scene_index);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}