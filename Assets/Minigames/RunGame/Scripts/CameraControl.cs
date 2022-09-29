using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class CameraControl : MonoBehaviour
    {
        public GameObject Camera;
        public GameObject Caracter;
        float rotTime;
        float maxRotTime;
        Vector3 CameraRot;

        // Start is called before the first frame update
        void Start()
        {
            rotTime = 0;
            maxRotTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            rotTime += Time.deltaTime;

            if (GameHaven.RunGame.GameManager.gameTime > 10 && rotTime < maxRotTime)
            {
                Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, Quaternion.Euler(CameraRot), 0.1f* Time.deltaTime);
                Caracter.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, Quaternion.Euler(CameraRot), 0.1f * Time.deltaTime);
            }
            else if (GameHaven.RunGame.GameManager.gameTime > 10 && rotTime > maxRotTime)
            {
                CameraRot = new Vector3(0, 0, Random.Range(0, 360));
                maxRotTime = Random.Range(0, 4f);
                rotTime = 0;
            }

        }
    }
}