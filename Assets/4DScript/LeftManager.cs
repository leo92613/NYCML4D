using UnityEngine;
using System.Collections;
using FRL.IO;
using System;

namespace FRL.IO.FourD { 
    public class LeftManager : GlobalReceiver, IGlobalTriggerHandler, IGlobalGripHandler, IGlobalApplicationMenuHandler {
        public GameObject rightcontroller;
        public Transform trackball;
        public bool buttondown = false;
        public GameObject root;

        private LineRenderer line;
        private Manager right;
        private RaycastHit hit;
        private GameObject tmp;
        private bool chosen;

        public void OnGlobalTriggerPress(ViveControllerModule.EventData eventData) {
            right.root.GetComponent<Renderer>().material = right.mat[0];
            Ray choose = new Ray(eventData.module.transform.position, eventData.module.transform.forward);
            if (Physics.Raycast(choose, out hit)) {
                if (hit.transform.gameObject != tmp) {
                    tmp.GetComponent<Renderer>().material = right.mat[0];
                    tmp = hit.transform.gameObject;
                }
                line.enabled = true;
                line.SetPosition(0, eventData.module.transform.position);
                line.SetPosition(1, hit.point);
                hit.transform.gameObject.GetComponent<Renderer>().material = right.mat[1];

                chosen = true;
            }
            else {
              
                chosen = false;
                line.enabled = true;
                line.SetPosition(0, eventData.module.transform.position);
                line.SetPosition(1, eventData.module.transform.position+1000* eventData.module.transform.forward);
            }
        }

        public void OnGlobalTriggerPressDown(ViveControllerModule.EventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerPressUp(ViveControllerModule.EventData eventData) {

            line.enabled = false;
            if (chosen) {
                hit.transform.gameObject.GetComponent<Renderer>().material = right.mat[0];
                right.root = hit.transform.gameObject;
                right.root.GetComponent<Renderer>().material = right.mat[1];
                right.Sethyperface();
                right.ball = trackball;
            } else {
                right.root.GetComponent<Renderer>().material = right.mat[1];
            }
            buttondown = false;
            right.buttondown = false;
        }

        public void OnGlobalTriggerTouch(ViveControllerModule.EventData eventData) {
            //throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchDown(ViveControllerModule.EventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveControllerModule.EventData eventData) {
           // throw new NotImplementedException();
        }
        // Use this for initialization
        void Awake() {
            right = rightcontroller.GetComponent<Manager>();
            line = GetComponent<LineRenderer>();
            line.SetVertexCount(2);
            line.SetWidth(0.001f, 0.005f);
            line.enabled = false;
            tmp = right.root;
        }

        // Update is called once per frame
        void Update() {

        }

        public void OnGlobalGripPressDown(ViveControllerModule.EventData eventData) {
            right.setscale();
            right.setleft(eventData.module.transform.position);
            right.setissalce(true);
        }

        public void OnGlobalGripPress(ViveControllerModule.EventData eventData) {
            right.setleft(eventData.module.transform.position);

        }

        public void OnGlobalGripPressUp(ViveControllerModule.EventData eventData) {
            right.setissalce(false);
            buttondown = false;
            right.buttondown = false;
        }

        public void OnGlobalApplicationMenuPressDown(ViveControllerModule.EventData eventData) {
            right.Explode();
            right.root = root;
            right.root.GetComponent<Renderer>().material = right.mat[1];
            right.hyperface.GetComponent<Hyperface>().center = right.root.GetComponent<Hypermesh>().center;
            right.hyperface.GetComponent<Hyperface>().Renew();
            //Application.LoadLevel(1);
        }

        public void OnGlobalApplicationMenuPress(ViveControllerModule.EventData eventData) {
          //  throw new NotImplementedException();
        }

        public void OnGlobalApplicationMenuPressUp(ViveControllerModule.EventData eventData) {
            buttondown = false;
            right.buttondown = false;
        }
    }
}
