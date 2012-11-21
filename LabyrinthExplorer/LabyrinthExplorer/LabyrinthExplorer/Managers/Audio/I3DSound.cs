using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public interface I3DSound
    {
        AudioEmitter GetAudioEmitter();
    }

    public interface I3DSoundCustDivFact : I3DSound
    {
        float GetCustomDivisionFactor();
    }
}
