using UnityEngine;
using System.Collections;
using FRL.IO;
using System;
using UnityEngine.SceneManagement;


namespace FRL.IO.FourD
{
    public class ObjManager :  GlobalReceiver, IGlobalApplicationMenuPressDownHandler, IGlobalTouchpadPressDownHandler
    {
        public GameObject[] objs;

        private int obj_index;
        public void OnGlobalApplicationMenuPressDown(ViveControllerModule.EventData eventData)
        {
            int scene_index;
            scene_index = SceneManager.GetActiveScene().buildIndex;
            scene_index = (scene_index + 1) % 3;
            SceneManager.LoadScene(scene_index);
        }

        public void OnGlobalTouchpadPressDown(ViveControllerModule.EventData eventData)
        {
            objs[obj_index].SetActive(false);
            obj_index = (obj_index + 1) % 3;
            objs[obj_index].SetActive(true);
        }

        // Use this for initialization
        void Start()
        {
            obj_index = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
