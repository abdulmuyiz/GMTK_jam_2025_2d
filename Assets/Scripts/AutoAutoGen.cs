using System.Collections;
using UnityEngine;

public class AutoAutoGen : MonoBehaviour
{
    private GameObject duplicate;
    private int count = 0;

    public GameObject parent;

    private void Update()
    {
        Debug.Log(count);
        if(count == 0)
        {
            StartCoroutine(auto());
        }
        else
        {
            this.enabled = false;
        }

    }

    private IEnumerator auto()
    {
        if (count == 0)
        {
            yield return new WaitForSeconds(0.1f);
            duplicate = GameObject.FindGameObjectWithTag("ChestGroup");
            GameObject one = Instantiate(duplicate, Vector3.zero, Quaternion.Euler(0, 0, 90));
            GameObject two =Instantiate(duplicate, Vector3.zero, Quaternion.Euler(0, 0, 180));
            GameObject three = Instantiate(duplicate, Vector3.zero, Quaternion.Euler(0, 0, 270));
            one.transform.SetParent(parent.transform);
            two.transform.SetParent(parent.transform);
            three.transform.SetParent(parent.transform);
        }
        count++;
    }
}
