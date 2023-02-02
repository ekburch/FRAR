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

		public override void OnPointerClicked(MixedRealityPointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public override void OnPointerDown(MixedRealityPointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public override void OnPointerDragged(MixedRealityPointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public override void OnPointerUp(MixedRealityPointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		protected override void OnLocationUpdated()
        {
            m_objOnRails.transform.position = PointOnLine;
        }
    }
}