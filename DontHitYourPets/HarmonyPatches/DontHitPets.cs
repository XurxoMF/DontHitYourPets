using HarmonyLib;
using PetAI;
using Vintagestory.API.Common;

namespace DontHitYourPets.HarmonyPatches
{
    [HarmonyPatch(typeof(EntityPet), nameof(EntityPet.ShouldReceiveDamage))]
    public static class DontHitYourPetsPatch
    {
        public static bool Prefix(EntityPet __instance, ref bool __result, DamageSource damageSource, float damage)
        {
            if (PetConfig.Current.pvpOff
                && __instance.GetBehavior<EntityBehaviorTameable>()?.domesticationLevel != DomesticationLevel.WILD
                && (damageSource.CauseEntity is EntityPlayer || damageSource.SourceEntity is EntityPlayer))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}