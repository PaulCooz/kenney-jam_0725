using System;
using Random = UnityEngine.Random;

namespace JamSpace
{
    public static class PowerUps
    {
        private static int _moveSpeedScale;
        private static int _jumpGravityScale;
        private static int _jumpForceScale;
        private static int _shootDamageAdd;
        private static int _shootIntervalScale;
        private static int _shootBulletSizeScale;

        public static readonly PowerUp[] All =
        {
            new()
            {
                Name = "MoveSpeedScale", Prob = 9,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _moveSpeedScale = Random.Range(1, 10 + 1);
                    return $"+{_moveSpeedScale * 10}% to speed";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.moveSpeedScale += _moveSpeedScale / 10f;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "JumpGravityScale", Prob = 1,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _jumpGravityScale = Random.Range(-5, +5 + 1);
                    if (_jumpGravityScale == 0) _jumpGravityScale = -1;
                    return $"{(_jumpGravityScale > 0 ? "+" : "")}{_jumpGravityScale * 10}% to player gravity";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.jumpGravityScale += _jumpGravityScale / 10f;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "JumpForceScale", Prob = 1,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _jumpForceScale = Random.Range(1, 2 + 1);
                    return $"+{_jumpForceScale * 10}% to jump force";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.jumpForceScale += _jumpForceScale / 10f;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "ShootDamageAdd", Prob = 10,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _shootDamageAdd = Random.Range(1, 3);
                    return $"+{_shootDamageAdd} to damage";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.shootDamageAdd += _shootDamageAdd;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "ShootIntervalScale", Prob = 8,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _shootIntervalScale = Random.Range(1, 7 + 1);
                    return $"-{_shootIntervalScale * 10}% to shoot interval";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.shootIntervalScale -= _shootIntervalScale / 10f;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "ShootBulletSizeScale", Prob = 15,
                CanChoose = _ => true,
                GetToChoose = () =>
                {
                    _shootBulletSizeScale = Random.Range(3, 10 + 1);
                    return $"{_shootBulletSizeScale * 10}% to bullet size";
                },
                Use = player =>
                {
                    var values = player.stateValues;
                    values.shootBulletSizeScale += _shootBulletSizeScale / 10f;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "ShootAimEnable", Prob = 7,
                CanChoose = player => !player.stateValues.shootAimEnable,
                GetToChoose = () => "add shoot aim",
                Use = player =>
                {
                    var values = player.stateValues;
                    values.shootAimEnable = true;
                    player.SaveValues();
                },
            },
            new()
            {
                Name = "HealthAdd", Prob = 10,
                CanChoose = _ => true,
                GetToChoose = () => "+1 max health",
                Use = player =>
                {
                    var values = player.stateValues;
                    values.maxHealthAdd++;
                    player.SaveValues();
                },
            },
        };

        public struct PowerUp
        {
            public string Name;
            public int Prob;
            public Func<PlayerState, bool> CanChoose;
            public Func<string> GetToChoose;
            public Action<PlayerState> Use;
        }
    }
}