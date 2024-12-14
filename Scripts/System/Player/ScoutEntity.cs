#nullable enable
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoutEntity : AbstractEntity
{
	[SerializeField]
	private string Description;
    [SerializeField]
    private float MovementSpeed;

    public ScoutEntity() : base()
    {
        Description = "";
        MovementSpeed = 0f;
    }

    protected ScoutEntity(string name, PlayerRelation playerRelation, Color color, string description, float movementSpeed) : base (name, playerRelation, color)
    {
        Description = description;
        MovementSpeed = movementSpeed;
    }

    

    public float GetMovementSpeed()
    {
        return MovementSpeed;
    }

    public static class Scouts
    {
        public static ScoutEntity NOVICE_SCOUTS = new("Novice Scouts", PlayerRelation.PLAYER, UIUtils.Colors.Gold, "", 1f);
        public static ScoutEntity VETERAN_SCOUTS = new("Veteran Scouts", PlayerRelation.PLAYER, UIUtils.Colors.Gold, "", 2f);
        public static ScoutEntity ROYAL_SCOUTS = new("Royal Scouts", PlayerRelation.PLAYER, UIUtils.Colors.Gold, "", 3f);

        public static List<ScoutEntity> GetAllScoutEntities()
        {
            return new List<ScoutEntity>()
            {
                NOVICE_SCOUTS,
                VETERAN_SCOUTS,
                ROYAL_SCOUTS
            };
        }

        public static ScoutEntity GetScoutEntityByName(string name)
        {
            foreach (ScoutEntity entity in GetAllScoutEntities())
            {
                if (entity.GetName() == name) return entity;
            }

            return NOVICE_SCOUTS;
        }
    }
}
