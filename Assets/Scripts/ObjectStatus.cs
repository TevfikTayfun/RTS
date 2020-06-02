using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ObjectStatus : MonoBehaviour
{
    public NodeManager.ResourceTypes heldResourceType;


    public bool isSelected = false;
    public bool isGathering = false;

    public string objectName;
    private NavMeshAgent agent;

    public int heldResource;
    public int maxHeldResource;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GatherTick());
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(heldResource >= maxHeldResource)
        {

        }
        if(Input.GetMouseButtonDown(1) && isSelected)
        {
            MoveClick();
        }
    }

    public void MoveClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100 ))
        {
            if(hit.collider.tag == "Ground" && isSelected)
            {
                agent.destination = hit.point;
                Debug.Log("Movinggg");
            }

            else if(hit.collider.tag == "Resource" && isSelected)
            {
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log("Harvesting?");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if(hitObject.tag == "Resource")
        {
            isGathering = true;
            hitObject.GetComponent<NodeManager>().gatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if(hitObject.tag == "Resource")
        {
            hitObject.GetComponent<NodeManager>().gatherers--;
        }
    }

    IEnumerator GatherTick()
    {
        while(true)
        {
            if(isGathering)
            {
                heldResource++; 
            }
            yield return new WaitForSeconds(1);
            
        }
    }
}
