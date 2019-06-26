using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePunch : MonoBehaviour {

    public float unitDistance;
    public float timeDelay;

    public void Start()
    {
        StartCoroutine(Startup());
    }


    IEnumerator Punch()
    {
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();
        transform.Translate(Vector3.up * unitDistance, Space.World);
        yield return new WaitForFixedUpdate();

        for (int i = 0; i < 20; i++)
        {
            transform.Translate(Vector3.down * unitDistance *(10f/20f) , Space.World);
            yield return new WaitForSeconds(.02f);
        }
        StartCoroutine(Punch());
    }

    IEnumerator Startup()
    {
        yield return new WaitForSeconds(timeDelay);
        StartCoroutine(Punch());
    }
}
