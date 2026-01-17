using UnityEngine;
using VertigoCase.Systems.ZoneSystem;

namespace VertigoCase.Systems.InfoSystem
{
    public sealed class ZoneColorResolver
    {
        public Color GetLevelColor(int level)
        {
            if (level % ZoneManager.Instance.IntervalLevelBySuperZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Super).slidePanelLevelPointColor;

            if (level % ZoneManager.Instance.IntervalLevelBySafeZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Safe).slidePanelLevelPointColor;

            return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Normal).slidePanelLevelPointColor;
        }
        public Color GetLevelTextColor(int level)
        {
            if (level % ZoneManager.Instance.IntervalLevelBySuperZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Super).slidePanelLevelTextCursorColor;

            if (level % ZoneManager.Instance.IntervalLevelBySafeZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Safe).slidePanelLevelTextCursorColor;

            return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Normal).slidePanelLevelTextCursorColor;
        }
    }
}
