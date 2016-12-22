using UnityEngine;
using System.Collections;
using System;
using FRL.IO;

namespace FRL.IO.FourD
{
    public class HyperCubeOBJ : GlobalReceiver, IGlobalTriggerPressSetHandler
    {

        //public SphereCollider trackballArea;
        //public BoxCollider interactionArea;

        private Transform box;
        private HyperCube cube;
        private Trackball trackball = new Trackball(4);

        private Vector3 A_ = new Vector3();
        private Vector3 B_ = new Vector3();
        private Vector4 A = new Vector4();
        private Vector4 B = new Vector4();

        private float radius;
        private bool isTriggerPressed = false;

        void Awake()
        {
            box = this.GetComponent<Transform>();
            cube = new HyperCube(box);
            UpdateRotation(cube, trackball, A, B);
        }

        Vector4 ReturnVector(Vector3 vector)
        {
            radius = 1f;
            Vector4 result;
            Vector3 relapos = new Vector3();
            relapos = (vector - box.position) * 8f / 3f;
            float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
            if (r < radius)
            {
                result = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
            }
            else
            {
                Vector3 Q = (radius / r) * relapos;
                relapos = Q + box.position;
                result = new Vector4(Q.x, Q.y, Q.z, 0f);
            }
            return result;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTriggerPressed)
            {
                A = ReturnVector(A_);
                B = ReturnVector(B_);
                UpdateRotation(cube, trackball, A, B);
                A_ = B_;
            }
        }

        void UpdateRotation(HyperCube cube, Trackball trackball, Vector4 A_, Vector4 B_)
        {
            float[] A = new float[4] { A_.x, A_.y, A_.z, A_.w };
            float[] B = new float[4] { B_.x, B_.y, B_.z, B_.w };
            trackball.rotate(A, B);
            for (int i = 0; i < 16; i++)
            {
                float[] src = new float[4];
                src[0] = cube.srcVertices[i].x;
                src[1] = cube.srcVertices[i].y;
                src[2] = cube.srcVertices[i].z;
                src[3] = cube.srcVertices[i].w;
                float[] dst = new float[4];
                trackball.transform(src, dst);
                cube.updatepoint4(dst, i);
                cube.update_edges();
            }
        }

        public void OnGlobalTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            if (module == null || module.Equals(eventData.module))
          //      && interactionArea.bounds.Contains(eventData.module.transform.position))
            {
                module = eventData.module;
                A_ = module.transform.position;
                B_ = module.transform.position;
                isTriggerPressed = true;
            }
        }

        public void OnGlobalTriggerPress(ViveControllerModule.EventData eventData)
        {
            if (eventData.module.Equals(this.module))
            {
                B_ = eventData.module.transform.position;
                //if (!interactionArea.bounds.Contains(eventData.module.transform.position))
                //{
                //    module = null;
                //    isTriggerPressed = false;
                //}
            }
        }

        public void OnGlobalTriggerPressUp(ViveControllerModule.EventData eventData)
        {

            if (eventData.module.Equals(this.module))
            {
                isTriggerPressed = false;
                module = null;
            }
        }
    }

    internal class HyperCube
    {
        int size;
        GameObject[] edges;
        public Vector4[] srcVertices;
        public Vector4[] vertices;
        Vector2[] index;
        Transform parentobj;

        void setparent(Transform par)
        {
            for (int i = 0; i < size; i++)
            {
                edges[i].transform.parent = par;
            }
        }

        public HyperCube(Transform par)
        {
            parentobj = par;
            size = 32;
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2)
                        {
                            vertices[n] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
                            srcVertices[n++] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
                        }
            index = new Vector2[32];
            index[0] = new Vector2(0, 1);
            index[1] = new Vector2(2, 3);
            index[2] = new Vector2(4, 5);
            index[3] = new Vector2(6, 7);
            index[4] = new Vector2(8, 9);
            index[5] = new Vector2(10, 11);
            index[6] = new Vector2(12, 13);
            index[7] = new Vector2(14, 15);
            index[8] = new Vector2(0, 2);
            index[9] = new Vector2(1, 3);
            index[10] = new Vector2(4, 6);
            index[11] = new Vector2(5, 7);
            index[12] = new Vector2(8, 10);
            index[13] = new Vector2(9, 11);
            index[14] = new Vector2(12, 14);
            index[15] = new Vector2(13, 15);
            index[16] = new Vector2(0, 4);
            index[17] = new Vector2(1, 5);
            index[18] = new Vector2(2, 6);
            index[19] = new Vector2(3, 7);
            index[20] = new Vector2(8, 12);
            index[21] = new Vector2(9, 13);
            index[22] = new Vector2(10, 14);
            index[23] = new Vector2(11, 15);
            index[24] = new Vector2(0, 8);
            index[25] = new Vector2(1, 9);
            index[26] = new Vector2(2, 10);
            index[27] = new Vector2(3, 11);
            index[28] = new Vector2(4, 12);
            index[29] = new Vector2(5, 13);
            index[30] = new Vector2(6, 14);
            index[31] = new Vector2(7, 15);

            edges = new GameObject[32];

            for (int i = 0; i < size; i++)
            {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                GameObject segObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                MonoBehaviour.Destroy(segObj.GetComponent<Collider>());
                segObj.transform.position = pos;
                segObj.transform.LookAt(endpoint_);
                segObj.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
                segObj.transform.localScale = new Vector3(0.01f, distance, 0.01f);
                edges[i] = segObj;
            }

            // This is for testing the algorithm
            setparent(par);
        }

        public Vector3 get3dver(int i)
        {
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            return rst;
        }
        public void returnpoint4(float[] src, int i)
        {
            src[0] = (float)vertices[i].x;
            src[1] = (float)vertices[i].y;
            src[2] = (float)vertices[i].z;
            src[3] = (float)vertices[i].w;
        }

        public void updatepoint4(float[] src, int i)
        {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }

        public void update_edges()
        {


            for (int i = 0; i < 32; i++)
            {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                Quaternion rot = new Quaternion();
                if (distance != 0)
                {
                    rot = Quaternion.LookRotation(beginpoint_ - endpoint_);
                    //Debug.Log(rot);
                }
                edges[i].transform.localPosition = pos;
                edges[i].transform.LookAt(endpoint_);
                edges[i].transform.rotation = rot;
                edges[i].transform.localScale = new Vector3(0.01f, 0.01f, distance);
            }
        }

    }

}

