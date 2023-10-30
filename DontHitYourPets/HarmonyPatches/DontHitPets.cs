using HarmonyLib;
using PetAI;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

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

    [HarmonyPatch(typeof(ItemPetWhistle), "notifyNearbyPets")]
    public static class DontTargetYourPets
    {
        private static bool Prefix(ItemPetWhistle __instance, EntityAgent byEntity)
        {
            var giveBehavior = byEntity.GetBehavior<EntityBehaviorGiveCommand>();
            if (giveBehavior == null) return false;

            var command = giveBehavior.activeCommand;
            if (command == null) return false;

            if (byEntity is EntityPlayer player && command.commandName == "settarget")
            {
                EntitySelection entitySel = null;
                BlockSelection blockSel = null;
                Vec3d pos = player.Pos.XYZ.Add(player.LocalEyePos);
                player.World.RayTraceForSelection(pos, player.SidedPos.Pitch, player.SidedPos.Yaw, 50, ref blockSel, ref entitySel);

                if (entitySel?.Entity is EntityPet pet && pet.GetBehavior<EntityBehaviorTameable>().domesticationLevel != DomesticationLevel.WILD)
                {
                    return false;
                }
            }

            return true;
        }
    }
}