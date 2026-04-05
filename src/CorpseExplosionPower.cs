using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace CorpseExplosion
{
    public class CorpseExplosionPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private static readonly AccessTools.FieldRef<RunManager, RunState?> StateRef =
            AccessTools.FieldRefAccess<RunManager, RunState?>("<State>k__BackingField");

        public override async Task AfterRemoved(Creature creature)
        {
            await Cmd.CustomScaledWait(0.5f, 0.75f, true);
            Player p = LocalContext.GetMe(StateRef(RunManager.Instance))!;
            await CreatureCmd.Damage(
                new ThrowingPlayerChoiceContext(),
                p.Creature.CombatState!.HittableEnemies,
                Amount * creature.MaxHp,
                ValueProp.Unpowered,
                p.Creature,
                null
            );
        }
    }
}
