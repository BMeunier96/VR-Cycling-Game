﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System;
using UnityEngine.Events;

namespace Valve.VR
{
    [Serializable]
    public class SteamVR_Behaviour_Pose_TrackingChangedEvent : UnityEvent<SteamVR_Behaviour_Pose, SteamVR_Input_Sources, ETrackingResult> { }
}