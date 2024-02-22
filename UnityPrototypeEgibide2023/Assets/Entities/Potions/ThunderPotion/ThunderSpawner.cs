using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSpawner : MonoBehaviour
{
	public GameObject thunderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnThunder());
    }
    
    IEnumerator SpawnThunder()
    {
    	while(true)
    	{
    		yield return new WaitForSeconds(Random.Range(1, 3));
    		Instantiate(thunderPrefab, new Vector3(Random.Range(-10, 10) + transform.position.x,  transform.position.y, Random.Range(0, 10)), Quaternion.identity);
    	}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
