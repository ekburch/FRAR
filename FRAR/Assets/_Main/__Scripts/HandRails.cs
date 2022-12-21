using FRAR.Utils;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class HandRails : HandRailsBase
    {
        [SerializeField] GameObject m_objOnRails;

        public override void OnFocusEnter(FocusEventData eventData)
        {

        }

        public override void OnFocusExit(FocusEventData eventData)
        {

        }

        protected override void OnLocationUpdated()
        {
            m_objOnRails.transform.position = PointOnLine;
        }
    }
}