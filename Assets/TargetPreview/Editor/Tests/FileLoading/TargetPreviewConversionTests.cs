using System.Linq;
using AudicaTools;
using NUnit.Framework;
using TargetPreview.Scripts.Targets;
using UnityEngine;


public class TargetPreviewConversionTests : RequireMockFileTest
{
    [Test]
    public void Test_cues_to_targetcues_conversion()
    {
        audica.expert.cues
            .TakeLast(10)
            .ToList()
            .ForEach(cue =>
            {
                TargetCue targetCue = (TargetCue)cue;
                Assert.AreEqual(targetCue.timeMs, TempoData.TickToMilliseconds(cue.tick, audica.tempoData), "Time in ms hasn't been converted."); 
                Assert.AreEqual(targetCue.tick, cue.tick, "Tick hasn't been converted.");
                Assert.AreEqual(new Cue.GridOffset(){x = targetCue.xOffset, y = targetCue.yOffset}, cue.gridOffset, "Grid offset hasn't been converted.");
                Assert.AreEqual(targetCue.behavior.ToString(), cue.behavior.ToString(), "Target behavior hasn't been converted.");
                Assert.AreEqual(targetCue.handType.ToString(), cue.handType.ToString(), "Target hand type hasn't been converted.");
            });
    }
}