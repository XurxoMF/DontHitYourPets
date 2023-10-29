using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace DontHitYourPets
{
    public class DontHitYourPetsModSystem : ModSystem
    {
        public static Harmony HarmonyInstance { get; set; } = new Harmony("dhyp");

        public override bool AllowRuntimeReload => true;

        public override bool ShouldLoad(EnumAppSide side)
        {
            if (side == EnumAppSide.Server) return true;
            return false;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            // Apply all the Harmony patches.
            HarmonyInstance.PatchAll();
        }

        public override void Dispose()
        {
            HarmonyInstance.UnpatchAll();

            base.Dispose();
        }
    }
}
