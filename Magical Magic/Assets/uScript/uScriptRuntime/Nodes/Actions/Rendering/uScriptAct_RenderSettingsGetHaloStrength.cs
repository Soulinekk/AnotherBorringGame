// uScript Action Node
// (C) 2012 Detox Studios LLC

using UnityEngine;
using System.Collections;

[NodePath("Actions/Rendering/Render Settings")]

[NodeCopyright("Copyright 2011 by Detox Studios LLC")]
[NodeToolTip("Returns the current light halo strength used by the renderer.")]
[NodeAuthor("Detox Studios LLC", "http://www.detoxstudios.com")]
[NodeHelp("http://docs.uscript.net/#3-Working_With_uScript/3.4-Nodes.htm")]

[FriendlyName("Get Halo Strength", "Returns the current light halo strength used by the renderer.")]
public class uScriptAct_RenderSettingsGetHaloStrength : uScriptLogic
{
   public bool Out { get { return true; } }

   public void In([FriendlyName("Value", "The current value of the light halo strength used by the renderer.")] out float currentHaloStrength)
   {
      currentHaloStrength = RenderSettings.haloStrength;
   }
}