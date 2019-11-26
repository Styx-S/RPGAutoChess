using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAttrValue : MonoBehaviour
{
   void Awake() {
       ObjAttr attr = GetComponentInParent<ObjAttr>();
       attr.Value = gameObject;
   }
}
