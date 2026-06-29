using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class MonsterTests_EditMode
{
    private const int BaseAttackPower = 20;

    [Test]
    public void Monster_Constructor_ValidAttackPower_DoesNotThrowTest()
    {
        Assert.DoesNotThrow(() => new Monster(BaseAttackPower));
    }

    [Test]
    public void Monster_Constructor_ZeroAttackPower_DoesNotThrowTest()
    {
        Assert.DoesNotThrow(() => new Monster(0));
    }

    [Test]
    public void Monster_Constructor_NegativeAttackPower_DoesNotThrowTest()
    {
        Assert.DoesNotThrow(() => new Monster(-100));
    }

    [Test]
    public void Monster_AttackPower_NormalTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var monster = new Monster(BaseAttackPower);
            Assert.Greater(monster.AttackPower, 0);
        }
    }

    [Test]
    public void Monster_AttackPower__IsInRangeTest()
    {
        int enemy = BaseAttackPower;
        int expectedMin = enemy - 15;
        int expectedMax = enemy + 15;

        for (int i = 0; i < 10; i++)
        {
            var monster = new Monster(enemy);
            Assert.GreaterOrEqual(monster.AttackPower, expectedMin);
            Assert.LessOrEqual(monster.AttackPower, expectedMax);
        }
    }

    [Test]
    public void Monster_AttackPower_IsAtLeastFiveTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var monster = new Monster(0);
            Assert.GreaterOrEqual(monster.AttackPower, 0);
        }
    }

    [Test]
    public void Monster_Reward_IsInRangeTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var monster = new Monster(BaseAttackPower);
            Assert.That(monster.Reward, Is.InRange(1, 4));
        }
    }
}