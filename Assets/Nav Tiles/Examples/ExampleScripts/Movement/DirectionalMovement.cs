using System.Collections;
using System.Collections.Generic;
using NavigationTiles.Entities;
using UnityEngine;

namespace NavigationTiles.Examples
{
    public class DirectionalMovement : MonoBehaviour
    {
        private Agent _agent;
        
        // Start is called before the first frame update
        private void Awake()
        {
            _agent = GetComponent<Agent>();
        }

        // Update is called once per frame
        void Update()
        {
            //this is not how you should implement your movement code.
            //don't use this script.
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _agent.TryMoveInDirection(Vector3Int.up);
            }else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _agent.TryMoveInDirection(Vector3Int.down);
            }else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _agent.TryMoveInDirection(Vector3Int.left);
            }else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _agent.TryMoveInDirection(Vector3Int.right);
            }
        }
    }
}
