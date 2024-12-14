public class Occupation
{
	private Entity Entity;
	private PlayerRelation Relation;

	public Occupation( Entity entity, PlayerRelation relation )
	{
		Entity = entity;
		Relation = relation;
	}

	public Entity GetEntity()
	{
		return Entity;
	}

	public PlayerRelation GetRelation() 
	{ 
		return Relation;
	}
}
