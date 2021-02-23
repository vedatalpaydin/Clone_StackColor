using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chibiKick : MonoBehaviour
{
  [SerializeField] PlayerController pc;
  private bool kicked;
  public void setParent()
  {
    kicked = true;
    pc.GetKick();
    transform.parent = null;
  }

  public void SetPos()
  {
    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
  }

  public bool getKick()
  {
    return kicked;
  }
  
}
