﻿using OverlayMod.Stat.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayMod.Stat.Preview.PreviewStats
{
    internal class RankStatPreview : PreviewStat
    {
        protected override Stats.Stat parentStat => RankStat.Instance;

        protected override string text => "SS";
    }
}
