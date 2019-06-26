using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetachedEgg : MonoBehaviour
{

    public bool Eggtime;
    public GameObject Car;
    public float boostPower;
    public ParticleSystem Exhast;
    public float rotateSpeed;
    public float timeSinceDetach;

    public void Start()
    {
        Eggtime = false;
        StartCoroutine(EggSetup());
    }
    public IEnumerator EggSetup()
    {
        while (!Eggtime)
        {
            yield return null;
        }
        Rigidbody Eggbody = this.gameObject.AddComponent<Rigidbody>();
        Eggbody.mass = 8000;
        Eggbody.useGravity = true;
        Eggbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Eggbody.velocity = Car.GetComponent<Rigidbody>().velocity;

        MeshCollider EggMesh = transform.Find("EggCollider").gameObject.AddComponent<MeshCollider>();
        EggMesh.convex = true;
        timeSinceDetach = Time.time;
        StartCoroutine(BeginBehavior());
    }

    public IEnumerator BeginBehavior()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForFixedUpdate();
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 70f), ForceMode.VelocityChange);
        }
        while (true)
        {
            ApplyBoost();
            ApplyRotation();
            yield return null;
        }
    }

    public void ApplyBoost()
    {
        if (Input.GetButton(SavedInputs.boost))
        {
            Vector3 boostForce = new Vector3(0, 0, boostPower);
            GetComponent<Rigidbody>().AddRelativeForce(boostForce, ForceMode.Force);

            AudioManager.instance.Play("Boost");
            Exhast.Play();
        }
        else
        {
            AudioManager.instance.Stop("Boost");
            Exhast.Stop();
        }
    }
    public void ApplyRotation()
    {
        Vector3 yRotate = new Vector3(0, rotateSpeed * Input.GetAxis("L_YAxis_1"), 0);
        transform.Rotate(yRotate);
        Vector3 xRotate = new Vector3(rotateSpeed * Input.GetAxis("L_XAxis_1"), 0, 0);
        transform.Rotate(xRotate);

    }

    public float SecondsSinceDetached()
    {
        return (Time.time - timeSinceDetach);
    }
}
