using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopulator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> PrecinctList;
    [SerializeField]
    List<float> CrimeLevels;
    [SerializeField]
    List<float> CrimeDensities;

    [SerializeField]
    GameObject Criminal;

    public int Denominator;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject precinct in PrecinctList)
        {
            //Get the index (precincts should all be ordered the same among lists
            int index = PrecinctList.IndexOf(precinct);
            BoxCollider precinctCollider = precinct.GetComponent<BoxCollider>();
            //CrimeLevels[index]
            //CrimeDensities[index]

            //Calculate the # of criminals to spawn using the crime level and density in that precinct
            int criminalCount = (int)(((CrimeDensities[index])/Denominator) * CrimeLevels[index]);

            //For loop that runs a # of times dependent on the Crime Level + Density
            //Spawns an enemy in a random spot within the precinct
            for (int i = 0; i < criminalCount; i++)
            {
                //Pick a random point in the precinct
                Vector3 criminalSpawnPoint = Vector3.zero;
                bool pointFound = false;
                while(!pointFound)
                {
                    //Generate point
                    criminalSpawnPoint = new Vector3(Random.Range(-3526, 2564), 200, Random.Range(-10708, 10900));

                    //Check that the point lies within the precincts bounds
                    pointFound = PointInOABB(criminalSpawnPoint, precinctCollider);

                }

                //Instantiate a criminal in that point
                Instantiate(Criminal, criminalSpawnPoint, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Method from https://answers.unity.com/questions/53989/test-to-see-if-a-vector3-point-is-within-a-boxcoll.html
    bool PointInOABB(Vector3 point, BoxCollider box)
    {
        point = box.transform.InverseTransformPoint(point) - box.center;

        float halfX = (box.size.x * 0.5f);
        float halfY = (box.size.y * 0.5f);
        float halfZ = (box.size.z * 0.5f);
        if (point.x < halfX && point.x > -halfX &&
           point.y < halfY && point.y > -halfY &&
           point.z < halfZ && point.z > -halfZ)
            return true;
        else
            return false;
    }
}
