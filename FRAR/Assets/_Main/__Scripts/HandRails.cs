using FRAR.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class HandRails : HandRailsBase
    {
        [SerializeField] GameObject m_objOnRails;
        protected override void OnLocationUpdated()
        {
            m_objOnRails.transform.position = PointOnLine;
        }
    }
}