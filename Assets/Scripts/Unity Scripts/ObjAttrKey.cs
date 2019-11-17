using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAttrKey : MonoBehaviour
{
   void Awake() {
       ObjAttr attr = GetComponentInParent<ObjAttr>();
       attr.Key = gameObject;
   }
}
