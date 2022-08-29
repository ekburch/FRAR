using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class PanelStateManager : MonoBehaviour
    {
        public static PanelStateManager Instance = null;

        PanelBaseState currentState;
        public PanelEntryState EntryState = new PanelEntryState();
        public PanelOperationState OperationState = new PanelOperationState();
        public PanelCavitationState CavitationState = new PanelCavitationState();
        public PanelFailState FailState = new PanelFailState();
        public PanelWinState WinState = new PanelWinState();

        //Master discharge as float
        //Bools for other stuff

        public enum PanelItem { DISCHARGE_LEVEL, THROTTLE_LEVEL, TANK_TO_PUMP_STATUS }
        public Dictionary<PanelItem, float> worldState;

        //Make helper to reference this
        //stuff to retrieve values in dict

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"PanelStateManager().Awake: {Instance} already exists, destroying duplicate.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            worldState = new Dictionary<PanelItem, float>();
            foreach(int i in Enum.GetValues(typeof(PanelItem)))
            {
                worldState.Add((PanelItem)i, 0.0f);
            }
        }

        private void Start()
        {
            currentState = EntryState;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.UpdateState(this);
        }

        public void Onclick()
        {
            currentState.OnClick(this);
        }

        public void ChangeState(PanelBaseState state)
        {
            currentState = state;
            state.EnterState(this);
        }

        public bool GetWorldStateBool(PanelItem item)
        {
            if (Mathf.Approximately(worldState[item], 1.0f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public float GetWorldStateValue(PanelItem item)
        {
            return worldState[item];
        }
    }
}
