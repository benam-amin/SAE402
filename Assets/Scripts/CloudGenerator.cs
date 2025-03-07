using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] listCloudsPrefab;
    private Vector3 startPos;
    [SerializeField]
    private Transform endPos;
    [SerializeField]
    private int nbCloudsToGenerate = 5;

    private List<GameObject> listCloudsGenerated = new List<GameObject>();
    // Start is called before the first frame update
    IEnumerator Start()
    {
        GenerateClouds();

        int index = 0;
        GameObject currentCloud;
        float quarter = ScreenUtility.Instance.height * 0.25f;

        while (true)
        {
            if (!listCloudsGenerated.All(go => go.activeSelf))
            {
                startPos = transform.position;

                currentCloud = listCloudsGenerated[index];
                currentCloud.transform.position = new Vector3(startPos.x, Random.Range(ScreenUtility.Instance.Top - quarter, ScreenUtility.Instance.Top + quarter), 0);
                currentCloud.SetActive(true);
                currentCloud.GetComponent<Cloud>().endPos = endPos.position;

                yield return new WaitForSeconds(0.75f);

                index = (index + 1) % listCloudsGenerated.Count;
            }

            yield return null;
        }
    }

    private void GenerateClouds()
    {
        for (var i = 0; i < nbCloudsToGenerate; i++)
        {
            GameObject cloud = Instantiate(listCloudsPrefab[0]);

            listCloudsGenerated.Add(cloud);
            cloud.SetActive(false);
        }
    }
}
