using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory.Gun;
using UnityEngine;

namespace Axlebolt.Standoff.Fixes
{
    public class WeaponFix : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ApplyFixes()
        {
            FixBulletTraces();
            FixWeaponDamage();
        }

        private static void FixBulletTraces()
        {
            try
            {
                var traceParams = Resources.Load<BulletTraceEffectParams>("effects/BulletTraceParams");
                if (traceParams != null)
                {
                    var field = typeof(BulletTraceEffectParams).GetField("_tracerParamsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (field != null)
                    {
                        var list = field.GetValue(traceParams) as System.Collections.IList;
                        if (list != null)
                        {
                            foreach (var obj in list)
                            {
                                var p = obj as BulletTraceEffectParams.TracerParams;
                                if (p != null && p.Speed <= 0.1f)
                                {
                                    p.Speed = 600f;
                                    Debug.Log($"[WeaponFix] Fixed tracer {p.TraceType} speed 0 -> 600");
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) { Debug.LogWarning("[WeaponFix] BulletTrace fix failed: " + e.Message); }
        }

        private static void FixWeaponDamage()
        {
            try
            {
                // UMP45 SMG - fix zero damage
                var ump = Resources.Load<GunParameters>("weapons/ump45/UMP45");
                if (ump != null) FixGun(ump, "UMP45", 35, 30, 30, 25);
                var sm = Resources.Load<GunParameters>("weapons/sm1014/SM1014");
                if (sm != null) FixGun(sm, "SM1014", 18, 15, 18, 13);
            }
            catch (System.Exception e) { Debug.LogWarning("[WeaponFix] Weapon damage fix failed: " + e.Message); }
        }

        private static void FixGun(GunParameters gun, string name, int head, int chest, int stomach, int legs)
        {
            try
            {
                var dmg = gun.Damage;
                bool needFix = dmg.HeadDamage == 0 && dmg.ChestAndArmsDamage == 0;
                if (needFix)
                {
                    // Use reflection to set private fields
                    var t = dmg.GetType();
                    var fHead = t.GetField("_headDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var fChest = t.GetField("_chestAndArmsDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var fStomach = t.GetField("_stomachDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var fLegs = t.GetField("_legsDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (fHead != null) fHead.SetValue(dmg, head);
                    if (fChest != null) fChest.SetValue(dmg, chest);
                    if (fStomach != null) fStomach.SetValue(dmg, stomach);
                    if (fLegs != null) fLegs.SetValue(dmg, legs);
                    Debug.Log($"[WeaponFix] Fixed {name} damage {head}/{chest}/{stomach}/{legs}");
                }
                // Fix trace type if Submachine (5) which had speed 0 originally, change to Smg (2)
                if (gun.BulletTraceType == BulletTraceType.Submachine)
                {
                    // Use reflection for private field
                    var tf = gun.GetType().GetField("_bulletTraceType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (tf != null) tf.SetValue(gun, BulletTraceType.Smg);
                    Debug.Log($"[WeaponFix] Fixed {name} trace Submachine->Smg");
                }
            }
            catch (System.Exception e) { Debug.LogWarning($"[WeaponFix] FixGun {name} failed: {e.Message}"); }
        }
    }
}
