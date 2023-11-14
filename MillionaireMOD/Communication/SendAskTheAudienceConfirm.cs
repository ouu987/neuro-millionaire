﻿using HarmonyLib;

namespace MillionaireMOD.Communication;

[HarmonyPatch]
public static class SendAskTheAudienceConfirm
{
    [HarmonyPatch(typeof(MusicDirector), nameof(MusicDirector.PlayMusicTrack), typeof(string), typeof(bool))]
    [HarmonyPatch(typeof(MusicDirector), nameof(MusicDirector.PlayMusicTrack), typeof(string), typeof(float))]
    [HarmonyPrefix, HarmonyPriority(Priority.First)]
    public static void Prefix(string MusicTrackName)
    {
        if (MusicTrackName != "AK_Event_Mus_Bonus_Public_Keyboard") return;

        PreventSkippingCustomLifelines.CanSkip = false;
        WebSocketConnection.Send(new WSMessage("millionaire/lifeline/ask_the_audience/confirm_start"));
    }
}
