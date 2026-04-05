using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace CorpseExplosion;

[Pool(typeof(SilentCardPool))]
public class CorpseExplosion()
    : CustomCardModel(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(6)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        NPoisonImpactVfx child = NPoisonImpactVfx.Create(cardPlay.Target!)!;
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await CreatureCmd.TriggerAnim(
            Owner.Creature,
            CreatureAnimator.castTrigger,
            Owner.Character.CastAnimDelay
        );

        await PowerCmd.Apply<PoisonPower>(
            cardPlay.Target!,
            DynamicVars.Poison.BaseValue,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<CorpseExplosionPower>(cardPlay.Target!, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(3);
    }
}
